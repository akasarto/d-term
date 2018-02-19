using Consoles.Core;
using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionDeletedMessage
	{
		public ConsoleOptionDeletedMessage(ConsoleOption deletedConsoleOption)
		{
			DeletedConsoleOption  = deletedConsoleOption ?? throw new ArgumentNullException(nameof(deletedConsoleOption), nameof(ConsoleOptionDeletedMessage));
		}

		public ConsoleOption DeletedConsoleOption { get; private set; }
	}
}
