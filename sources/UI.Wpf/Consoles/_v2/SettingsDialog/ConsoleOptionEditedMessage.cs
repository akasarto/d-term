using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionEditedMessage
	{
		public ConsoleOptionEditedMessage(ConsoleOption newConsoleOption, ConsoleOption oldConsoleOption)
		{
			NewConsoleOption = newConsoleOption ?? throw new ArgumentNullException(nameof(newConsoleOption), nameof(ConsoleOptionEditedMessage));
			OldConsoleOption = oldConsoleOption ?? throw new ArgumentNullException(nameof(oldConsoleOption), nameof(ConsoleOptionEditedMessage));
		}

		public ConsoleOption NewConsoleOption { get; private set; }

		public ConsoleOption OldConsoleOption { get; private set; }
	}
}
