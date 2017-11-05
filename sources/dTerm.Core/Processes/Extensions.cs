using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public static class ProcessExtensions
	{
		public static bool PathExists(this ProcessStartInfo @this)
		{
			if (@this == null)
			{
				return false;
			}

			var fileName = @this.FileName;

			if (File.Exists(fileName))
			{
				return true;
			}

			var rawPath = Environment.GetEnvironmentVariable("PATH");
			var availablePaths = rawPath?.Split(';') ?? new string[0];

			fileName = $"{fileName.TrimEnd(".exe".ToCharArray())}.exe";

			foreach (var path in availablePaths)
			{
				var fullPath = Path.Combine(path, fileName);

				if (File.Exists(fullPath))
					return true;
			}

			return false;
		}
	}
}
