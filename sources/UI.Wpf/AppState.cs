using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Security.Principal;
using UI.Wpf.Processes;

namespace UI.Wpf
{
	public interface IAppState
	{
		byte AlphaLevel { get; set; }
		bool HasAdministrationPrivileges { get; }
		void AddProcessInstance(IProcessInstanceViewModel instance);
		IReactiveList<IProcessInstanceViewModel> GetProcessInstances();
		IntPtr GetShellViewHandle();
	}

	public class AppState : ReactiveObject, IAppState
	{
		private readonly IntPtr _shellViewHandle;
		private readonly IReactiveList<IProcessInstanceViewModel> _processInstances;
		private byte _alphaLevel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public AppState(IntPtr shellViewHandle)
		{
			_shellViewHandle = shellViewHandle;

			_processInstances = new ReactiveList<IProcessInstanceViewModel>()
			{
				ChangeTrackingEnabled = true
			};
		}

		public byte AlphaLevel
		{
			get => _alphaLevel;
			set => this.RaiseAndSetIfChanged(ref _alphaLevel, value);
		}

		public bool HasAdministrationPrivileges
		{
			get
			{
				using (var identity = WindowsIdentity.GetCurrent())
				{
					var principal = new WindowsPrincipal(identity);
					return principal.IsInRole(WindowsBuiltInRole.Administrator);
				}
			}
		}

		public void AddProcessInstance(IProcessInstanceViewModel instance)
		{
			var subscription = instance.ProcessTerminated.ObserveOnDispatcher().Subscribe(@event =>
			{
				_processInstances.Remove(instance);
			});

			_processInstances.Add(instance);
		}

		public IReactiveList<IProcessInstanceViewModel> GetProcessInstances() => _processInstances;

		public IntPtr GetShellViewHandle() => _shellViewHandle;
	}
}
