using dTerm.Infra.ConPTY;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Threading;

namespace dTerm.UI.Wpf.Views
{
    public abstract class TerminalWindowBase : BaseWindow<TerminalWindowViewModel> { }

    public partial class TerminalWindow : TerminalWindowBase
    {
        private TerminalConnection _terminalConnection;

        public TerminalWindow()
        {
            InitializeComponent();

            this.WhenActivated(bindings =>
            {
                
            });

            this.Events().Loaded.Subscribe(_ =>
            {
                _terminalConnection = new TerminalConnection(ViewModel.ProcessExecutablePath);

                _terminalConnection.ProcessStarted += ProcessStarted;
                _terminalConnection.ProcessExited += ProcessExited;
            });

            this.Events().SizeChanged.Subscribe(_ =>
            {
                Terminal.TriggerResize(new Size(Width, Height));
            });

            this.Events().Closing.Subscribe(e =>
            {
                if (_terminalConnection.IsConnected)
                {
                    e.Cancel = true;

                    _terminalConnection.Close();
                }
            });

            Terminal.Events().Loaded.Subscribe(_ =>
            {
                Terminal.Connection = _terminalConnection;
                Terminal.TriggerResize(new Size(Width, Height));
                Terminal.Focus();
            });
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
    }
}
