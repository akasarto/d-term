using System;
using System.Collections.Generic;

namespace Consoles.Core
{
	public interface IConsoleOptionsRepository
	{
		ConsoleOption Add(ConsoleOption consoleOption);
		void Delete(Guid consoleOptionId);
		List<ConsoleOption> GetAll();
		void Update(ConsoleOption consoleOption);
	}
}
