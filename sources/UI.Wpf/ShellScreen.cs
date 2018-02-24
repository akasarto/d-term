using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Wpf
{
	/// <summary>
	/// 
	/// </summary>
	public interface IShellScreen : IScreen
	{
	}

	/// <summary>
	/// Shell
	/// </summary>
	public class ShellScreen : ReactiveObject, IShellScreen
	{
		private readonly RoutingState _routingState;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellScreen(RoutingState routingState = null)
		{
			_routingState = _routingState ?? new RoutingState();
		}

		/// <summary>
		/// Gets the shell screen router instance.
		/// </summary>
		public RoutingState Router => _routingState;
	}
}
