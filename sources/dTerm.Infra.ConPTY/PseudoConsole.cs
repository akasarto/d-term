using Microsoft.Win32.SafeHandles;
using System;
using static dTerm.Core.WinApi;

namespace dTerm.Infra.ConPTY
{
    public sealed class PseudoConsole : IDisposable
    {
        public static readonly IntPtr PseudoConsoleThreadAttribute = (IntPtr)PROC_THREAD_ATTRIBUTE_PSEUDOCONSOLE;

        public IntPtr Handle { get; }

        private PseudoConsole(IntPtr handle)
        {
            Handle = handle;
        }

        public static PseudoConsole Create(SafeFileHandle inputReadSide, SafeFileHandle outputWriteSide, int width, int height)
        {
            var createResult = CreatePseudoConsole(
                new COORD
                {
                    X = (short)width,
                    Y = (short)height
                },
                inputReadSide,
                outputWriteSide,
                0,
                out IntPtr hPC
            );

            if (createResult != 0)
            {
                throw new InvalidOperationException("Could not create psuedo console. Error Code " + createResult);
            }

            return new PseudoConsole(hPC);
        }

        public void Dispose()
        {
            ClosePseudoConsole(Handle);
        }
    }
}
