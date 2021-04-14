using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;

namespace dTerm.UI.Wpf.Views
{
    public class IconEditorViewModel : ReactiveObject
    {
        public IconEditorViewModel(PackIconKind kind, List<string> aliases)
        {
            Kind = kind;
            Aliases = aliases;
        }

        [Reactive] public PackIconKind Kind { get; set; }
        [Reactive] public List<string> Aliases { get; set; }
    }
}
