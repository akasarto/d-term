using dTerm.Core;
using dTerm.Core.Processes;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace dTerm.UI.Wpf.Infrastructure
{
	public class ProcessHwndHost : HwndHost
	{
		private TermProcess _dtermProcess;

		public ProcessHwndHost(TermProcess dtermProcess)
		{
			_dtermProcess = dtermProcess;
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			var childHandle = _dtermProcess.MainWindowHandle;
			Win32ApiWrapper.SetParent(childHandle, hwndParent.Handle);
			Win32ApiWrapper.MakeChildWindow(childHandle);
			return new HandleRef(this, childHandle);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			Win32ApiWrapper.DestroyWindow(hwnd.Handle);
		}
	}
}
