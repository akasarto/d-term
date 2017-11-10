using System.ComponentModel.DataAnnotations;

namespace dTerm.Core
{
	public enum ProcessStatus
	{
		[Display(Name = "Created")]
		Created = 0,

		[Display(Name = "Initialized")]
		Initialized = 1,

		[Display(Name = "Terminated")]
		Terminated = 2
	}
}
