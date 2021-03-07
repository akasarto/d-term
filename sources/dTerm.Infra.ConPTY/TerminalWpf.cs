using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Threading;
using static dTerm.Core.WinApi;

namespace dTerm.Infra.ConPTY
{
    public sealed class TerminalWpf
    {
        public StreamWriter InputStream { get; private set; }
        public FileStream OutputStream { get; private set; }

        public delegate void InstanceReadyHandler();

        public event InstanceReadyHandler InstanceReady;

        public void Start(string command, int consoleWidth = 80, int consoleHeight = 30)
        {
            using (var inputPipe = new PseudoConsolePipe())
            {
                using (var outputPipe = new PseudoConsolePipe())
                {
                    using (var pseudoConsole = PseudoConsole.Create(inputPipe.ReadSide, outputPipe.WriteSide, consoleWidth, consoleHeight))
                    {
                        using (var process = ProcessFactory.Start(command, PseudoConsole.PseudoConsoleThreadAttribute, pseudoConsole.Handle))
                        {
                            InputStream = new StreamWriter(new FileStream(inputPipe.WriteSide, FileAccess.Write))
                            {
                                AutoFlush = true
                            };
                            OutputStream = new FileStream(outputPipe.ReadSide, FileAccess.Read);
                            InstanceReady?.Invoke();
                            OnClose(() => DisposeResources(process, pseudoConsole, outputPipe, inputPipe, InputStream));
                            WaitForExit(process).WaitOne(Timeout.Infinite);
                        }
                    }
                }
            }
        }

        public void Write(char @char) => InputStream.Write(@char);

        private AutoResetEvent WaitForExit(Process process) => new AutoResetEvent(false)
        {
            SafeWaitHandle = new SafeWaitHandle(process.ProcessInfo.hProcess, ownsHandle: false)
        };

        private void OnClose(Action handler) => SetConsoleCtrlHandler(eventType =>
        {
            if (eventType == CtrlTypes.CTRL_CLOSE_EVENT)
            {
                handler();
            }
            return false;
        }, true);

        private void DisposeResources(params IDisposable[] disposables)
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
