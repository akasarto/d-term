using dTerm.Infra.ConPTY;
using Microsoft.Terminal.Wpf;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Text;
using System.Threading;
using static dTerm.Core.WinApi;

namespace dTerm.UI.Wpf
{
    public class TerminalConnection : ITerminalConnection
    {
        private readonly Thread _processThread;
        private readonly Thread _readingThread;

        private volatile bool _processRunning = false;

        private string _command;
        private int _consoleWidth;
        private int _consoleHeight;
        private StreamWriter _consoleWriter;
        private StreamReader _consoleReader;
        private IntPtr _processHandle;
        private int _processId;

        public TerminalConnection(string command, int consoleWidth = 80, int consoleHeight = 30)
        {
            _command = command;

            _consoleWidth = consoleWidth;
            _consoleHeight = consoleHeight;

            _processThread = new Thread(ProcessExecution);
            _readingThread = new Thread(ReadConsoleOutput);
        }

        public delegate void ProcessStartedEventHandler(int processId);

        public event EventHandler<TerminalOutputEventArgs> TerminalOutput;
        public event ProcessStartedEventHandler ProcessStarted;
        public event ProcessStartedEventHandler ProcessExited;

        public bool IsConnected => _processRunning;

        public void Close() => TerminateProcess(_processHandle, 1);

        public void Start() => _processThread.Start();

        public void Resize(uint rows, uint columns)
        {
        }

        public void WriteInput(string data)
        {
            if (data.Length != 0)
            {
                _consoleWriter.Write(data);
            }
        }

        private void ProcessExecution()
        {
            using (var inputPipe = new PseudoConsolePipe())
            using (var outputPipe = new PseudoConsolePipe())
            using (_consoleReader = new StreamReader(new FileStream(outputPipe.ReadSide, FileAccess.Read), Encoding.UTF8))
            using (_consoleWriter = new StreamWriter(new FileStream(inputPipe.WriteSide, FileAccess.Write)) { AutoFlush = true })
            using (var pseudoConsole = PseudoConsole.Create(inputPipe.ReadSide, outputPipe.WriteSide, _consoleWidth, _consoleHeight))
            using (var process = ProcessFactory.Start(_command, PseudoConsole.PseudoConsoleThreadAttribute, pseudoConsole.Handle))
            {
                _processRunning = true;
                _processHandle = process.ProcessInfo.hProcess;
                _processId = process.ProcessInfo.dwProcessId;

                ProcessStarted?.Invoke(_processId);

                _readingThread.Start();

                SetConsoleCtrlHandler(eventType =>
                {
                    if (eventType is CtrlTypes.CTRL_CLOSE_EVENT or CtrlTypes.CTRL_SHUTDOWN_EVENT)
                    {
                        _processRunning = false;

                        DisposeResources(process, pseudoConsole, outputPipe, inputPipe, _consoleWriter, _consoleReader);
                    }

                    return false;

                }, true);

                WaitForExit(process).WaitOne(
                    Timeout.Infinite
                );

                _processRunning = false;

                ProcessExited?.Invoke(_processId);
            }
        }

        private void ReadConsoleOutput()
        {
            var buffer = new char[1024];

            while (_processRunning)
            {
                var bytesRead = _consoleReader.Read(buffer, 0, buffer.Length);

                if (bytesRead != -1)
                {
                    var args = new TerminalOutputEventArgs(
                        buffer.AsSpan(0, bytesRead).ToString()
                    );

                    TerminalOutput?.Invoke(this, args);
                }
            }
        }

        private AutoResetEvent WaitForExit(Process process) => new AutoResetEvent(false)
        {
            SafeWaitHandle = new SafeWaitHandle(process.ProcessInfo.hProcess, ownsHandle: true)
        };

        private void DisposeResources(params IDisposable[] disposables)
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
