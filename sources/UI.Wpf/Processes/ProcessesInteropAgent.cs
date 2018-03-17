using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using UI.Wpf.Properties;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	public interface IProcessesInteropAgent : IDisposable
	{
	}

	public class ProcessesInteropAgent : ReactiveObject, IProcessesInteropAgent
	{
		private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
		private const uint EVENT_OBJECT_NAMECHANGE = 0x800C;
		private const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
		private const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;

		private readonly uint[] _winEvents = new uint[]
		{
			EVENT_SYSTEM_FOREGROUND,
			EVENT_OBJECT_NAMECHANGE,
			EVENT_SYSTEM_MINIMIZESTART,
			EVENT_SYSTEM_MINIMIZEEND
		};

		private readonly IAppState _appState;
		private readonly WinEventDelegate _winEventDelegate;
		private readonly IEnumerable<IntPtr> _eventHookHandlers;
		private readonly IReactiveDerivedList<IProcessInstanceViewModel> _integratedInstances;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesInteropAgent(IAppState appState = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();

			_winEventDelegate = new WinEventDelegate(WinEventsHandler);
			_eventHookHandlers = Win32Api.AddEventsHook(_winEvents, _winEventDelegate);

			_integratedInstances = _appState.GetProcessInstances().CreateDerivedCollection(
				filter: instance => !instance.IsElevated,
				selector: instance => instance
			);

			// Integrate
			_integratedInstances.ItemsAdded.Subscribe(instance => Integrate(instance));
		}

		public void Dispose()
		{
			foreach (var handle in _eventHookHandlers)
			{
				Win32Api.RemoveEventHook(handle);
			}
		}

		private void Integrate(IProcessInstanceViewModel instance)
		{
			var handle = instance.ProcessMainWindowHandle;
			TakeWindowOwnership(handle);
			SetWindowTitleBar(instance);
		}

		private void TakeWindowOwnership(IntPtr windowHandle)
		{
			var shellViewHandle = _appState.GetShellViewHandle();

			Win32Api.SetWindowOwner(windowHandle, shellViewHandle);
			Win32Api.RemoveWindowFromTaskbar(windowHandle);
			Win32Api.MakeLayeredWindow(windowHandle);
		}

		private void SetWindowTitleBar(IProcessInstanceViewModel instance)
		{
			var icon = Resources.dTermIcon.Handle;
			var handle = instance.ProcessMainWindowHandle;
			var wndTitle = Win32Api.GetWindowTitle(handle);
			var cleanTitle = Win32Api.GetWindowTitleClean(handle);
			var newWndTitle = $"[PID: {instance.ProcessId}] {cleanTitle}";

			if (!wndTitle.ToLower().Equals(newWndTitle.ToLower()))
			{
				User32Methods.SetWindowText(handle, newWndTitle);
			}

			Win32Api.SetWindowIcons(handle, icon);
		}

		private bool GetCurrentEventInstance(IntPtr windowHandle, out IProcessInstanceViewModel instance)
		{
			instance = _integratedInstances.SingleOrDefault(i => i.ProcessMainWindowHandle == windowHandle);

			return instance != null;
		}

		private void WinEventsHandler(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
		{
			IProcessInstanceViewModel instance;

			if (idObject != 0 || idChild != 0 || !_winEvents.Contains(eventType) || !GetCurrentEventInstance(hwnd, out instance))
			{
				return;
			}

			switch (eventType)
			{
				case EVENT_SYSTEM_FOREGROUND:
					{
						if (!Win32Api.IsOwnedWindow(hwnd))
						{
							TakeWindowOwnership(hwnd);
						}
					}
					break;

				case EVENT_OBJECT_NAMECHANGE:
				case EVENT_SYSTEM_MINIMIZEEND:
					{
						SetWindowTitleBar(instance);
					}
					break;

				case EVENT_SYSTEM_MINIMIZESTART:
					{
						instance.IsMinimized = true;
						User32Methods.ShowWindow(hwnd, ShowWindowCommands.SW_HIDE);
					}
					break;
			}
		}
	}
}
