using Shared.Kernel;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Processes.SystemDiagnostics
{
	internal static class Win32Api
	{
		/// <summary>
		/// Attempts to get the underlying process window handle.
		/// </summary>
		/// <returns><see cref="IntPtr"/> for the process main window.</returns>
		internal static IntPtr FindHiddenProcessWindowHandle(Process consoleProcess)
		{
			uint threadId = 0;
			uint processId = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				processId = 0;
				consoleProcess.Refresh();
				windowHandle = FindWindowEx(IntPtr.Zero, windowHandle, null, null);
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

		[DllImport("user32.dll", CharSet = Win32Charset.Current)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
	}
}
