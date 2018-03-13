using ReactiveUI;
using UI.Wpf.Processes;

namespace UI.Wpf
{
	public interface IAppState
	{
		IReactiveDerivedList<IProcessInstanceModel> GetConsoleInstances();
		void RemoveConsoleInstance(IProcessInstanceModel instance);
		void AddConsoleInstance(IProcessInstanceModel instance);
	}

	public class AppState : ReactiveObject, IAppState
	{
		private readonly IReactiveList<IProcessInstanceModel> _consoleInstancesSource;
		private readonly IReactiveDerivedList<IProcessInstanceModel> _consoleInstancesList;
		private readonly IReactiveDerivedList<IProcessInstanceModel> _minimizedconsoleInstancesList;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public AppState()
		{
			_consoleInstancesSource = new ReactiveList<IProcessInstanceModel>() { ChangeTrackingEnabled = true };
			_consoleInstancesList = _consoleInstancesSource.CreateDerivedCollection(
				selector: instance => instance
			);
		}

		public IReactiveDerivedList<IProcessInstanceModel> GetConsoleInstances() => _consoleInstancesList;

		public void AddConsoleInstance(IProcessInstanceModel instance)
		{
			_consoleInstancesSource.Add(instance);
		}

		public void RemoveConsoleInstance(IProcessInstanceModel instance)
		{
			_consoleInstancesSource.Remove(instance);
		}
	}
}
