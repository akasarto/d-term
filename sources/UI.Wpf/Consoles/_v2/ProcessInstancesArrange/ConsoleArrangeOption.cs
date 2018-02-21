using System.ComponentModel.DataAnnotations;

namespace UI.Wpf.Consoles
{
	public enum ConsoleArrangeOption
	{
		[Display(Name = "Arrange as Grid")]
		Grid = 0,

		[Display(Name = "Arrange vertically")]
		Vertically = 1,

		[Display(Name = "Arrange horizontally")]
		Horizontally = 2
	}
}
