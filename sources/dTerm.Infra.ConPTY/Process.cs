using System;
using System.Runtime.InteropServices;

namespace dTerm.Infra.ConPTY
{
    public sealed class Process : IDisposable
    {
        private bool _disposedValue = false;

        public Process(WinNativeApi.STARTUPINFOEX startupInfo, WinNativeApi.PROCESS_INFORMATION processInfo)
        {
            StartupInfo = startupInfo;
            ProcessInfo = processInfo;
        }

        ~Process()
        {
            Dispose(false);
        }

        public WinNativeApi.STARTUPINFOEX StartupInfo { get; }
        public WinNativeApi.PROCESS_INFORMATION ProcessInfo { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                // dispose unmanaged state

                // Free the attribute list
                if (StartupInfo.lpAttributeList != IntPtr.Zero)
                {
                    WinNativeApi.DeleteProcThreadAttributeList(StartupInfo.lpAttributeList);
                    Marshal.FreeHGlobal(StartupInfo.lpAttributeList);
                }

                // Close process and thread handles
                if (ProcessInfo.hProcess != IntPtr.Zero)
                {
                    WinNativeApi.CloseHandle(ProcessInfo.hProcess);
                }
                if (ProcessInfo.hThread != IntPtr.Zero)
                {
                    WinNativeApi.CloseHandle(ProcessInfo.hThread);
                }

                _disposedValue = true;
            }
        }
    }
}
