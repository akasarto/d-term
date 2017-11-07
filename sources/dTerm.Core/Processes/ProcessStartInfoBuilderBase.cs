using System.Diagnostics;

namespace dTerm.Core.Processes
{
	public abstract class ProcessStartInfoBuilderBase
	{
		internal abstract ProcessStartInfo GetProcessStartInfo();

		internal static string NormalizeFilename(string input)
		{
			input = (input ?? string.Empty).Trim('~', '.', '/', '\\');

			return input.Replace("/", "\\");
		}
	}
}
