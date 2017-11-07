using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public static class ProcessExtensions
	{
		public static bool PathExists(this ProcessStartInfo @this) => File.Exists(@this?.FileName);
	}
}
