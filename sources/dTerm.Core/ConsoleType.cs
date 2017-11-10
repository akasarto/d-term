using System.ComponentModel.DataAnnotations;

namespace dTerm.Core
{
	public enum ConsoleType : byte
	{
		[Display(Name = "Command Prompt")]
		Cmd = 0,

		[Display(Name = "Git Bash")]
		GitBash = 1,

		[Display(Name = "Power Shell")]
		PowerShell = 2,

		[Display(Name = "Ubuntu Bash")]
		UbuntuBash = 3
	}
}
