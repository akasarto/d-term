using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Shared.Kernel
{
	public static class Win32Api
	{
		private const string user32Dll = "user32.dll";

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern bool DestroyWindow(IntPtr hwnd);

		[DllImport(user32Dll, CharSet = Properties.BuildCharSet)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern bool IsWindowVisible(IntPtr hwnd);

		[DllImport(user32Dll, ExactSpelling = true, CharSet = Properties.BuildCharSet)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern bool ShowWindow(IntPtr hwnd, ShowWindowCommands nCmdShow);

		[DllImport(user32Dll, CharSet = Properties.BuildCharSet)]
		public static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		public static IntPtr GetWindowLongPtr(IntPtr hwnd, WindowLongFlags nIndex)
		{
			if (IntPtr.Size > 4)
			{
				return GetWindowLongPtr(hwnd, (int)nIndex);
			}

			return new IntPtr(GetWindowLong(hwnd, (int)nIndex));
		}

		public static IntPtr SetWindowLongPtr(IntPtr hwnd, WindowLongFlags nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size > 4)
			{
				return SetWindowLongPtr(hwnd, (int)nIndex, dwNewLong);
			}

			return new IntPtr(SetWindowLong(hwnd, (int)nIndex, dwNewLong.ToInt32()));
		}

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		[DllImport(user32Dll, ExactSpelling = true)]
		public static extern bool SetForegroundWindow(IntPtr hwnd);

		[DllImport(user32Dll, CharSet = Properties.BuildCharSet)]
		private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[DllImport(user32Dll, CharSet = Properties.BuildCharSet, EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr(IntPtr hwnd, int nIndex);

		[DllImport(user32Dll, CharSet = Properties.BuildCharSet)]
		private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
		[DllImport(user32Dll, CharSet = Properties.BuildCharSet, EntryPoint = "SetWindowLongPtr")]
		private static extern IntPtr SetWindowLongPtr(IntPtr hwnd, int nIndex, IntPtr dwNewLong);
	}
}
