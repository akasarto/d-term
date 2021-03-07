using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static dTerm.Core.WinApi;

namespace dTerm.Infra.ConPTY
{
    public sealed class TerminalConsole
    {
        private const string CtrlC_Command = "\x3";

        public TerminalConsole()
        {
            EnableVirtualTerminalSequenceProcessing();
        }

        private static void EnableVirtualTerminalSequenceProcessing()
        {
            var hStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            if (!GetConsoleMode(hStdOut, out uint outConsoleMode))
            {
                throw new InvalidOperationException("Could not get console mode");
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;

            if (!SetConsoleMode(hStdOut, outConsoleMode))
            {
                throw new InvalidOperationException("Could not enable virtual terminal processing");
            }
        }

        public void Run(string command)
        {
            using (var inputPipe = new PseudoConsolePipe())
            using (var outputPipe = new PseudoConsolePipe())
            using (var pseudoConsole = PseudoConsole.Create(inputPipe.ReadSide, outputPipe.WriteSide, (short)Console.WindowWidth, (short)Console.WindowHeight))
            using (var process = ProcessFactory.Start(command, PseudoConsole.PseudoConsoleThreadAttribute, pseudoConsole.Handle))
            {
                Task.Run(() => CopyPipeToOutput(outputPipe.ReadSide));
                Task.Run(() => CopyInputToPipe(inputPipe.WriteSide));
                OnClose(() => DisposeResources(process, pseudoConsole, outputPipe, inputPipe));
                WaitForExit(process).WaitOne(Timeout.Infinite);
            }
        }

        private static void CopyInputToPipe(SafeFileHandle inputWriteSide)
        {
            using (var writer = new StreamWriter(new FileStream(inputWriteSide, FileAccess.Write)))
            {
                ForwardCtrlC(writer);

                writer.AutoFlush = true;

                while (true)
                {
                    char key = Console.ReadKey(intercept: true).KeyChar;
                    writer.Write(key);
                }
            }
        }

        private static void ForwardCtrlC(StreamWriter writer)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                writer.Write(CtrlC_Command);
            };
        }

        private static void CopyPipeToOutput(SafeFileHandle outputReadSide)
        {
            using (var terminalOutput = Console.OpenStandardOutput())
            using (var pseudoConsoleOutput = new FileStream(outputReadSide, FileAccess.Read))
            {
                pseudoConsoleOutput.CopyTo(terminalOutput);
            }
        }

        private static AutoResetEvent WaitForExit(Process process) => new AutoResetEvent(false)
        {
            SafeWaitHandle = new SafeWaitHandle(process.ProcessInfo.hProcess, ownsHandle: false)
        };

        private static void OnClose(Action handler) => SetConsoleCtrlHandler(eventType =>
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
