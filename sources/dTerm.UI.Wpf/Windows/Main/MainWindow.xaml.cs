using dTerm.Infra.EfCore;
using dTerm.UI.Wpf.Windows;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace dTerm.UI.Wpf.Windows
{
    public partial class MainWindow
    {
        private readonly AppDbContextFactory _appDbContextFactory;

        public MainWindow(AppDbContextFactory appDbContextFactory)
        {
            InitializeComponent();

            Loaded += WindowLoaded;
            Closing += WindowClosing;

            _appDbContextFactory = appDbContextFactory;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _ = Task.Run(() => LoadConsoles());
        }

        private void LoadConsoles()
        {
            using (var context = _appDbContextFactory.Create())
            {
                var consoles = context.Consoles.ToList();

                Dispatcher.Invoke(() =>
                {
                    Consoles.ItemsSource = consoles;
                });
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Window window in OwnedWindows)
            {
                window.Close();
            }
        }

        private void ButtonSettingsClick(object sender, RoutedEventArgs e)
        {
            var wnd = new SettingsWindow()
            {
                Owner = this,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                ShowActivated = false
            };

            _ = wnd.ShowDialog();
        }
    }
}
