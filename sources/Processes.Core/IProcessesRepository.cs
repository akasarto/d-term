using System;
using System.Collections.Generic;

namespace Processes.Core
{
	public interface IProcessesRepository
	{
		ProcessEntity Add(ProcessEntity consoleOption);
		void Delete(Guid consoleOptionId);
		List<ProcessEntity> GetAll();
		void Update(ProcessEntity consoleOption);
	}
}
