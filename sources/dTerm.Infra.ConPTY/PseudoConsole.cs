using Microsoft.Win32.SafeHandles;
using System;

namespace dTerm.Infra.ConPTY
{
    public sealed class PseudoConsole : IDisposable
    {
        public static readonly IntPtr PseudoConsoleThreadAttribute = (IntPtr)WinNativeApi.PROC_THREAD_ATTRIBUTE_PSEUDOCONSOLE;

        public IntPtr Handle { get; }

        private PseudoConsole(IntPtr handle) => Handle = handle;

        public static PseudoConsole Create(SafeFileHandle inputReadSide, SafeFileHandle outputWriteSide, int width, int height)
        {
            var createResult = WinNativeApi.CreatePseudoConsole(
                new WinNativeApi.COORD
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

        public void Dispose() => WinNativeApi.ClosePseudoConsole(Handle);
    }
}
