using dTerm.Consoles.Core;
using dTerm.WinNative;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.Consoles
{
	public class ConsoleHwndHost : HwndHost
	{
		private IConsoleProcess _consoleProcess;

		public ConsoleHwndHost(IConsoleProcess consoleProcess)
		{
			_consoleProcess = consoleProcess ?? throw new ArgumentNullException(nameof(consoleProcess), nameof(ConsoleHwndHost));
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			var childHandle = _consoleProcess.MainWindowHandle;
			IntegrateConsole(childHandle, hwndParent.Handle);
			return new HandleRef(this, childHandle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			NativeMethods.DestroyWindow(hwnd.Handle);
		}

		private void IntegrateConsole(IntPtr childHandle, IntPtr parentHandle)
		{
			//
			NativeMethods.SetParent(childHandle, parentHandle);

			//
			var newStyle = (WindowStyles)NativeMethods.GetWindowLongPtr(childHandle, WindowLongFlags.GWL_STYLE);

			newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
			newStyle &= ~WindowStyles.WS_MINIMIZEBOX;
			newStyle &= ~WindowStyles.WS_THICKFRAME;
			newStyle &= ~WindowStyles.WS_DLGFRAME;
			newStyle &= ~WindowStyles.WS_BORDER;
			newStyle &= ~WindowStyles.WS_POPUP;

			newStyle |= WindowStyles.WS_CHILD;

			NativeMethods.SetWindowLongPtr(childHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));
		}
	}
}
