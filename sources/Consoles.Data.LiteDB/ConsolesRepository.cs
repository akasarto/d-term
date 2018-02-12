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
			return new List<ConsoleEntity>()
			{
				new ConsoleEntity() {
					Id = Guid.NewGuid(),
					Index = 1,
					Name = "Command Prompt",
					ProcessPath = @"/cmd.exe",
					ProcessPathArgs = string.Empty,
					ProcessPathType = PathType.SystemPathVar,
					UTCCreation = DateTime.UtcNow
				},

				new ConsoleEntity() {
					Id = Guid.NewGuid(),
					Index = 2,
					Name = "Git Bash",
					ProcessPath = @"/git/bin/sh.exe",
					ProcessPathArgs = "--login -i",
					ProcessPathType = PathType.ProgramFilesFolder,
					UTCCreation = DateTime.UtcNow
				},

				new ConsoleEntity() {
					Id = Guid.NewGuid(),
					Index = 3,
					Name = "PowerShell",
					ProcessPath = @"/powershell.exe",
					ProcessPathArgs = string.Empty,
					ProcessPathType = PathType.SystemPathVar,
					UTCCreation = DateTime.UtcNow
				},

				new ConsoleEntity() {
					Id = Guid.NewGuid(),
					Index = 4,
					Name = "Ubuntu Bash",
					ProcessPath = @"/bash.exe",
					ProcessPathArgs = string.Empty,
					ProcessPathType = PathType.System32Folder,
					UTCCreation = DateTime.UtcNow
				}
			};
		}
	}
}
