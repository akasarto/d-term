using System;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.User32;

namespace UI.Wpf
{
	internal static class Win32Api
	{
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

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
	}
}
