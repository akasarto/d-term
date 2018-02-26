using System.ComponentModel.DataAnnotations;

namespace Processes.Core
{
	public enum ProcessBasePath : byte
	{
		[Display(Name = "Physical Path")]
		Physical = 0,

		[Display(Name = "Program Files Folder")]
		ProgramFilesFolder = 1,

		[Display(Name = "System32 Folder")]
		System32Folder = 2,

		[Display(Name = "System Path VARS Folder")]
		SystemPathVar = 3
	}
}
