using Avalonia.Controls;

namespace Sprocket.App;

/// <summary>
/// Small, code-based UI localization layer. Keep technical format/codec names unchanged.
/// The language is stored per user in settings.json; Italian is the default for this fork.
/// </summary>
internal static class AppLocalization
{
    public const string Italian = "it";
    public const string English = "en";

    public static string Normalize(string? language) =>
        string.Equals(language, English, StringComparison.OrdinalIgnoreCase) ? English : Italian;

    public static void Apply(Window window, string? language)
    {
        if (Normalize(language) != Italian)
            return;

        window.Title = "Sprocket";
        Set(window, "FileMenu", "_File");
        Set(window, "EditMenu", "_Modifica");
        Set(window, "ClipMenu", "_Clip");
        Set(window, "EffectsMenu", "_Effetti");
        Set(window, "ViewMenu", "_Visualizza");
        Set(window, "WindowMenu", "_Finestra");
        Set(window, "HelpMenu", "_Aiuto");

        // File
        Set(window, "NewMenuItem", "_Nuovo progetto");
        Set(window, "OpenMenuItem", "_Apri progetto…");
        Set(window, "OpenRecentMenuItem", "Apri _recenti");
        Set(window, "OpenSampleMenuItem", "Apri progetto di _esempio");
        Set(window, "SaveMenuItem", "_Salva progetto");
        Set(window, "SaveAsMenuItem", "Salva progetto _con nome…");
        Set(window, "ImportMenuItem", "_Importa media…");
        Set(window, "RelinkMenuItem", "_Ricollega media…");
        Set(window, "ExportMenuItem", "_Esporta…");
        Set(window, "ExportQueueMenuItem", "_Coda di esportazione…");
        Set(window, "ExitMenuItem", "_Esci");

        // Edit
        Set(window, "UndoMenuItem", "_Annulla");
        Set(window, "RedoMenuItem", "_Ripeti");
        Set(window, "CutMenuItem", "_Taglia");
        Set(window, "CopyMenuItem", "_Copia");
        Set(window, "PasteMenuItem", "_Incolla");
        Set(window, "DeleteMenuItem", "_Elimina");
        Set(window, "RippleDeleteMenuItem", "_Elimina con chiusura");
        Set(window, "SelectAllMenuItem", "Seleziona _tutto");
        Set(window, "PreferencesMenuItem", "_Preferenze…");
        Set(window, "PluginsMenuItem", "_Plugin…");

        // Clip
        Set(window, "ClipInsertMenuItem", "_Inserisci");
        Set(window, "InsertEditMenuItem", "Inserisci _modifica  (,)");
        Set(window, "OverwriteEditMenuItem", "_Sovrascrivi modifica  (.)");
        Set(window, "FitToFillMenuItem", "Adatta al riempimento");
        Set(window, "ClipSplitAtPlayheadMenuItem", "_Dividi alla testina");
        Set(window, "ClipDuplicateMenuItem", "_Duplica");
        Set(window, "ClipEnableMenuItem", "_Abilita");
        Set(window, "ClipLinkMenuItem", "_Collega");
        Set(window, "ClipUnlinkMenuItem", "_Scollega");
        Set(window, "ClipSpeedMenuItem", "_Velocità / durata…");
        Set(window, "ClipInterpretFootageMenuItem", "_Interpreta metraggio…");
        Set(window, "ClipFrameHoldOptionsMenuItem", "Opzioni _fermo immagine…");
        Set(window, "ClipAddFrameHoldMenuItem", "_Aggiungi fermo immagine");
        Set(window, "ClipInsertFrameHoldSegmentMenuItem", "Inserisci _segmento fermo immagine");
        Set(window, "ClipDuplicateFrameMenuItem", "_Duplica fotogramma");
        Set(window, "ClipRemoveFrameMenuItem", "_Rimuovi fotogramma");
        Set(window, "ClipNormalizeMenuItem", "_Normalizza audio");
        Set(window, "CreateMulticamMenuItem", "Crea sorgente _multicamera");
        Set(window, "NudgeLeftMenuItem", "Sposta a sinistra");

        // Common visible panel labels
        SetText(window, "TimelineHeader", "Timeline");
    }

    private static void Set(Window window, string name, string value)
    {
        if (window.FindControl<MenuItem>(name) is { } item)
            item.Header = value;
    }

    private static void SetText(Window window, string name, string value)
    {
        if (window.FindControl<TextBlock>(name) is { } text)
            text.Text = value;
    }
}
