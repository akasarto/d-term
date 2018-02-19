using Shared.Kernel;
using System;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.User32;

namespace UI.Wpf
{
	internal delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

	internal static class Win32Api
	{
		/// <summary>
		/// Send win messages to paint the corresponding window as active.
		/// </summary>
		/// <param name="handle">The pointer to the window that should be painted as active.</param>
		internal static void SetVisualAsActive(IntPtr handle)
		{
			PostMessage(handle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			PostMessage(handle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);
		}

		/// <summary>
		/// Check if the given handle is from a console process.
		/// </summary>
		/// <param name="hWnd">The process windo handle to test.</param>
		/// <returns><c>True</c> if the handle is a console process, otherwise, <c>false</c>.</returns>
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

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true, CharSet = Win32Charset.Current)]
		internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
	}
}
