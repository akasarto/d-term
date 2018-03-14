using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using UI.Wpf.Properties;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	public interface IInstancesManager
	{
		IReactiveDerivedList<IInstanceViewModel> GetAllInstances();
		IReactiveDerivedList<IInstanceViewModel> GetMinimizedInstances();
		void Track(IInstanceViewModel instance);
	}

	public class InstancesManager : ReactiveObject, IInstancesManager
	{
		private readonly IntPtr _shellViewHandle;
		private readonly IReactiveList<IInstanceViewModel> _instancesSource;
		private readonly IReactiveDerivedList<IInstanceViewModel> _allInstancesList;
		private readonly IReactiveDerivedList<IInstanceViewModel> _minimizedInstancesList;

		private Dictionary<int, (WinEventDelegate hookDelegate, IntPtr hookHandle)> _processHooksTracker;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public InstancesManager(IntPtr shellViewHandle)
		{
			_shellViewHandle = shellViewHandle;
			_processHooksTracker = new Dictionary<int, (WinEventDelegate hookDelegate, IntPtr hookHandle)>();

			_instancesSource = new ReactiveList<IInstanceViewModel>()
			{
				ChangeTrackingEnabled = true
			};

			_instancesSource.ItemsAdded.Subscribe(instance => Integrate(instance));
			_instancesSource.ItemsRemoved.Subscribe(instance => Release(instance));

			_allInstancesList = _instancesSource.CreateDerivedCollection(
				selector: instance => instance
			);

			_minimizedInstancesList = _instancesSource.CreateDerivedCollection(
				filter: instance => instance.IsMinimized,
				selector: instance => instance
			);
		}

		public IReactiveDerivedList<IInstanceViewModel> GetAllInstances() => _allInstancesList;

		public IReactiveDerivedList<IInstanceViewModel> GetMinimizedInstances() => _minimizedInstancesList;

		public void Track(IInstanceViewModel instance)
		{
			var subscription = instance.ProcessTerminated.ObserveOnDispatcher().Subscribe(@event =>
			{
				_instancesSource.Remove(instance);
			});

			_instancesSource.Add(instance);
		}

		private void Integrate(IInstanceViewModel instance)
		{
			var iconHandle = Resources.dTermIcon.Handle;
			var instanceHandle = instance.ProcessMainWindowHandle;
			var isntanceWndTitle = $"[{instance.ProcessId}] {instance.Name}";

			SetWindowTitle(instance);
			Win32Api.HideFromTaskbar(instanceHandle);
			Win32Api.MakeLayeredWindow(instanceHandle);
			Win32Api.SetProcessWindowIcon(instanceHandle, iconHandle);
			Win32Api.SetProcessWindowOwner(instanceHandle, _shellViewHandle);

			var hookProc = new WinEventDelegate(WinEventProc);
			var hookHandle = Win32Api.AddEventsHook(instanceHandle, hookProc);

			_processHooksTracker.Add(instance.ProcessId, (hookProc, hookHandle));
		}

		public void Release(IInstanceViewModel instance)
		{
			var hookData = _processHooksTracker[instance.ProcessId];
			_processHooksTracker.Remove(instance.ProcessId);
			Win32Api.RemoveEventsHook(hookData.hookHandle);
		}

		private void SetWindowTitle(IInstanceViewModel instance)
		{
			var wndHandle = instance.ProcessMainWindowHandle;
			var windowTitle = $"[{instance.ProcessId}] {instance.Name}";
			Win32Api.SetProcessWindowTitle(wndHandle, windowTitle);
		}

		private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
		{
			if (idObject != 0 || idChild != 0)
			{
				return;
			}

			var instance = _instancesSource.SingleOrDefault(i => i.ProcessMainWindowHandle == hwnd);

			if (instance == null)
			{
				return;
			}

			switch (eventType)
			{
				// EVENT_SYSTEM_MINIMIZESTART
				case 0x0016:
					{
						instance.IsMinimized = true;
						User32Methods.ShowWindow(hwnd, ShowWindowCommands.SW_HIDE);
					}
					break;

				// EVENT_OBJECT_NAMECHANGE
				case 0x800C:
					{
						SetWindowTitle(instance);
					}
					break;

				// EVENT_SYSTEM_FOREGROUND
				case 0x0003:
					{
					}
					break;
			}
		}
	}
}
