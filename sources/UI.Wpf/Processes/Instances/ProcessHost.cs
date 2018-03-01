using Processes.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	public interface IProcessHost
	{
	}

	public class ProcessHost : HwndHost, IProcessHost
	{
		private IProcess _process;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessHost(IProcess process)
		{
			_process = process ?? throw new ArgumentNullException(nameof(process), nameof(ProcessHost));
		}

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			var parentHandle = hwndParent.Handle;
			var childHandle = _process.MainWindowHandle;

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
