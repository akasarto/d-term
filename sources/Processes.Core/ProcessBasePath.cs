using System.ComponentModel.DataAnnotations;

namespace Processes.Core
{
	public enum ProcessBasePath : byte
	{
		[Display(Name = "None")]
		None = 0,

		[Display(Name = "Application")]
		App = 1,

		[Display(Name = "Physical Path")]
		Physical = 2,

		[Display(Name = "Program Files Folder")]
		ProgramFilesFolder = 3,

		[Display(Name = "System32 Folder")]
		System32Folder = 4,

		[Display(Name = "System Path VARS Folder")]
		SystemPathVar = 5
	}
}
