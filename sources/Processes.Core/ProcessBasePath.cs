using System.ComponentModel.DataAnnotations;

namespace Processes.Core
{
	public enum ProcessBasePath : byte
	{
		[Display(Name = "Application")]
		App = 0,

		[Display(Name = "Physical Path")]
		Physical = 1,

		[Display(Name = "Program Files Folder")]
		ProgramFilesFolder = 2,

		[Display(Name = "System32 Folder")]
		System32Folder = 3,

		[Display(Name = "System Path VARS Folder")]
		SystemPathVar = 4
	}
}
