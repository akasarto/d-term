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
					Name = "Command Prompt",
					OrderIndex = 1,
					PicturePath = "M20,19V7H4V19H20M20,3A2,2 0 0,1 22,5V19A2,2 0 0,1 20,21H4A2,2 0 0,1 2,19V5C2,3.89 2.9,3 4,3H20M13,17V15H18V17H13M9.58,13L5.57,9H8.4L11.7,12.3C12.09,12.69 12.09,13.33 11.7,13.72L8.42,17H5.59L9.58,13Z",
					ProcessBasePath = BasePath.SystemPathVar,
					ProcessExecutableName = @"~/cmd.exe",
					ProcessStartupArgs = string.Empty,
					UTCCreation = DateTime.UtcNow
				},

				new ConsoleEntity() {
					Id = Guid.NewGuid(),
					Name = "Git Bash",
					OrderIndex = 2,
					PicturePath = "M2.6,10.59L8.38,4.8L10.07,6.5C9.83,7.35 10.22,8.28 11,8.73V14.27C10.4,14.61 10,15.26 10,16A2,2 0 0,0 12,18A2,2 0 0,0 14,16C14,15.26 13.6,14.61 13,14.27V9.41L15.07,11.5C15,11.65 15,11.82 15,12A2,2 0 0,0 17,14A2,2 0 0,0 19,12A2,2 0 0,0 17,10C16.82,10 16.65,10 16.5,10.07L13.93,7.5C14.19,6.57 13.71,5.55 12.78,5.16C12.35,5 11.9,4.96 11.5,5.07L9.8,3.38L10.59,2.6C11.37,1.81 12.63,1.81 13.41,2.6L21.4,10.59C22.19,11.37 22.19,12.63 21.4,13.41L13.41,21.4C12.63,22.19 11.37,22.19 10.59,21.4L2.6,13.41C1.81,12.63 1.81,11.37 2.6,10.59Z",
					ProcessBasePath = BasePath.ProgramFilesFolder,
					ProcessExecutableName = @"~/git/bin/sh.exe",
					ProcessStartupArgs = "--login -i",
					UTCCreation = DateTime.UtcNow
				},

				new ConsoleEntity() {
					Id = Guid.NewGuid(),
					Name = "PowerShell",
					OrderIndex = 3,
					PicturePath = "M 9.0566406 2 C 8.6047931 2.0075009 8.1538206 2.1773437 7.7988281 2.5117188 C 7.0407047 3.2242179 7.0042935 4.4157022 7.7167969 5.1738281 L 14.097656 11.958984 L 4.7480469 19.136719 C 4.048666 19.673595 3.9162491 20.675003 4.453125 21.375 C 4.9900001 22.074376 5.9939785 22.206797 6.6933594 21.669922 L 17.59375 13.300781 C 17.790627 13.149533 17.942508 12.961406 18.046875 12.753906 L 18.072266 12.699219 L 18.101562 12.650391 C 18.496533 11.951645 18.413353 11.049843 17.833984 10.433594 L 10.460938 2.59375 C 10.08218 2.19125 9.5685298 1.99125 9.0566406 2 z M 12.216797 18.851562 C 11.386802 18.851562 10.712891 19.524218 10.712891 20.355469 C 10.712891 21.186091 11.386802 21.857422 12.216797 21.857422 L 18.373047 21.857422 C 19.203651 21.857422 19.876953 21.186091 19.876953 20.355469 C 19.876953 19.524218 19.203651 18.851562 18.373047 18.851562 L 12.216797 18.851562 z",
					ProcessBasePath = BasePath.SystemPathVar,
					ProcessExecutableName = @"~/powershell.exe",
					ProcessStartupArgs = string.Empty,
					UTCCreation = DateTime.UtcNow
				},

				new ConsoleEntity() {
					Id = Guid.NewGuid(),
					Name = "Ubuntu Bash",
					OrderIndex = 4,
					PicturePath = "M22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2A10,10 0 0,1 22,12M14.34,7.74C14.92,8.07 15.65,7.87 16,7.3C16.31,6.73 16.12,6 15.54,5.66C14.97,5.33 14.23,5.5 13.9,6.1C13.57,6.67 13.77,7.41 14.34,7.74M11.88,15.5C11.35,15.5 10.85,15.39 10.41,15.18L9.57,16.68C10.27,17 11.05,17.22 11.88,17.22C12.37,17.22 12.83,17.15 13.28,17.03C13.36,16.54 13.64,16.1 14.1,15.84C14.56,15.57 15.08,15.55 15.54,15.72C16.43,14.85 17,13.66 17.09,12.33L15.38,12.31C15.22,14.1 13.72,15.5 11.88,15.5M11.88,8.5C13.72,8.5 15.22,9.89 15.38,11.69L17.09,11.66C17,10.34 16.43,9.15 15.54,8.28C15.08,8.45 14.55,8.42 14.1,8.16C13.64,7.9 13.36,7.45 13.28,6.97C12.83,6.85 12.37,6.78 11.88,6.78C11.05,6.78 10.27,6.97 9.57,7.32L10.41,8.82C10.85,8.61 11.35,8.5 11.88,8.5M8.37,12C8.37,10.81 8.96,9.76 9.86,9.13L9,7.65C7.94,8.36 7.15,9.43 6.83,10.69C7.21,11 7.45,11.47 7.45,12C7.45,12.53 7.21,13 6.83,13.31C7.15,14.56 7.94,15.64 9,16.34L9.86,14.87C8.96,14.24 8.37,13.19 8.37,12M14.34,16.26C13.77,16.59 13.57,17.32 13.9,17.9C14.23,18.47 14.97,18.67 15.54,18.34C16.12,18 16.31,17.27 16,16.7C15.65,16.12 14.92,15.93 14.34,16.26M5.76,10.8C5.1,10.8 4.56,11.34 4.56,12C4.56,12.66 5.1,13.2 5.76,13.2C6.43,13.2 6.96,12.66 6.96,12C6.96,11.34 6.43,10.8 5.76,10.8Z",
					ProcessBasePath = BasePath.System32Folder,
					ProcessExecutableName = @"~/bash.exe",
					ProcessStartupArgs = string.Empty,
					UTCCreation = DateTime.UtcNow
				}
			};
		}
	}
}
