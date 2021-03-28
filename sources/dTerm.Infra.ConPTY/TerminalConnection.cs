using Microsoft.Terminal.Wpf;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace dTerm.Infra.ConPTY
{
    public class TerminalConnection : ITerminalConnection
    {
        private readonly Thread _processThread;
        private readonly Thread _readingThread;

        private int _processId;
        private readonly string _processName;
        private IntPtr _processHandle;
        private IntPtr _pseudoConsoleHandle;
        private volatile bool _processRunning;
        private bool _processIsTerminating;
        private StreamWriter _consoleWriter;
        private StreamReader _consoleReader;
        private readonly int _consoleHeight;
        private readonly int _consoleWidth;

        public TerminalConnection(string processName, int consoleWidth = 80, int consoleHeight = 30)
        {
            _processName = processName;

            _consoleWidth = consoleWidth;
            _consoleHeight = consoleHeight;

            _processThread = new Thread(ProcessExecute);
            _readingThread = new Thread(ConsoleDataRead);
        }

        public delegate void ProcessStartedEventHandler(int processId);
        public delegate void ProcessExitedEventHandler(int processId);

        public event EventHandler<TerminalOutputEventArgs> TerminalOutput;
        public event ProcessStartedEventHandler ProcessStarted;
        public event ProcessExitedEventHandler ProcessExited;

        public bool IsConnected => !_processIsTerminating && _processRunning;

        public void Start() => _processThread.Start();

        public void Resize(uint rows, uint columns)
        {
            _ = WinNativeApi.ResizePseudoConsole(
                _pseudoConsoleHandle,
                new WinNativeApi.COORD()
                {
                    X = (short)columns,
                    Y = (short)rows
                }
            );
        }

        public void WriteInput(string data)
        {
            if (data.Length != 0)
            {
                _consoleWriter.Write(data);
            }
        }

        public void Close()
        {
            if (!_processIsTerminating && _processRunning)
            {
                _processIsTerminating = true;

                WinNativeApi.TerminateProcess(_processHandle, 1);
            }
        }

        private void ProcessExecute()
        {
            using (var inputPipe = new PseudoConsolePipe())
            using (var outputPipe = new PseudoConsolePipe())
            using (_consoleReader = new StreamReader(new FileStream(outputPipe.ReadSide, FileAccess.Read), Encoding.UTF8))
            using (_consoleWriter = new StreamWriter(new FileStream(inputPipe.WriteSide, FileAccess.Write)) { AutoFlush = true })
            using (var pseudoConsole = PseudoConsole.Create(inputPipe.ReadSide, outputPipe.WriteSide, _consoleWidth, _consoleHeight))
            using (var consoleProcess = ProcessFactory.Start(_processName, PseudoConsole.PseudoConsoleThreadAttribute, pseudoConsole.Handle))
            {
                _readingThread.Start();

                _processRunning = true;
                _pseudoConsoleHandle = pseudoConsole.Handle;
                _processHandle = consoleProcess.ProcessInfo.hProcess;
                _processId = consoleProcess.ProcessInfo.dwProcessId;

                ProcessStarted?.Invoke(_processId);

                WaitExit(consoleProcess);
            }

            _processRunning = false;

            ProcessExited?.Invoke(_processId);
        }

        private void ConsoleDataRead()
        {
            var buffer = new char[1024];

            try
            {
                while (_processRunning)
                {

                    int bytesRead;

                    if ((bytesRead = _consoleReader.Read(buffer, 0, buffer.Length)) != -1)
                    {
                        var args = new TerminalOutputEventArgs(
                            buffer.AsSpan(0, bytesRead).ToString()
                        );

                        TerminalOutput?.Invoke(this, args);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignore racing condition exceptions.
            }
        }

        private static void WaitExit(Process process)
        {
            var waitEvent = new AutoResetEvent(false)
            {
                SafeWaitHandle = new SafeWaitHandle(
                    process.ProcessInfo.hProcess,
                    ownsHandle: false
                )
            };

            waitEvent.WaitOne(Timeout.Infinite);
        }
    }
}
