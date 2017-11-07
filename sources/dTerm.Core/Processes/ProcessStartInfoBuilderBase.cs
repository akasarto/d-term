using System.Diagnostics;

namespace dTerm.Core.Processes
{
	public abstract class ProcessStartInfoBuilderBase
	{
		public abstract ProcessStartInfo ProcessStartInfo { get; internal set; }

		internal static string NormalizeFilename(string input)
		{
			input = (input ?? string.Empty).Trim('~', '.', '/', '\\');

			return input.Replace("/", "\\");
		}
	}
}
