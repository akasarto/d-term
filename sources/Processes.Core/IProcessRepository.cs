using System;
using System.Collections.Generic;

namespace Processes.Core
{
	public interface IProcessRepository
	{
		ProcessEntity Add(ProcessEntity entity);
		void Delete(Guid entityId);
		IEnumerable<ProcessEntity> GetAll();
		IEnumerable<ProcessEntity> GetAllConsoles();
		IEnumerable<ProcessEntity> GetAllUtilities();
		void Update(ProcessEntity entity);
	}
}
