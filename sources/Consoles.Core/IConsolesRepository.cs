using System.Collections.Generic;

namespace Consoles.Core
{
	public interface IConsolesRepository
	{
		List<Console> GetAll();
	}
}
