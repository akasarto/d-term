using System.ComponentModel.DataAnnotations;

namespace Processes.Core
{
	public enum ProcessType
	{
		[Display(Name = "None")]
		None = 0,

		[Display(Name = "Console")]
		Console = 1,

		[Display(Name = "Utility")]
		Utility = 2
	}
}
