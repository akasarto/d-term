using Processes.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process instance host interface.
	/// </summary>
	public interface IProcessInstanceHost
	{
	}

	/// <summary>
	/// App process instance host implementation.
	/// <para>Hosts Win32 process windows into WPF controls.</para>
	/// </summary>
	public class ProcessInstanceHost : HwndHost, IProcessInstanceHost
	{
		//
		private IProcessInstance _processInstance;

		/// <summary>
		/// Constructor method.
		/// </summary>
		/// <param name="processInstance">The started process instance to be hosted.</param>
		public ProcessInstanceHost(IProcessInstance processInstance)
		{
			_processInstance = processInstance ?? throw new ArgumentNullException(nameof(processInstance), nameof(ProcessInstanceHost));
		}

		/// <summary>
		/// Creates the child window to be hosted.
		/// </summary>
		/// <param name="hwndParent">The parent window handle ref.</param>
		/// <returns>A <see cref="HandleRef"/> instance.</returns>
		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			var parentHandle = hwndParent.Handle;
			var childHandle = _processInstance.MainWindowHandle;

			//
			_processInstance.ParentHandle = parentHandle;

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

		/// <summary>
		/// Destroy the hosted windo.
		/// </summary>
		/// <param name="hwnd">The hadle reference for the window to be destroyed.</param>
		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			User32Methods.DestroyWindow(hwnd.Handle);
		}
	}
}
