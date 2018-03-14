using ReactiveUI;
using System;
using System.Reactive.Linq;
using UI.Wpf.Processes;

namespace UI.Wpf
{
	public interface IAppState
	{
		void AddProcessInstance(IProcessInstanceViewModel instance);
		IReactiveDerivedList<IProcessInstanceViewModel> GetAllProcessInstances();
		IReactiveDerivedList<IProcessInstanceViewModel> GetMinimizedProcessInstances();
	}

	public class AppState : ReactiveObject, IAppState
	{
		private readonly IReactiveList<IProcessInstanceViewModel> _instancesSource;
		private readonly IReactiveDerivedList<IProcessInstanceViewModel> _allInstancesList;
		private readonly IReactiveDerivedList<IProcessInstanceViewModel> _minimizedInstancesList;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public AppState()
		{
			_instancesSource = new ReactiveList<IProcessInstanceViewModel>() { ChangeTrackingEnabled = true };
			_allInstancesList = _instancesSource.CreateDerivedCollection(
				selector: instance => instance
			);
			_minimizedInstancesList = _instancesSource.CreateDerivedCollection(
				filter: instance => instance.IsMinimized,
				selector: instance => instance
			);
		}

		public void AddProcessInstance(IProcessInstanceViewModel instance)
		{
			var instanceSubscription = instance.ProcessTerminated.ObserveOnDispatcher().Subscribe(@event =>
			{
				_instancesSource.Remove(instance);
			});

			_instancesSource.Add(instance);
		}

		public IReactiveDerivedList<IProcessInstanceViewModel> GetAllProcessInstances() => _allInstancesList;

		public IReactiveDerivedList<IProcessInstanceViewModel> GetMinimizedProcessInstances() => _minimizedInstancesList;

		public void Remove(IProcessInstanceViewModel instance)
		{
			_instancesSource.Remove(instance);
		}
	}
}
