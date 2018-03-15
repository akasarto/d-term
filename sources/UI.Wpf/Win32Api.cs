using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.User32;

namespace UI.Wpf
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct Win32Param
	{
		[FieldOffset(0)]
		public uint BaseValue;

		[FieldOffset(2)]
		public ushort HOWord;

		[FieldOffset(0)]
		public ushort LOWord;
	}

	internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

	internal static class Win32Api
	{
		internal static void SetVisualAsActive(IntPtr handle)
		{
			PostMessage(handle, (uint)WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
			PostMessage(handle, (uint)WM.NCPAINT, new IntPtr(1), IntPtr.Zero);
		}

		internal static bool IsConsoleClass(IntPtr hWnd)
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

		internal static bool IsOwnedWindow(IntPtr windowHandle)
		{
			return User32Helpers.GetWindow(windowHandle, GetWindowFlag.GW_OWNER) != IntPtr.Zero;
		}

		internal static string GetWindowTitle(IntPtr hWnd)
		{
			int length = User32Methods.GetWindowTextLength(hWnd);
			StringBuilder stringBuilder = new StringBuilder(length + 1);
			User32Methods.GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		internal static void SetWindowTitle(IntPtr targetWindowHandle, string newTitle)
		{
			User32Methods.SetWindowText(targetWindowHandle, newTitle);
		}

		internal static void SetWindowIcons(IntPtr targetWindowHandle, IntPtr newIconHandle)
		{
			User32Methods.SendMessage(targetWindowHandle, 0x80, new IntPtr(0), newIconHandle);
			User32Methods.SendMessage(targetWindowHandle, 0x80, new IntPtr(1), newIconHandle);
		}

		internal static void SetWindowOwner(IntPtr targetWindoHandle, IntPtr parentWindowHandle)
		{
			User32Helpers.SetWindowLongPtr(targetWindoHandle, WindowLongFlags.GWLP_HWNDPARENT, parentWindowHandle);
		}

		internal static void RemoveWindowFromTaskbar(IntPtr targetWindowHandle)
		{
			var newStyle = (WindowExStyles)User32Helpers.GetWindowLongPtr(targetWindowHandle, WindowLongFlags.GWL_EXSTYLE);

			newStyle &= ~WindowExStyles.WS_EX_APPWINDOW;

			User32Helpers.SetWindowLongPtr(targetWindowHandle, WindowLongFlags.GWL_EXSTYLE, new IntPtr((long)newStyle));
		}

		internal static void MakeToolWindow(IntPtr targetWindowHandle)
		{
			var newStyle = (WindowStyles)User32Helpers.GetWindowLongPtr(targetWindowHandle, WindowLongFlags.GWL_STYLE);

			newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
			newStyle &= ~WindowStyles.WS_MINIMIZEBOX;

			User32Helpers.SetWindowLongPtr(targetWindowHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));
		}

		internal static string GetWindowClassName(IntPtr targetWindowHandle)
		{
			int outLength;
			var stringBuilder = new StringBuilder(256);

			outLength = GetClassName(targetWindowHandle, stringBuilder, stringBuilder.Capacity);

			if (outLength != 0)
			{
				return stringBuilder.ToString();
			}

			return string.Empty;
		}

		internal static IEnumerable<IntPtr> AddEventsHook(uint[] winEvents, WinEventDelegate winEventsDelegate)
		{
			var handles = new List<IntPtr>();

			foreach (var @event in winEvents)
			{
				var hookHandle = SetWinEventHook(@event, @event, IntPtr.Zero, winEventsDelegate, 0, 0, 0);

				handles.Add(hookHandle);
			}

			return handles;
		}

		internal static void RemoveEventHook(IntPtr hookHandle)
		{
			UnhookWinEvent(hookHandle);
		}

		internal static void MakeLayeredWindow(IntPtr targetWindowHandle)
		{
			User32Helpers.SetWindowLongPtr(
				targetWindowHandle,
				WindowLongFlags.GWL_EXSTYLE,
				new IntPtr(
					(int)User32Helpers.GetWindowLongPtr(targetWindowHandle, WindowLongFlags.GWL_EXSTYLE)
					| (int)WindowExStyles.WS_EX_LAYERED
				)
			);
		}

		internal static void SetTransparency(IntPtr targetWindowHandle, byte opacityLevel)
		{
			User32Methods.SetLayeredWindowAttributes(targetWindowHandle, 0, opacityLevel, LayeredWindowAttributeFlag.LWA_ALPHA);
		}

		internal static IntPtr GetProcessWindow(int processId)
		{
			uint threadId = 0;
			uint ownerPid = 0;
			IntPtr windowHandle = IntPtr.Zero;

			do
			{
				ownerPid = 0;
				windowHandle = FindWindowEx(IntPtr.Zero, windowHandle, null, null);
				threadId = GetWindowThreadProcessId(windowHandle, out ownerPid);
				if (ownerPid == processId && IsMainWindow(windowHandle))
				{
					return windowHandle;
				}
			} while (!windowHandle.Equals(IntPtr.Zero));

			return IntPtr.Zero;
		}

		private static bool IsMainWindow(IntPtr windowHandle)
		{
			var style = (WindowStyles)User32Helpers.GetWindowLongPtr(windowHandle, WindowLongFlags.GWL_STYLE);

			var isMinimized = (style & WindowStyles.WS_MINIMIZE) == WindowStyles.WS_MINIMIZE;

			return
				User32Methods.IsWindow(windowHandle)
				&& (User32Methods.IsWindowVisible(windowHandle) || isMinimized);
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

		[DllImport("user32.dll")]
		private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
	}
}
