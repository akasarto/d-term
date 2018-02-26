using System;
using System.Collections.Generic;

namespace Consoles.Core
{
	public interface IConsoleOptionsRepository
	{
		ConsoleEntity Add(ConsoleEntity consoleOption);
		void Delete(Guid consoleOptionId);
		List<ConsoleEntity> GetAll();
		void Update(ConsoleEntity consoleOption);
	}
}
