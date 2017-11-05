using dTerm.Core.Entities;
using System.Diagnostics;

namespace dTerm.UI.Wpf.Models
{
	public interface IConsoleOption
	{
		ConsoleType ConsoleType { get; }

		string Description { get; }

		int DisplayOrder { get; set; }

		bool IsSupported { get; }

		ProcessStartInfo ProcessStartInfo { get; }
	}
}
