using App.Consoles.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

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
			//_consoleInstance.SetOwner(hwndParent.Handle);
			var childHandle = _consoleInstance.MainWindowHandle;
			//IntegrateConsole(childHandle, hwndParent.Handle);
			return new HandleRef(this, childHandle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			DestroyWindow(hwnd.Handle);
		}

		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern bool DestroyWindow(IntPtr hwnd);

		//private void IntegrateConsole(IntPtr childHandle, IntPtr parentHandle)
		//{
		//	//
		//	NativeMethods.SetParent(childHandle, parentHandle);

		//	//
		//	var newStyle = (WindowStyles)NativeMethods.GetWindowLongPtr(childHandle, WindowLongFlags.GWL_STYLE);

		//	newStyle &= ~WindowStyles.WS_MAXIMIZEBOX;
		//	newStyle &= ~WindowStyles.WS_MINIMIZEBOX;
		//	newStyle &= ~WindowStyles.WS_THICKFRAME;
		//	newStyle &= ~WindowStyles.WS_DLGFRAME;
		//	newStyle &= ~WindowStyles.WS_BORDER;
		//	newStyle &= ~WindowStyles.WS_POPUP;

		//	newStyle |= WindowStyles.WS_CHILD;

		//	NativeMethods.SetWindowLongPtr(childHandle, WindowLongFlags.GWL_STYLE, new IntPtr((long)newStyle));
		//}
	}
}
