using dTerm.Core;
using dTerm.Core.Processes;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.Infrastructure
{
	public class ProcessHwndHost : HwndHost
	{
		private IConsoleInstance _dtermProcess;

		public ProcessHwndHost(IConsoleInstance dtermProcess)
		{
			_dtermProcess = dtermProcess;
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			var childHandle = _dtermProcess.ProcessMainWindowHandle;
			Win32Api.SetParent(childHandle, hwndParent.Handle);
			Win32Api.MakeChildWindow(childHandle);
			return new HandleRef(this, childHandle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			Win32Api.DestroyWindow(hwnd.Handle);
		}
	}
}
