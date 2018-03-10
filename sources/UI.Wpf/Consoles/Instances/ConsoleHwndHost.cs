using Processes.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using WinApi.User32;

namespace UI.Wpf.Consoles
{
	public interface IConsoleHwndHost
	{
	}

	public class ConsoleHwndHost : HwndHost, IConsoleHwndHost
	{
		private IProcess _process;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleHwndHost(IProcess process)
		{
			_process = process ?? throw new ArgumentNullException(nameof(process), nameof(ConsoleHwndHost));
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			var parentHandle = hwndParent.Handle;
			var childHandle = _process.MainWindowHandle;

			Win32Api.MakeToolWindow(parentHandle);

			//
			_process.ParentHandle = parentHandle;

			//
			User32Methods.SetParent(childHandle, parentHandle);

			//
			var newStyle = (WindowStyles)User32Helpers.GetWindowLongPtr(childHandle, WindowLongFlags.GWL_STYLE);

			newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
			newStyle &= ~WindowStyles.WS_MINIMIZEBOX;
			newStyle &= ~WindowStyles.WS_THICKFRAME;
			newStyle &= ~WindowStyles.WS_DLGFRAME;
			newStyle &= ~WindowStyles.WS_BORDER;
			newStyle &= ~WindowStyles.WS_POPUP;

			newStyle |= WindowStyles.WS_CHILD;

			//
			User32Helpers.SetWindowLongPtr(childHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));

			return new HandleRef(this, childHandle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			User32Methods.DestroyWindow(hwnd.Handle);
		}
	}
}
