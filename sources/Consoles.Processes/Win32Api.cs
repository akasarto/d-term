using Shared.Kernel;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WinApi.User32;

namespace Consoles.Processes
{
	internal static class Win32Api
	{
		/// <summary>
		/// Attempts to get the underlying process window handle.
		/// </summary>
		/// <returns><see cref="IntPtr"/> for the process main window.</returns>
		internal static IntPtr FindHiddenConsoleWindowHandle(Process consoleProcess)
		{
			uint threadId = 0;
			uint processId = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				processId = 0;
				consoleProcess.Refresh();
				windowHandle = User32Methods.FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = GetWindowThreadProcessId(windowHandle, out processId);
				if (processId == consoleProcess.Id)
				{
					return windowHandle;
				}
			} while (!windowHandle.Equals(IntPtr.Zero));

			return IntPtr.Zero;
		}

		[DllImport("user32.dll", ExactSpelling = true, CharSet = Win32Charset.Current)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
	}
}
