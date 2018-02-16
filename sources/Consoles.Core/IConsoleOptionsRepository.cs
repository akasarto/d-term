using System.Collections.Generic;

namespace Consoles.Core
{
	public interface IConsoleOptionsRepository
	{
		List<ConsoleOption> GetAll();
	}
}
