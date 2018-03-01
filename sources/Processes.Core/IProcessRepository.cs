using System;
using System.Collections.Generic;

namespace Processes.Core
{
	public interface IProcessRepository
	{
		ProcessEntity Add(ProcessEntity entity);
		void Delete(Guid entityId);
		List<ProcessEntity> GetAll();
		void Update(ProcessEntity entity);
	}
}
