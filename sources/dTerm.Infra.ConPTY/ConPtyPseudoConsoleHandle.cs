using Microsoft.Win32.SafeHandles;
using System;
using static dTerm.Infra.ConPTY.ConPtyApi;

namespace dTerm.Infra.ConPTY
{
    internal sealed class ConPtyPseudoConsoleHandle : IDisposable
    {
        public static readonly IntPtr PseudoConsoleThreadAttribute = (IntPtr)PROC_THREAD_ATTRIBUTE_PSEUDOCONSOLE;

        public IntPtr Handle { get; }

        private ConPtyPseudoConsoleHandle(IntPtr handle)
        {
            Handle = handle;
        }

        internal static ConPtyPseudoConsoleHandle Create(SafeFileHandle inputReadSide, SafeFileHandle outputWriteSide, int width, int height)
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

            return new ConPtyPseudoConsoleHandle(hPC);
        }

        public void Dispose()
        {
            ClosePseudoConsole(Handle);
        }
    }
}
