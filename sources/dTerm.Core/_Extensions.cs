using System.Diagnostics;
using System.IO;

namespace dTerm.Core
{
	public static class _Extensions
	{
		public static bool PathExists(this ProcessStartInfo @this) => File.Exists(@this?.FileName);
	}
}
