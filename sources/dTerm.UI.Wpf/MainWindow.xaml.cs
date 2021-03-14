using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Window window in OwnedWindows)
            {
                window.Close();
            }
        }

        private void CmdClick(object sender, RoutedEventArgs e)
        {
            var newTerminal = new TerminalWindow("cmd.exe")
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ShowInTaskbar = false,
                Owner = this
            };

            newTerminal.Show();
        }

        private void GitClick(object sender, RoutedEventArgs e)
        {
            var newTerminal = new TerminalWindow("sh.exe")
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ShowInTaskbar = false,
                Owner = this
            };

            newTerminal.Show();
        }

        private void PShellClick(object sender, RoutedEventArgs e)
        {
            var newTerminal = new TerminalWindow("powershell.exe")
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ShowInTaskbar = false,
                Owner = this
            };

            newTerminal.Show();
        }
    }
}
