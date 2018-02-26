using ReactiveUI;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console configs screen interface.
	/// </summary>
	public interface IConsoleConfigsScreen : IScreen
	{
	}

	/// <summary>
	/// App console configs screen implementation.
	/// </summary>
	public class ConsoleConfigsScreen : ReactiveObject, IConsoleConfigsScreen
	{
		//
		private readonly RoutingState _routingState;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleConfigsScreen(RoutingState routingState = null)
		{
			_routingState = _routingState ?? new RoutingState();
		}

		/// <summary>
		/// Gets the shell screen router instance.
		/// </summary>
		public RoutingState Router => _routingState;
	}
}
