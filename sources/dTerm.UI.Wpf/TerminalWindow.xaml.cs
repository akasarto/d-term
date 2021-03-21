using System.Windows;

namespace dTerm.UI.Wpf
{
    public partial class TerminalWindow : Window
    {
        private readonly TerminalConnection _terminalConnection;

        public TerminalWindow(string processName)
        {
            InitializeComponent();

            _terminalConnection = new TerminalConnection(processName);

            _terminalConnection.ProcessStarted += ProcessStarted;
            _terminalConnection.ProcessExited += ProcessExited;

            Terminal.Loaded += TerminalLoaded;
            SizeChanged += WindowSizeChanged;
            Closing += WindowClosing;
        }

        private void ProcessStarted(int processId)
        {
            Dispatcher.Invoke(() => Title = $"PID {processId}");
        }

        private void ProcessExited(int processId)
        {
            if (!Dispatcher.HasShutdownStarted)
            {
                Dispatcher.Invoke(() => Close());
            }
        }

        private void TerminalLoaded(object sender, RoutedEventArgs e)
        {
            Terminal.Connection = _terminalConnection;
            Terminal.TriggerResize(new Size(Width, Height));
            Terminal.Focus();
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Terminal.TriggerResize(new Size(Width, Height));
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_terminalConnection.IsConnected)
            {
                e.Cancel = true;

                _terminalConnection.Close();
            }
        }
    }
}
