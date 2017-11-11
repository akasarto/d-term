using dTerm.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using WinApi.User32;

namespace dTerm.UI.Wpf.Infrastructure
{
	public class ConsoleHwndHost : HwndHost
	{
		private IConsoleInstance _consoleInstance;

		public ConsoleHwndHost(IConsoleInstance consoleInstance)
		{
			_consoleInstance = consoleInstance ?? throw new ArgumentNullException(nameof(consoleInstance), nameof(ConsoleHwndHost));
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			var childHandle = _consoleInstance.ProcessMainWindowHandle;
			Integrate(childHandle, hwndParent.Handle);
			return new HandleRef(this, childHandle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			User32Methods.DestroyWindow(hwnd.Handle);
		}

		private void Integrate(IntPtr childHandle, IntPtr parentHandle)
		{
			// Set Parent
			User32Methods.SetParent(childHandle, parentHandle);

			// Make child window
			var newStyle = (WindowStyles)User32Helpers.GetWindowLongPtr(childHandle, WindowLongFlags.GWL_STYLE);

			newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
			newStyle &= ~WindowStyles.WS_MINIMIZEBOX;
			newStyle &= ~WindowStyles.WS_THICKFRAME;
			newStyle &= ~WindowStyles.WS_DLGFRAME;
			newStyle &= ~WindowStyles.WS_BORDER;
			newStyle &= ~WindowStyles.WS_POPUP;

			newStyle |= WindowStyles.WS_CHILD;

			User32Helpers.SetWindowLongPtr(childHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));

			User32Methods.ShowWindow(childHandle, ShowWindowCommands.SW_SHOW);
		}
	}
}
