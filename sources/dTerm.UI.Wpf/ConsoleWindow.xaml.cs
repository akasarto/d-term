using dTerm.Infra.ConPTY;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static dTerm.Core.WinApi;

namespace dTerm.UI.Wpf
{
    public partial class ConsoleWindow : Window
    {
        private TerminalWpf _terminalWpf;
        private bool _autoScroll = true;

        public ConsoleWindow()
        {
            InitializeComponent();
        }

        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }

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
                // This is where you'd take the pressed key, and convert it to a 
                // VT100 code before sending it along. For now, we'll just send _something_.

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
    }
}
