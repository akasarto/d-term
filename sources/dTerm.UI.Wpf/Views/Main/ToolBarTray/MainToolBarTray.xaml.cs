namespace dTerm.UI.Wpf.Views
{
    public abstract class MainToolBarTrayBase : BaseUserControl<MainToolBarTrayViewModel> { }

    public partial class MainToolBarTray : MainToolBarTrayBase
    {
        public MainToolBarTray()
        {
            InitializeComponent();
        }
    }
}
