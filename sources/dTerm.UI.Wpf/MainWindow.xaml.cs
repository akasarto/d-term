using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newTerminal = new TerminalWindow()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ShowInTaskbar = false,
                Owner = this
            };

            newTerminal.Show();
        }

        /*
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _terminalWpf = new TerminalWpf();

            _terminalWpf.InstanceReady += () =>
            {
                Task.Factory.StartNew(() => CopyConsoleToWindow(), TaskCreationOptions.LongRunning);
                Dispatcher.Invoke(() => { TitleBarTitle.Text = "Console"; });
            };

            Task.Run(() => _terminalWpf.Start("cmd.exe"));
        }

        private void CopyConsoleToWindow()
        {
            using (StreamReader reader = new StreamReader(_terminalWpf.OutputStream))
            {
                int bytesRead;
                char[] buf = new char[1];
                while ((bytesRead = reader.ReadBlock(buf, 0, 1)) != 0)
                {
                    // This is where you'd parse and tokenize the incoming VT100 text, most likely.
                    Dispatcher.Invoke(() =>
                    {
                        // ...and then you'd do something to render it.
                        // For now, just emit raw VT100 to the primary TextBlock.
                        TerminalHistoryBlock.Text += new string(buf.Take(bytesRead).ToArray());
                    });
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.Key == Key.Enter)
                {
                    _terminalWpf.Write('\r');
                    _terminalWpf.Write('\n');
                }
                else
                {
                    _terminalWpf.Write(GetCharFromKey(e.Key));
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {
                _autoScroll = TerminalHistoryViewer.VerticalOffset == TerminalHistoryViewer.ScrollableHeight;

                if (_autoScroll && e.ExtentHeightChange != 0)
                {
                    TerminalHistoryViewer.ScrollToEnd();
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }

        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                MaximizeRestoreButton.Content = "\uE923";
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaximizeRestoreButton.Content = "\uE922";
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        */
    }
}
