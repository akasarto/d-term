using ReactiveUI;

namespace dTerm.UI.Wpf.Views
{
    public abstract class MainHeaderBase : BaseUserControl<MainHeaderViewModel> { }

    public partial class MainHeader : MainHeaderBase
    {
        public MainHeader()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                ViewModel ??= new MainHeaderViewModel();
            });
        }
    }
}
