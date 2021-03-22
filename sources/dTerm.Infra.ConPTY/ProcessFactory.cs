using System;
using System.Runtime.InteropServices;

namespace dTerm.Infra.ConPTY
{
    public static class ProcessFactory
    {
        public static Process Start(string command, IntPtr attributes, IntPtr hPC)
        {
            var startupInfo = ConfigureProcessThread(hPC, attributes);
            var processInfo = RunProcess(ref startupInfo, command);

            return new Process(startupInfo, processInfo);
        }

        private static WinNativeApi.STARTUPINFOEX ConfigureProcessThread(IntPtr hPC, IntPtr attributes)
        {
            var lpSize = IntPtr.Zero;

            var success = WinNativeApi.InitializeProcThreadAttributeList(
                lpAttributeList: IntPtr.Zero,
                dwAttributeCount: 1,
                dwFlags: 0,
                lpSize: ref lpSize
            );

            if (success || lpSize == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not calculate the number of bytes for the attribute list. " + Marshal.GetLastWin32Error());
            }

            var startupInfo = new WinNativeApi.STARTUPINFOEX();

            startupInfo.StartupInfo.cb = Marshal.SizeOf<WinNativeApi.STARTUPINFOEX>();
            startupInfo.lpAttributeList = Marshal.AllocHGlobal(lpSize);

            success = WinNativeApi.InitializeProcThreadAttributeList(
                lpAttributeList: startupInfo.lpAttributeList,
                dwAttributeCount: 1,
                dwFlags: 0,
                lpSize: ref lpSize
            );

            if (!success)
            {
                throw new InvalidOperationException("Could not set up attribute list. " + Marshal.GetLastWin32Error());
            }

            success = WinNativeApi.UpdateProcThreadAttribute(
                lpAttributeList: startupInfo.lpAttributeList,
                dwFlags: 0,
                attribute: attributes,
                lpValue: hPC,
                cbSize: (IntPtr)IntPtr.Size,
                lpPreviousValue: IntPtr.Zero,
                lpReturnSize: IntPtr.Zero
            );

            if (!success)
            {
                throw new InvalidOperationException("Could not set pseudoconsole thread attribute. " + Marshal.GetLastWin32Error());
            }

            return startupInfo;
        }

        private static WinNativeApi.PROCESS_INFORMATION RunProcess(ref WinNativeApi.STARTUPINFOEX sInfoEx, string commandLine)
        {
            int securityAttributeSize = Marshal.SizeOf<WinNativeApi.SECURITY_ATTRIBUTES>();
            var pSec = new WinNativeApi.SECURITY_ATTRIBUTES { nLength = securityAttributeSize };
            var tSec = new WinNativeApi.SECURITY_ATTRIBUTES { nLength = securityAttributeSize };
            var success = WinNativeApi.CreateProcess(
                lpApplicationName: null,
                lpCommandLine: commandLine,
                lpProcessAttributes: ref pSec,
                lpThreadAttributes: ref tSec,
                bInheritHandles: false,
                dwCreationFlags: WinNativeApi.EXTENDED_STARTUPINFO_PRESENT,
                lpEnvironment: IntPtr.Zero,
                lpCurrentDirectory: null,
                lpStartupInfo: ref sInfoEx,
                lpProcessInformation: out WinNativeApi.PROCESS_INFORMATION pInfo
            );

            if (!success)
            {
                throw new InvalidOperationException("Could not create process. " + Marshal.GetLastWin32Error());
            }

            return pInfo;
        }
    }
}
