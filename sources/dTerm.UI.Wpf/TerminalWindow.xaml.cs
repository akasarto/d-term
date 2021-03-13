using Microsoft.Terminal.Wpf;
using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class TerminalWindow : Window
    {
        private readonly TerminalConnection _terminalConnection;

        public TerminalWindow()
        {
            InitializeComponent();

            _terminalConnection = new TerminalConnection("cmd.exe");

            _terminalConnection.ProcessStarted += _terminalConnection_ProcessStarted;
            _terminalConnection.ProcessExited += _terminalConnection_ProcessExited;

            Terminal.Loaded += Terminal_Loaded;
            Closing += TerminalWindow_Closing;
        }

        private void _terminalConnection_ProcessStarted(int processId)
        {
            Dispatcher.Invoke(() => Title = $"PID {processId}");
        }

        private void _terminalConnection_ProcessExited(int processId)
        {
            Dispatcher.Invoke(() => Close());
        }

        private void TerminalWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_terminalConnection.IsConnected)
            {
                e.Cancel = true;

                _terminalConnection.Close();
            }
        }

        private void Terminal_Loaded(object sender, RoutedEventArgs e)
        {
            var theme = new TerminalTheme
            {
                ColorTable = new uint[]
                {
                    0x0C0C0C, 0x1F0FC5, 0x0EA113, 0x009CC1, 0xDA3700, 0x981788, 0xDD963A, 0xCCCCCC, 0x767676, 0x5648E7, 0x0CC616, 0xA5F1F9, 0xFF783B, 0x9E00B4, 0xD6D661, 0xF2F2F2
                },
                CursorStyle = CursorStyle.BlinkingBar,
                DefaultBackground = 0x0c0c0c,
                DefaultForeground = 0xcccccc
            };

            Terminal.Connection = _terminalConnection;
            Terminal.SetTheme(theme, "Cascadia Code", 12);
            Terminal.Focus();
        }
    }
}
