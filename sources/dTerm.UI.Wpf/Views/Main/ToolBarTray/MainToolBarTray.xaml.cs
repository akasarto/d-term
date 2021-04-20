using ReactiveUI;

namespace dTerm.UI.Wpf.Views
{
    public abstract class MainToolBarTrayBase : BaseUserControl<MainToolBarTrayViewModel> { }

    public partial class MainToolBarTray : MainToolBarTrayBase
    {
        public MainToolBarTray()
        {
            InitializeComponent();

            ViewModel ??= new MainToolBarTrayViewModel();

            this.WhenActivated(bindings =>
            {
                DataContext ??= ViewModel;
            });
        }
    }
}
