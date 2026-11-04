using Avalonia.Controls;

namespace Sprocket.App;

/// <summary>Per-user interface language. Technical codec and format names stay unchanged.</summary>
internal static class AppLocalization
{
    public const string Italian = "it";
    public const string English = "en";

    public static string Normalize(string? language) =>
        string.Equals(language, English, StringComparison.OrdinalIgnoreCase) ? English : Italian;

    public static void Apply(Window window, string? language)
    {
        if (Normalize(language) == English) { ApplyEnglish(window); return; }
        window.Title = "Sprocket";
        Menu(window, "FileMenu", "_File"); Menu(window, "EditMenu", "_Modifica");
        Menu(window, "ClipMenu", "_Clip"); Menu(window, "SequenceMenu", "_Sequenza");
        Menu(window, "EffectsMenu", "_Effetti"); Menu(window, "ViewMenu", "_Visualizza");
        Menu(window, "WindowMenu", "_Finestra"); Menu(window, "HelpMenu", "_Aiuto");

        Menu(window, "NewMenuItem", "_Nuovo progetto"); Menu(window, "OpenMenuItem", "_Apri progetto…");
        Menu(window, "OpenRecentMenuItem", "Apri _recenti"); Menu(window, "OpenSampleMenuItem", "Apri progetto di _esempio");
        Menu(window, "SaveMenuItem", "_Salva progetto"); Menu(window, "SaveAsMenuItem", "Salva progetto con _nome…");
        Menu(window, "ImportMenuItem", "_Importa media…"); Menu(window, "RelinkMenuItem", "_Ricollega media…");
        Menu(window, "ExportMenuItem", "_Esporta…"); Menu(window, "ExportQueueMenuItem", "Coda di _esportazione…");
        Menu(window, "ExportEdlMenuItem", "EDL (CMX3600)…"); Menu(window, "ExportFcpXmlMenuItem", "Final Cut XML…");
        Menu(window, "ExitMenuItem", "_Esci");

        Menu(window, "UndoMenuItem", "_Annulla"); Menu(window, "RedoMenuItem", "_Ripeti");
        Menu(window, "CutMenuItem", "_Taglia"); Menu(window, "CopyMenuItem", "_Copia");
        Menu(window, "PasteMenuItem", "_Incolla"); Menu(window, "DeleteMenuItem", "_Elimina");
        Menu(window, "RippleDeleteMenuItem", "_Elimina e chiudi lo spazio");
        Menu(window, "SelectAllMenuItem", "Seleziona _tutto");
        Menu(window, "PreferencesMenuItem", "_Preferenze…"); Menu(window, "PluginsMenuItem", "_Plugin…");

        Menu(window, "ClipInsertMenuItem", "_Inserisci"); Menu(window, "InsertEditMenuItem", "Inserisci modifica  (,)");
        Menu(window, "OverwriteEditMenuItem", "Modifica con sovrascrittura  (.)");
        Menu(window, "FitToFillMenuItem", "Adatta al riempimento");
        Menu(window, "ClipSplitAtPlayheadMenuItem", "_Dividi alla testina");
        Menu(window, "ClipDuplicateMenuItem", "_Duplica"); Menu(window, "ClipEnableMenuItem", "_Abilita");
        Menu(window, "ClipLinkMenuItem", "_Collega"); Menu(window, "ClipUnlinkMenuItem", "_Scollega");
        Menu(window, "ClipSpeedMenuItem", "_Velocità / durata…");
        Menu(window, "ClipInterpretFootageMenuItem", "_Interpreta metraggio…");
        Menu(window, "ClipFrameHoldOptionsMenuItem", "Opzioni _fermo immagine…");
        Menu(window, "ClipAddFrameHoldMenuItem", "_Aggiungi fermo immagine");
        Menu(window, "ClipInsertFrameHoldSegmentMenuItem", "Inserisci _segmento fermo immagine");
        Menu(window, "ClipDuplicateFrameMenuItem", "_Duplica fotogramma");
        Menu(window, "ClipRemoveFrameMenuItem", "_Rimuovi fotogramma");
        Menu(window, "ClipNormalizeMenuItem", "_Normalizza audio");
        Menu(window, "CreateMulticamMenuItem", "Crea sorgente _multicamera");
        Menu(window, "NudgeLeftMenuItem", "Sposta a _sinistra"); Menu(window, "NudgeRightMenuItem", "Sposta a _destra");

        Menu(window, "NewSequenceMenuItem", "_Nuova sequenza"); Menu(window, "OpenSequenceMenuItem", "_Apri sequenza");
        Menu(window, "NestMenuItem", "_Annida"); Menu(window, "PlayInOutMenuItem", "Riproduci da In a Out");
        Menu(window, "MarksMenuItem", "_Marcatori In / Out"); Menu(window, "MarkInMenuItem", "Imposta punto _In");
        Menu(window, "MarkOutMenuItem", "Imposta punto _Out"); Menu(window, "MarkClipMenuItem", "Marca _clip");
        Menu(window, "MarkSelectionMenuItem", "Marca _selezione  (/)");
        Menu(window, "GoToInMenuItem", "Vai al punto _In"); Menu(window, "GoToOutMenuItem", "Vai al punto _Out");
        Menu(window, "ClearInMenuItem", "Cancella punto I_n"); Menu(window, "ClearOutMenuItem", "Cancella punto O_ut");
        Menu(window, "ClearInOutMenuItem", "Cancella In e _Out");
        Menu(window, "LiftMenuItem", "_Rimuovi lasciando lo spazio  (;)"); Menu(window, "ExtractMenuItem", "_Estrai  (')");
        Menu(window, "RenderInOutMenuItem", "_Renderizza da In a Out");
        Menu(window, "RenderSelectionMenuItem", "Renderizza _selezione"); Menu(window, "RenderAudioMenuItem", "Renderizza _audio");
        Menu(window, "FreezeClipAudioMenuItem", "_Congela audio clip"); Menu(window, "UnfreezeClipAudioMenuItem", "_Scongela audio clip");
        Menu(window, "DeleteRenderFilesMenuItem", "_Elimina file render…");
        Menu(window, "SequenceSettingsMenuItem", "Impo_stazioni sequenza…");

        Menu(window, "ZoomInMenuItem", "Aumenta _zoom"); Menu(window, "ZoomOutMenuItem", "Riduci _zoom");
        Menu(window, "ZoomToFitMenuItem", "Adatta allo _schermo"); Menu(window, "SnappingMenuItem", "_Aggancio");
        Menu(window, "GuidesMenuItem", "_Guide"); Menu(window, "AutoScrollMenuItem", "Scorrimento _automatico");
        Menu(window, "AutoScrollNoneMenuItem", "_Nessuno"); Menu(window, "AutoScrollPageMenuItem", "Scorrimento a _pagina");
        Menu(window, "AutoScrollSmoothMenuItem", "Scorrimento _fluido");
        Menu(window, "ShowProjectMenuItem", "Pannello _progetto"); Menu(window, "ShowInspectorMenuItem", "Pannello _ispettore");
        Menu(window, "ShowStatsMenuItem", "Statistiche di _riproduzione");
        Menu(window, "ShowProxyStatusMenuItem", "_Attività in background");
        Menu(window, "FullScreenMenuItem", "Schermo _intero");
        Menu(window, "FullScreenPreviewMenuItem", "Anteprima a schermo _intero");
        Menu(window, "ResetLayoutMenuItem", "_Ripristina disposizione");

        Menu(window, "CheckUpdatesMenuItem", "Controlla _aggiornamenti…");
        Menu(window, "AddToApplicationsMenuMenuItem", "Aggiungi al menu _Applicazioni");
        Menu(window, "RemoveFromApplicationsMenuMenuItem", "Rimuovi dal menu Applicazioni");
        Menu(window, "DocsMenuItem", "_Documentazione"); Menu(window, "WebsiteMenuItem", "Sito _web di Sprocket");
        Menu(window, "ReportIssueMenuItem", "_Segnala un problema…"); Menu(window, "SupportMenuItem", "_Sostieni Sprocket ♥");
        Menu(window, "ThirdPartyNoticesMenuItem", "Avvisi sulle librerie di _terze parti");
        Menu(window, "AboutMenuItem", "_Informazioni su Sprocket");

        Content(window, "SelectTool", "Selezione"); Content(window, "BladeTool", "Taglio");
        Content(window, "RippleTool", "Montaggio con chiusura"); Content(window, "RollTool", "Scorrimento montaggio");
        Content(window, "SlipTool", "Scorrimento clip"); Content(window, "SlideTool", "Spostamento clip");
        Content(window, "HandTool", "Mano"); Content(window, "ZoomTool", "Zoom");
        Content(window, "SnappingToggle", "Aggancio"); Content(window, "LinkedToggle", "Collegate");
        Content(window, "ExportButton", "Esporta");
        Text(window, "ProjectTitleText", "Senza titolo"); Text(window, "SaveStateText", "• tutte le modifiche salvate");
        Text(window, "StatusText", "Pronto");
    }

    private static void ApplyEnglish(Window window)
    {
        window.Title = "Sprocket";
        Menu(window, "FileMenu", "_File"); Menu(window, "EditMenu", "_Edit"); Menu(window, "ClipMenu", "_Clip");
        Menu(window, "SequenceMenu", "_Sequence"); Menu(window, "EffectsMenu", "Effe_cts");
        Menu(window, "ViewMenu", "_View"); Menu(window, "WindowMenu", "_Window"); Menu(window, "HelpMenu", "_Help");
        Menu(window, "NewMenuItem", "_New Project"); Menu(window, "OpenMenuItem", "_Open Project…");
        Menu(window, "OpenRecentMenuItem", "Open _Recent"); Menu(window, "OpenSampleMenuItem", "Open Sa_mple Project");
        Menu(window, "SaveMenuItem", "_Save Project"); Menu(window, "SaveAsMenuItem", "Save Project _As…");
        Menu(window, "ImportMenuItem", "_Import Media…"); Menu(window, "RelinkMenuItem", "_Relink Media…");
        Menu(window, "ExportMenuItem", "_Export…"); Menu(window, "ExportQueueMenuItem", "Export _Queue…");
        Menu(window, "ExitMenuItem", "E_xit");
        Menu(window, "ExportEdlMenuItem", "_EDL (CMX3600)…");
        Menu(window, "ExportFcpXmlMenuItem", "_Final Cut XML…");
        Menu(window, "ClipInsertMenuItem", "_Insert");
        Menu(window, "InsertEditMenuItem", "Insert Ed_it  (,)");
        Menu(window, "OverwriteEditMenuItem", "_Overwrite Edit  (.)");
        Menu(window, "FitToFillMenuItem", "Fit to Fill");
        Menu(window, "MarksMenuItem", "_In / Out"); Menu(window, "UndoMenuItem", "_Undo"); Menu(window, "RedoMenuItem", "_Redo");
        Menu(window, "CutMenuItem", "Cu_t"); Menu(window, "CopyMenuItem", "_Copy"); Menu(window, "PasteMenuItem", "_Paste");
        Menu(window, "DeleteMenuItem", "_Delete"); Menu(window, "RippleDeleteMenuItem", "_Ripple Delete");
        Menu(window, "SelectAllMenuItem", "Select _All"); Menu(window, "PreferencesMenuItem", "Pre_ferences…");
        Menu(window, "PluginsMenuItem", "_Plugins…"); Menu(window, "ClipSplitAtPlayheadMenuItem", "Spli_t at Playhead");
        Menu(window, "ClipDuplicateMenuItem", "Du_plicate"); Menu(window, "ClipEnableMenuItem", "_Enable");
        Menu(window, "ClipLinkMenuItem", "_Link"); Menu(window, "ClipUnlinkMenuItem", "_Unlink");
        Menu(window, "ClipSpeedMenuItem", "_Speed / Duration…"); Menu(window, "ClipInterpretFootageMenuItem", "Interpret _Footage…");
        Menu(window, "ClipFrameHoldOptionsMenuItem", "Frame _Hold Options…");
        Menu(window, "ClipAddFrameHoldMenuItem", "_Add Frame Hold");
        Menu(window, "ClipInsertFrameHoldSegmentMenuItem", "Insert Frame Hold Se_gment");
        Menu(window, "ClipDuplicateFrameMenuItem", "_Duplicate Frame"); Menu(window, "ClipRemoveFrameMenuItem", "Remo_ve Frame");
        Menu(window, "ClipNormalizeMenuItem", "_Normalize Audio"); Menu(window, "CreateMulticamMenuItem", "Create _Multicam Source");
        Menu(window, "NudgeLeftMenuItem", "Nudge _Left"); Menu(window, "NudgeRightMenuItem", "Nudge _Right");
        Menu(window, "NewSequenceMenuItem", "_New Sequence"); Menu(window, "OpenSequenceMenuItem", "_Open Sequence");
        Menu(window, "NestMenuItem", "_Nest"); Menu(window, "PlayInOutMenuItem", "_Play In to Out");
        Menu(window, "MarkInMenuItem", "Mark _In"); Menu(window, "MarkOutMenuItem", "Mark _Out");
        Menu(window, "MarkClipMenuItem", "Mark _Clip"); Menu(window, "MarkSelectionMenuItem", "Mark _Selection  (/)");
        Menu(window, "GoToInMenuItem", "_Go to In"); Menu(window, "GoToOutMenuItem", "Go to O_ut");
        Menu(window, "ClearInMenuItem", "Clear I_n"); Menu(window, "ClearOutMenuItem", "Clear Ou_t");
        Menu(window, "ClearInOutMenuItem", "Clear In _and Out"); Menu(window, "LiftMenuItem", "Lif_t  (;)");
        Menu(window, "ExtractMenuItem", "_Extract  (')"); Menu(window, "RenderInOutMenuItem", "_Render In to Out");
        Menu(window, "RenderSelectionMenuItem", "Render Se_lection"); Menu(window, "RenderAudioMenuItem", "Render _Audio");
        Menu(window, "FreezeClipAudioMenuItem", "Free_ze Clip Audio"); Menu(window, "UnfreezeClipAudioMenuItem", "Un_freeze Clip Audio");
        Menu(window, "DeleteRenderFilesMenuItem", "_Delete Render Files…"); Menu(window, "SequenceSettingsMenuItem", "Sequence _Settings…");
        Menu(window, "ZoomInMenuItem", "Zoom _In"); Menu(window, "ZoomOutMenuItem", "Zoom _Out");
        Menu(window, "ZoomToFitMenuItem", "Zoom to _Fit"); Menu(window, "SnappingMenuItem", "_Snapping");
        Menu(window, "GuidesMenuItem", "_Guides"); Menu(window, "AutoScrollMenuItem", "Playback _Auto-Scroll");
        Menu(window, "AutoScrollNoneMenuItem", "_No Scroll"); Menu(window, "AutoScrollPageMenuItem", "_Page Scroll");
        Menu(window, "AutoScrollSmoothMenuItem", "_Smooth Scroll"); Menu(window, "ShowProjectMenuItem", "_Project Panel");
        Menu(window, "ShowInspectorMenuItem", "I_nspector Panel"); Menu(window, "ShowStatsMenuItem", "Playback _Statistics");
        Menu(window, "ShowProxyStatusMenuItem", "_Background Tasks"); Menu(window, "FullScreenMenuItem", "_Full Screen");
        Menu(window, "FullScreenPreviewMenuItem", "Full Screen Previe_w"); Menu(window, "ResetLayoutMenuItem", "_Reset Layout");
        Menu(window, "CheckUpdatesMenuItem", "Check for _Updates…");
        Menu(window, "AddToApplicationsMenuMenuItem", "Add to _Applications Menu");
        Menu(window, "RemoveFromApplicationsMenuMenuItem", "Remove from Applications _Menu");
        Menu(window, "DocsMenuItem", "_Documentation"); Menu(window, "WebsiteMenuItem", "Sprocket _Website");
        Menu(window, "ReportIssueMenuItem", "_Report an Issue…"); Menu(window, "SupportMenuItem", "_Support Sprocket ♥");
        Menu(window, "ThirdPartyNoticesMenuItem", "_Third-Party Notices"); Menu(window, "AboutMenuItem", "_About Sprocket");
        Content(window, "SelectTool", "Select"); Content(window, "BladeTool", "Blade");
        Content(window, "RippleTool", "Ripple"); Content(window, "RollTool", "Roll");
        Content(window, "SlipTool", "Slip"); Content(window, "SlideTool", "Slide");
        Content(window, "HandTool", "Hand"); Content(window, "ZoomTool", "Zoom");
        Content(window, "SnappingToggle", "Snapping"); Content(window, "LinkedToggle", "Linked");
        Content(window, "ExportButton", "Export");
        Text(window, "ProjectTitleText", "Untitled"); Text(window, "SaveStateText", "• all changes saved");
        Text(window, "StatusText", "Ready");
    }

    private static void Menu(Window window, string name, string value)
    {
        if (window.FindControl<MenuItem>(name) is { } item) item.Header = value;
    }
    private static void Content(Window window, string name, string value)
    {
        if (window.FindControl<ContentControl>(name) is { } control) control.Content = value;
    }
    private static void Text(Window window, string name, string value)
    {
        if (window.FindControl<TextBlock>(name) is { } control) control.Text = value;
    }
}