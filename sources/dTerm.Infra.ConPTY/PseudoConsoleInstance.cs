using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Text;
using System.Threading;
using static dTerm.Infra.ConPTY.ConPtyApi;

namespace dTerm.Infra.ConPTY
{
    public sealed class PseudoConsoleInstance
    {
        private readonly Encoding _encoding;

        public FileStream InputStream { get; private set; }
        public FileStream OutputStream { get; private set; }

        public PseudoConsoleInstance(Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.UTF8;
        }

        public void Start(string command, int consoleWidth = 80, int consoleHeight = 30)
        {
            using (var inputPipe = new ConPtyPseudoConsolePipe())
            {
                using (var outputPipe = new ConPtyPseudoConsolePipe())
                {
                    using (var pseudoConsole = ConPtyPseudoConsoleHandle.Create(inputPipe.ReadSide, outputPipe.WriteSide, consoleWidth, consoleHeight))
                    {
                        using (var process = ConPtyProcessFactory.Start(command, ConPtyPseudoConsoleHandle.PseudoConsoleThreadAttribute, pseudoConsole.Handle))
                        {
                            InputStream = new FileStream(inputPipe.WriteSide, FileAccess.Write);
                            OutputStream = new FileStream(outputPipe.ReadSide, FileAccess.Read);
                            OnClose(() => DisposeResources(process, pseudoConsole, outputPipe, inputPipe, InputStream));
                            WaitForExit(process).WaitOne(Timeout.Infinite);
                        }
                    }
                }
            }
        }

        public void Write(string command)
        {
            using (var writer = new StreamWriter(InputStream, _encoding, bufferSize: _encoding.GetByteCount(command), leaveOpen: true))
            {
                writer.WriteLine(command);
            }
        }

        private AutoResetEvent WaitForExit(ConPtyProcess process) => new AutoResetEvent(false)
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
