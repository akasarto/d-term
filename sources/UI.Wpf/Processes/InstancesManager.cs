using ReactiveUI;
using System;
using System.Reactive.Linq;
using UI.Wpf.Properties;

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

		/// <summary>
		/// Constructor method.
		/// </summary>
		public InstancesManager(IntPtr shellViewHandle)
		{
			_shellViewHandle = shellViewHandle;

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

			Win32Api.RemoveFromTaskbar(instance.ProcessMainWindowHandle);
			Win32Api.SetProcessWindowIcon(instance.ProcessMainWindowHandle, iconHandle);
			Win32Api.SetProcessWindowOwner(instance.ProcessMainWindowHandle, _shellViewHandle);
		}

		public void Release(IInstanceViewModel instance)
		{
		}
	}
}
