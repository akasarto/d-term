using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.User32;

namespace UI.Wpf
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Win32Param
	{
		[FieldOffset(0)]
		public uint BaseValue;

		[FieldOffset(2)]
		public ushort HOWord;

		[FieldOffset(0)]
		public ushort LOWord;
	}

	internal static class Win32Api
	{
		internal static void SetVisualAsActive(IntPtr handle)
		{
			PostMessage(handle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			PostMessage(handle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);
		}

		internal static bool IsConsoleProcess(IntPtr hWnd)
		{
			int outLength;
			var stringBuilder = new StringBuilder(256);

			outLength = User32Methods.GetClassName(hWnd, stringBuilder, stringBuilder.Capacity);

			if (outLength != 0)
			{
				return stringBuilder.ToString().ToLower().Contains("consolewindowclass");
			}
			else
			{
				return false;
			}
		}

		public static void TakeOwnership(IntPtr targetWindoHandle, IntPtr parentWindowHandle)
		{
			User32Helpers.SetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWLP_HWNDPARENT, parentWindowHandle);
		}

		public static void RemoveFromTaskbar(IntPtr targetWindoHandle)
		{
			var newStyle = (WindowExStyles)User32Helpers.GetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWL_EXSTYLE);

			newStyle &= ~WindowExStyles.WS_EX_APPWINDOW;

			User32Helpers.SetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWL_EXSTYLE, new IntPtr((long)newStyle));
		}

		public static void MakeToolWindow(IntPtr targetWindoHandle)
		{
			var newStyle = (WindowStyles)User32Helpers.GetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWL_STYLE);

			newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
			newStyle &= ~WindowStyles.WS_MINIMIZEBOX;

			User32Helpers.SetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));
		}

		public static string GetWindowClassName(IntPtr hWnd)
		{
			int outLength;
			var stringBuilder = new StringBuilder(256);

			outLength = GetClassName(hWnd, stringBuilder, stringBuilder.Capacity);

			if (outLength != 0)
			{
				return stringBuilder.ToString();
			}

			return string.Empty;
		}

		public static IntPtr GetProcessMainWindowHandle(Process process)
		{
			uint threadId = 0;
			uint processId = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				processId = 0;
				process.Refresh();
				windowHandle = FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = GetWindowThreadProcessId(windowHandle, out processId);
				if (processId == process.Id)
				{
					return windowHandle;
				}
			} while (!windowHandle.Equals(IntPtr.Zero));

			return IntPtr.Zero;
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
	}
}
