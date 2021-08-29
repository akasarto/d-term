using MaterialDesignThemes.Wpf;

namespace dTerm.UI.Wpf.Views
{
    public class IconBrowserItemViewModel : BaseViewModel
    {
        public PackIconKind Kind { get; init; }
        public string Aliases { get; init; }
    }
}
