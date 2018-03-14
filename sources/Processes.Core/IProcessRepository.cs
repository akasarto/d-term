using System;
using System.Collections.Generic;

namespace Processes.Core
{
	public interface IProcessRepository
	{
		ProcessEntity Add(ProcessEntity entity);
		void Delete(Guid entityId);
		IEnumerable<ProcessEntity> GetAll();
		void Update(ProcessEntity entity);
	}
}
