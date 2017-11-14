using dTerm.Core;
using dTerm.Core.Events;
using System;

namespace dTerm.UI.Wpf.EventMessages
{
	public class HideConsoleMessage : IEventMessage
	{
		private IConsoleInstance _consoleInstance;

		public HideConsoleMessage(IConsoleInstance consoleInstance)
		{
			_consoleInstance = consoleInstance ?? throw new ArgumentNullException(nameof(consoleInstance), nameof(HideConsoleMessage));
		}

		public IConsoleInstance ConsoleInstance => _consoleInstance;
	}
}
