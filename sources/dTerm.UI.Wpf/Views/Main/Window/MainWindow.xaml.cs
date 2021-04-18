using ReactiveUI;

namespace dTerm.UI.Wpf.Views
{
    public abstract class MainWindowBase : BaseWindow<MainWindowViewModel> { }

    public partial class MainWindow : MainWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();

            this.WhenActivated(bindings =>
            {
                DataContext = ViewModel;
            });
        }
    }
}
