using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class SystemPathProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		private string _environmentPathVarRelativeFileName;

		public SystemPathProcessStartInfoBuilder(string environmentPathVarRelativeFileName)
		{
			_environmentPathVarRelativeFileName = environmentPathVarRelativeFileName ?? throw new ArgumentNullException(nameof(environmentPathVarRelativeFileName), nameof(SystemPathProcessStartInfoBuilder));
		}

		public static implicit operator ProcessStartInfo(SystemPathProcessStartInfoBuilder builder) => builder.GetProcessStartInfo();

		internal override ProcessStartInfo GetProcessStartInfo()
		{
			var procPath = string.Empty;
			var fileName = NormalizeFilename(_environmentPathVarRelativeFileName);

			var rawPath = Environment.GetEnvironmentVariable("PATH");
			var availablePaths = rawPath?.Split(';') ?? new string[0];

			fileName = $"{fileName.TrimEnd(".exe".ToCharArray())}.exe";

			foreach (var path in availablePaths)
			{
				var fullPath = Path.Combine(path, fileName);

				if (File.Exists(fullPath))
				{
					procPath = fullPath;
					break;
				}
			}

			return new ProcessStartInfo(procPath);
		}
	}
}
