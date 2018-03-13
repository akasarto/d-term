using ReactiveUI;
using UI.Wpf.Consoles;

namespace UI.Wpf
{
	public interface IAppState
	{
		IReactiveDerivedList<IConsoleInstanceViewModel> GetConsoleInstances();
		void RemoveConsoleInstance(IConsoleInstanceViewModel instance);
		void AddConsoleInstance(IConsoleInstanceViewModel instance);
	}

	public class AppState : IAppState
	{
		private readonly IReactiveList<IConsoleInstanceViewModel> _consoleInstancesSource;
		private readonly IReactiveDerivedList<IConsoleInstanceViewModel> _consoleInstancesList;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public AppState()
		{
			_consoleInstancesSource = new ReactiveList<IConsoleInstanceViewModel>() { ChangeTrackingEnabled = true };
			_consoleInstancesList = _consoleInstancesSource.CreateDerivedCollection(
				selector: instance => instance
			);
		}

		public IReactiveDerivedList<IConsoleInstanceViewModel> GetConsoleInstances() => _consoleInstancesList;

		public void AddConsoleInstance(IConsoleInstanceViewModel instance)
		{
			_consoleInstancesSource.Add(instance);
		}

		public void RemoveConsoleInstance(IConsoleInstanceViewModel instance)
		{
			_consoleInstancesSource.Remove(instance);
		}
	}
}
