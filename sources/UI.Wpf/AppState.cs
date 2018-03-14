using ReactiveUI;
using Splat;
using System;
using System.Reactive.Linq;
using UI.Wpf.Processes;

namespace UI.Wpf
{
	public interface IAppState
	{
		void AddProcessInstance(IInstanceViewModel instance);
		IReactiveDerivedList<IInstanceViewModel> GetAllProcessInstances();
		IReactiveDerivedList<IInstanceViewModel> GetMinimizedProcessInstances();
	}

	public class AppState : ReactiveObject, IAppState
	{
		private readonly IInstanceInteropManager _instanceInteropManager;
		private readonly IReactiveList<IInstanceViewModel> _instancesSource;
		private readonly IReactiveDerivedList<IInstanceViewModel> _allInstancesList;
		private readonly IReactiveDerivedList<IInstanceViewModel> _minimizedInstancesList;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public AppState(IInstanceInteropManager instanceInteropManager = null)
		{
			_instanceInteropManager = instanceInteropManager ?? Locator.CurrentMutable.GetService<IInstanceInteropManager>();

			_instancesSource = new ReactiveList<IInstanceViewModel>() { ChangeTrackingEnabled = true };

			_instancesSource.ItemsRemoved.Subscribe(item => _instanceInteropManager.Release(item));
			_instancesSource.ItemsAdded.Subscribe(item => _instanceInteropManager.Track(item));

			_allInstancesList = _instancesSource.CreateDerivedCollection(
				selector: instance => instance
			);

			_minimizedInstancesList = _instancesSource.CreateDerivedCollection(
				filter: instance => instance.IsMinimized,
				selector: instance => instance
			);
		}

		public void AddProcessInstance(IInstanceViewModel instance)
		{
			var instanceSubscription = instance.ProcessTerminated.ObserveOnDispatcher().Subscribe(@event =>
			{
				_instancesSource.Remove(instance);
			});

			_instancesSource.Add(instance);
		}

		public IReactiveDerivedList<IInstanceViewModel> GetAllProcessInstances() => _allInstancesList;

		public IReactiveDerivedList<IInstanceViewModel> GetMinimizedProcessInstances() => _minimizedInstancesList;

		public void Remove(IInstanceViewModel instance)
		{
			_instancesSource.Remove(instance);
		}
	}
}
