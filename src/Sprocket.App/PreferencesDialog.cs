using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Layout;
using Avalonia.Media;

namespace Sprocket.App;

/// <summary>
/// The application Preferences dialog (PLAN.md step 38) — the user-scoped settings surface, distinct from
/// per-sequence <c>SequenceSettingsDialog</c> and the per-project <c>ProjectSettings</c>. Code-built against
/// the shared dark <see cref="Palette"/> like the other modals in <c>Dialogs.cs</c> (its own file — Dialogs.cs
/// is already large). Look/behaviour rests on manual verification; the decision logic (parsing, clamping,
/// byte formatting, token resolution, the MCP setup command) lives in the tested <see cref="PreferencesFormat"/>
/// and <see cref="UserSettingsStore"/>.
///
/// Cache "Clear" buttons act immediately (after a confirm) rather than on OK — the standard NLE behavior for
/// cache management — so Cancel does not undo them.
/// </summary>
internal static class PreferencesDialog
{
    /// <summary>
    /// Shows the dialog over <paramref name="current"/>. The cache callbacks report/clear the proxy cache
    /// (global) and the render cache (the current session's store — render files live beside the project).
    /// Returns the clamped new settings on OK, or <see langword="null"/> on cancel.
    /// </summary>
    public static Task<UserSettings?> Show(Window owner, UserSettings current,
        Func<long> proxyCacheSize, Func<int> clearProxyCache,
        Func<long> renderCacheSize, Action clearRenderCache,
        Func<long> analysisCacheSize, Action clearAnalysisCache,
        IReadOnlyList<string> audioDevices)
    {
        bool italian = AppLocalization.Normalize(current.Language) == AppLocalization.Italian;
        string T(string english, string italianText) => italian ? italianText : english;

        // ── Audio output device ─────────────────────────────────────────────────────────────────
        // "System Default" ("" specifier) plus the enumerated devices. Keep the persisted device selectable even
        // when it isn't currently enumerated (unplugged) so leaving the dialog doesn't silently discard the choice.
        var deviceLabels = new List<string> { "System Default" };
        var deviceSpecs = new List<string> { "" };
        foreach (string d in audioDevices)
        {
            deviceLabels.Add(d);
            deviceSpecs.Add(d);
        }
        if (current.AudioOutputDevice.Length > 0 && !deviceSpecs.Contains(current.AudioOutputDevice))
        {
            deviceLabels.Add($"{current.AudioOutputDevice} (not connected)");
            deviceSpecs.Add(current.AudioOutputDevice);
        }
        var audioDevice = new ComboBox
        {
            ItemsSource = deviceLabels,
            SelectedIndex = Math.Max(0, deviceSpecs.IndexOf(current.AudioOutputDevice)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Foreground = Palette.TextBrush,
            Background = Palette.PanelBgBrush,
            FontSize = Typography.Body,
        };

        // ── Interface language ────────────────────────────────────────────────────────────────
        var language = new ComboBox
        {
            ItemsSource = new[] { "Italiano", "English" },
            SelectedIndex = AppLocalization.Normalize(current.Language) == AppLocalization.English ? 1 : 0,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Foreground = Palette.TextBrush,
            Background = Palette.PanelBgBrush,
            FontSize = Typography.Body,
        };

        // ── Caches ──────────────────────────────────────────────────────────────────────────────
        var proxySizeText = Muted(PreferencesFormat.Bytes(proxyCacheSize()));
        var renderSizeText = Muted(PreferencesFormat.Bytes(renderCacheSize()));
        var analysisSizeText = Muted(PreferencesFormat.Bytes(analysisCacheSize()));
        Button clearProxy = SmallButton(T("Clear", "Svuota"));
        Button clearRender = SmallButton("Clear");
        Button clearAnalysis = SmallButton("Clear");

        // ── Export metadata defaults ────────────────────────────────────────────────────────────
        TextBox title = MakeText(current.ExportTitle);
        TextBox author = MakeText(current.ExportAuthor);
        TextBox copyright = MakeText(current.ExportCopyright);
        TextBox comment = MakeText(current.ExportComment);

        // ── Autosave ────────────────────────────────────────────────────────────────────────────
        TextBox autosave = MakeText(current.AutosaveIntervalSeconds.ToString());
        autosave.Width = 80;
        autosave.HorizontalAlignment = HorizontalAlignment.Left;

        // ── Import (PLAN.md step 42) ──────────────────────────────────────────────────────────────
        TextBox stillDuration = MakeText(
            current.StillImageDefaultSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture));
        stillDuration.Width = 80;
        stillDuration.HorizontalAlignment = HorizontalAlignment.Left;

        // ── Updates (PLAN.md steps 36 + 45) ─────────────────────────────────────────────────────
        var updatesEnabled = new CheckBox
        {
            Content = T("Check for updates at startup", "Controlla gli aggiornamenti all’avvio"),
            IsChecked = current.UpdateCheckEnabled,
            Foreground = Palette.TextBrush,
        };
        var updatesNote = new TextBlock
        {
            Text = T("Sprocket tells you when a newer build is published; nothing downloads or installs without your say-so. Portable builds are pointed at the releases page instead.", "Sprocket ti avvisa quando è disponibile una nuova versione; non scarica né installa nulla senza il tuo consenso. Le versioni portatili rimandano alla pagina delle release."),
            Foreground = Palette.MutedTextBrush,
            FontSize = Typography.Caption,
            TextWrapping = TextWrapping.Wrap,
        };

        // ── AI control (MCP) ────────────────────────────────────────────────────────────────────
        string token = current.McpToken;
        var mcpEnabled = new CheckBox
        {
            Content = T("Enable MCP server (loopback only)", "Attiva il server MCP (solo connessioni locali)"),
            IsChecked = current.McpEnabled,
            Foreground = Palette.TextBrush,
        };
        TextBox port = MakeText(current.McpPort.ToString());
        port.Width = 80;
        port.HorizontalAlignment = HorizontalAlignment.Left;
        var requireToken = new CheckBox
        {
            Content = T("Require bearer token", "Richiedi token di autenticazione"),
            IsChecked = current.McpRequireToken,
            Foreground = Palette.TextBrush,
        };
        var tokenText = new TextBox
        {
            Text = token,
            IsReadOnly = true,
            FontFamily = Typography.Mono,
            FontSize = Typography.Caption,
            Foreground = Palette.MutedTextBrush,
            Background = Palette.PanelBgBrush,
        };
        Button copySetup = SmallButton(T("Copy setup command", "Copia comando di configurazione"));
        var mcpWarning = new TextBlock
        {
            Text = T("While enabled, a local AI client can inspect and edit the open project.", "Quando è attivo, un client AI locale può esaminare e modificare il progetto aperto."),
            Foreground = Palette.MutedTextBrush,
            FontSize = Typography.Caption,
            TextWrapping = TextWrapping.Wrap,
        };

        void RefreshMcpControls()
        {
            bool on = mcpEnabled.IsChecked == true;
            bool tokenOn = requireToken.IsChecked == true;
            port.IsEnabled = on;
            requireToken.IsEnabled = on;
            tokenText.IsVisible = on && tokenOn;
            copySetup.IsEnabled = on;
        }

        requireToken.IsCheckedChanged += (_, _) =>
        {
            token = PreferencesFormat.ResolveToken(requireToken.IsChecked == true, token, UserSettingsStore.NewToken);
            tokenText.Text = token;
            RefreshMcpControls();
        };
        mcpEnabled.IsCheckedChanged += (_, _) => RefreshMcpControls();
        RefreshMcpControls();

        // ── Buttons ─────────────────────────────────────────────────────────────────────────────
        var ok = new Button
        {
            Content = "OK",
            Padding = new Thickness(18, 5),
            Foreground = Brushes.White,
            Background = Palette.AccentBrush,
            CornerRadius = new CornerRadius(5),
        };
        var cancel = new Button
        {
            Content = T("Cancel", "Annulla"),
            Padding = new Thickness(16, 5),
            Foreground = Palette.TextBrush,
            Background = Palette.PanelBgBrush,
            CornerRadius = new CornerRadius(5),
        };

        var dialog = new Window
        {
            Title = T("Preferences", "Preferenze"),
            Icon = AppIcon.Window,
            Width = 474,
            Height = 640,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Background = Palette.WindowBgBrush,
            Content = new DockPanel
            {
                Margin = new Thickness(22),
                Children =
                {
                    new StackPanel
                    {
                        [DockPanel.DockProperty] = Dock.Bottom,
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Spacing = 8,
                        Margin = new Thickness(0, 16, 0, 0),
                        Children = { cancel, ok },
                    },
                    new ScrollViewer
                    {
                        // Keep content clear of the overlay scrollbar (§ STYLE_GUIDE surface rules).
                        Padding = new Thickness(0, 0, 14, 0),
                        Content = new StackPanel
                        {
                            Spacing = 8,
                            Children =
                            {
                                Section(T("Language", "Lingua")),
                                Labeled(T("Interface language", "Lingua dell’interfaccia"), language),

                                Section(T("Caches", "Cache")),
                                CacheRow(T("Proxy cache", "Cache proxy"), proxySizeText, clearProxy),
                                CacheRow(T("Render cache (this project)", "Cache render (questo progetto)"), renderSizeText, clearRender),
                                CacheRow(T("Stabilization analysis cache", "Cache analisi stabilizzazione"), analysisSizeText, clearAnalysis),

                                Section(T("Export metadata defaults", "Metadati predefiniti per l’esportazione")),
                                Labeled(T("Title", "Titolo"), title),
                                Labeled(T("Author", "Autore"), author),
                                Labeled(T("Copyright", "Copyright"), copyright),
                                Labeled(T("Comment", "Commento"), comment),
                                Hint(T("Tokens {project}, {username}, {year} and {date} are replaced when you export. Leave Comment blank and every export still carries “Created with Sprocket” plus the Sprocket version.", "I segnaposto {project}, {username}, {year} e {date} vengono sostituiti durante l’esportazione. Se lasci vuoto il commento, ogni esportazione includerà comunque “Creato con Sprocket” e la versione del programma.")),

                                Section(T("Autosave", "Salvataggio automatico")),
                                Labeled(T($"Interval (seconds, {UserSettingsStore.MinAutosaveSeconds}–{UserSettingsStore.MaxAutosaveSeconds})", $"Intervallo (secondi, {UserSettingsStore.MinAutosaveSeconds}–{UserSettingsStore.MaxAutosaveSeconds})"), autosave),

                                Section(T("Import", "Importazione")),
                                Labeled(T("Still image default duration (seconds)", "Durata predefinita immagini fisse (secondi)"), stillDuration),

                                Section(T("Audio", "Audio")),
                                Labeled(T("Output device", "Dispositivo di uscita"), audioDevice),
                                Hint(T("Choose where playback is monitored. A change applies immediately without interrupting playback; if the chosen device is later unavailable, Sprocket uses the system default.", "Scegli il dispositivo per ascoltare la riproduzione. La modifica si applica subito senza interrompere la riproduzione; se il dispositivo non è più disponibile, Sprocket usa quello predefinito del sistema.")),

                                Section(T("Updates", "Aggiornamenti")),
                                updatesEnabled,
                                updatesNote,

                                Section(T("AI control (MCP)", "Controllo AI (MCP)")),
                                mcpEnabled,
                                Labeled(T("Port", "Porta"), port),
                                requireToken,
                                tokenText,
                                copySetup,
                                mcpWarning,
                            },
                        },
                    },
                },
            },
        };

        clearProxy.Click += async (_, _) =>
        {
            long size = proxyCacheSize();
            bool confirmed = await ConfirmDialog.Show(dialog, T("Clear Proxy Cache", "Svuota cache proxy"),
                $T($"Eliminare tutti i file proxy memorizzati?\n\nSpazio attualmente utilizzato: {PreferencesFormat.Bytes(size)}. I proxy verranno rigenerati quando necessario; i progetti aperti potrebbero avviare nuovamente la creazione dei proxy.", $"Delete all cached proxy files?\n\nCurrently using {PreferencesFormat.Bytes(size)}. Proxies regenerate on demand; open projects may re-queue proxy builds."), "Clear", "Cancel");
            if (!confirmed)
                return;
            clearProxyCache();
            proxySizeText.Text = PreferencesFormat.Bytes(proxyCacheSize());
        };

        clearAnalysis.Click += async (_, _) =>
        {
            long size = analysisCacheSize();
            bool confirmed = await ConfirmDialog.Show(dialog, T("Clear Analysis Cache", "Svuota cache analisi"),
                $"Delete all cached stabilization motion tracks?\n\nCurrently using {PreferencesFormat.Bytes(size)}. " +
                "Stabilized clips re-analyze on demand; open projects may re-queue analyses.", "Clear", "Cancel");
            if (!confirmed)
                return;
            clearAnalysisCache();
            analysisSizeText.Text = PreferencesFormat.Bytes(analysisCacheSize());
        };

        clearRender.Click += async (_, _) =>
        {
            long size = renderCacheSize();
            bool confirmed = await ConfirmDialog.Show(dialog, T("Delete Render Files", "Elimina file di rendering"),
                $"Delete this project's preview render files?\n\nCurrently using {PreferencesFormat.Bytes(size)}. " +
                "Rendered previews rebuild when you render again.", "Delete", "Cancel");
            if (!confirmed)
                return;
            clearRenderCache();
            renderSizeText.Text = PreferencesFormat.Bytes(renderCacheSize());
        };

        copySetup.Click += async (_, _) =>
        {
            token = PreferencesFormat.ResolveToken(requireToken.IsChecked == true, token, UserSettingsStore.NewToken);
            tokenText.Text = token;
            string command = PreferencesFormat.McpSetupCommand(
                PreferencesFormat.ParsePort(port.Text) is { } p ? Math.Clamp(p, UserSettingsStore.MinPort, UserSettingsStore.MaxPort) : current.McpPort,
                requireToken.IsChecked == true, token);
            if (TopLevel.GetTopLevel(dialog)?.Clipboard is { } clipboard)
                await clipboard.SetValueAsync(DataFormat.Text, command);
        };

        ok.Click += (_, _) => dialog.Close(UserSettingsStore.Clamp(current with
        {
            ExportTitle = title.Text ?? "",
            ExportAuthor = author.Text ?? "",
            ExportCopyright = copyright.Text ?? "",
            ExportComment = comment.Text ?? "",
            AutosaveIntervalSeconds = PreferencesFormat.ParseSeconds(autosave.Text) ?? current.AutosaveIntervalSeconds,
            StillImageDefaultSeconds =
                double.TryParse(stillDuration.Text, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double s)
                    ? s
                    : current.StillImageDefaultSeconds,
            McpEnabled = mcpEnabled.IsChecked == true,
            McpPort = PreferencesFormat.ParsePort(port.Text) ?? current.McpPort,
            McpRequireToken = requireToken.IsChecked == true,
            McpToken = token,
            UpdateCheckEnabled = updatesEnabled.IsChecked == true,
            AudioOutputDevice = deviceSpecs[audioDevice.SelectedIndex >= 0 ? audioDevice.SelectedIndex : 0],
            Language = language.SelectedIndex == 1 ? AppLocalization.English : AppLocalization.Italian,
        }));
        cancel.Click += (_, _) => dialog.Close((UserSettings?)null);

        return dialog.ShowDialog<UserSettings?>(owner);
    }

    private static TextBlock Section(string text) => new()
    {
        Text = text,
        Foreground = Palette.TextBrush,
        FontWeight = FontWeight.SemiBold,
        FontSize = Typography.Emphasis,
        Margin = new Thickness(0, 10, 0, 2),
    };

    private static TextBlock Muted(string text) => new()
    {
        Text = text,
        Foreground = Palette.MutedTextBrush,
        FontSize = Typography.Body,
        VerticalAlignment = VerticalAlignment.Center,
    };

    /// <summary>A wrapping muted caption under a settings section (e.g. the metadata-token legend).</summary>
    private static TextBlock Hint(string text) => new()
    {
        Text = text,
        Foreground = Palette.MutedTextBrush,
        FontSize = Typography.Caption,
        TextWrapping = TextWrapping.Wrap,
        Margin = new Thickness(0, 2, 0, 0),
    };

    private static Button SmallButton(string label) => new()
    {
        Content = label,
        Padding = new Thickness(12, 3),
        FontSize = Typography.Body,
        Foreground = Palette.TextBrush,
        Background = Palette.PanelBgBrush,
        CornerRadius = new CornerRadius(5),
    };

    private static TextBox MakeText(string value) => new()
    {
        Text = value,
        Foreground = Palette.TextBrush,
        Background = Palette.PanelBgBrush,
        FontSize = Typography.Body,
    };

    private static StackPanel Labeled(string label, Control control) => new()
    {
        Spacing = 3,
        Children =
        {
            new TextBlock { Text = label, Foreground = Palette.MutedTextBrush, FontSize = Typography.Body },
            control,
        },
    };

    /// <summary>"Proxy cache   1.2 GB   [Clear]" — label left, size + action pinned right.</summary>
    private static Grid CacheRow(string label, TextBlock size, Button clear)
    {
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto,Auto") };
        var name = new TextBlock
        {
            Text = label,
            Foreground = Palette.TextBrush,
            FontSize = Typography.Body,
            VerticalAlignment = VerticalAlignment.Center,
        };
        size.Margin = new Thickness(0, 0, 10, 0);
        name.SetValue(Grid.ColumnProperty, 0);
        size.SetValue(Grid.ColumnProperty, 1);
        clear.SetValue(Grid.ColumnProperty, 2);
        grid.Children.Add(name);
        grid.Children.Add(size);
        grid.Children.Add(clear);
        return grid;
    }
}
