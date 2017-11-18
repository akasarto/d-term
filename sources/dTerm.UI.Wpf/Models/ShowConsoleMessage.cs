using dTerm.Core;
using dTerm.Core.Events;
using System;

namespace dTerm.UI.Wpf.Models
{
	public class ShowConsoleMessage : IEventMessage
	{
		private IConsoleInstance _consoleInstance;

		public ShowConsoleMessage(IConsoleInstance consoleInstance)
		{
			_consoleInstance = consoleInstance ?? throw new ArgumentNullException(nameof(consoleInstance), nameof(ShowConsoleMessage));
		}

		public IConsoleInstance ConsoleInstance => _consoleInstance;
	}
}
