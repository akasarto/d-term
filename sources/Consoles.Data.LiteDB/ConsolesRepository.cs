using Consoles.Core;
using System;
using System.Collections.Generic;

namespace Consoles.Data.LiteDB
{
	public class ConsolesRepository : IConsolesRepository
	{
		private readonly string _connectionString = null;

		public ConsolesRepository(string connectionString)
		{
			_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString), nameof(ConsolesRepository));
		}

		public List<ConsoleEntity> GetAll()
		{
			return new List<ConsoleEntity>();
		}
	}
}
