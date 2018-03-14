using Splat;
using System;
using ReactiveUI;
using System.Windows.Interop;
using WinApi.User32;
using UI.Wpf.Properties;

namespace UI.Wpf.Processes
{
	//
	public interface IInstanceInteropManager
	{
		void Track(IInstanceViewModel instance);
		void Release(IInstanceViewModel instance);
	}

	//
	public class InstanceInteropManager : ReactiveObject, IInstanceInteropManager
	{
		private readonly IntPtr _shellViewHandle;
		private readonly IAppState _appState;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public InstanceInteropManager(IntPtr shellViewHandle, IAppState appState = null)
		{
			_shellViewHandle = shellViewHandle;
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();
		}

		public void Track(IInstanceViewModel instance)
		{
			var iconHandle = Resources.dTermIcon.Handle;

			Win32Api.RemoveFromTaskbar(instance.ProcessMainWindowHandle);
			Win32Api.SetProcessWindowIcon(instance.ProcessMainWindowHandle, iconHandle);
			Win32Api.SetProcessWindowOwner(instance.ProcessMainWindowHandle, _shellViewHandle);
		}

		public void Release(IInstanceViewModel instance)
		{

		}
	}
}
