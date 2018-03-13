using ReactiveUI;
using UI.Wpf.Processes;

namespace UI.Wpf
{
	public interface IAppState
	{
		void Add(IInstanceViewModel instance);
		IReactiveDerivedList<IInstanceViewModel> GetAll();
		IReactiveDerivedList<IInstanceViewModel> GetMinimized();
		void Remove(IInstanceViewModel instance);
	}

	public class AppState : ReactiveObject, IAppState
	{
		private readonly IReactiveList<IInstanceViewModel> _instancesSource;
		private readonly IReactiveDerivedList<IInstanceViewModel> _allInstancesList;
		private readonly IReactiveDerivedList<IInstanceViewModel> _minimizedInstancesList;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public AppState()
		{
			_instancesSource = new ReactiveList<IInstanceViewModel>() { ChangeTrackingEnabled = true };
			_allInstancesList = _instancesSource.CreateDerivedCollection(
				selector: instance => instance
			);
			_minimizedInstancesList = _instancesSource.CreateDerivedCollection(
				filter: instance => instance.IsMinimized,
				selector: instance => instance
			);
		}

		public void Add(IInstanceViewModel instance)
		{
			_instancesSource.Add(instance);
		}

		public IReactiveDerivedList<IInstanceViewModel> GetAll() => _allInstancesList;

		public IReactiveDerivedList<IInstanceViewModel> GetMinimized() => _minimizedInstancesList;

		public void Remove(IInstanceViewModel instance)
		{
			_instancesSource.Remove(instance);
		}
	}
}
