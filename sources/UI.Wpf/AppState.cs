using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Security.Principal;
using UI.Wpf.Processes;

namespace UI.Wpf
{
	public interface IAppState
	{
		void AddProcessInstance(IProcessInstanceViewModel instance);
		IReactiveList<IProcessInstanceViewModel> GetProcessInstances();
		IntPtr GetShellViewHandle();
		bool HasAdminPrivileges();
	}

	public class AppState : ReactiveObject, IAppState
	{
		private readonly IntPtr _shellViewHandle;
		private readonly IReactiveList<IProcessInstanceViewModel> _processInstances;

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

		public bool HasAdminPrivileges()
		{
			using (var identity = WindowsIdentity.GetCurrent())
			{
				var principal = new WindowsPrincipal(identity);
				return principal.IsInRole(WindowsBuiltInRole.Administrator);
			}
		}
	}
}
