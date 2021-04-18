using ReactiveUI;

namespace dTerm.UI.Wpf.Views
{
    public abstract class MainHeaderBase : BaseUserControl<MainHeaderViewModel> { }

    public partial class MainHeader : MainHeaderBase
    {
        public MainHeader()
        {
            InitializeComponent();

            ViewModel = new MainHeaderViewModel();

            this.WhenActivated(bindings =>
            {
                DataContext = ViewModel;
            });
        }
    }
}
