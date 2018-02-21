using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionAddedMessage
	{
		public ConsoleOptionAddedMessage(ConsoleOption newConsoleOption)
		{
			NewConsoleOption = newConsoleOption ?? throw new ArgumentNullException(nameof(newConsoleOption), nameof(ConsoleOptionAddedMessage));
		}

		public ConsoleOption NewConsoleOption { get; private set; }
	}
}
