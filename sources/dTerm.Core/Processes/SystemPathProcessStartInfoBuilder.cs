using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class SystemPathProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		string _environmentFilePath;

		public override ProcessStartInfo ProcessStartInfo { get; internal set; }

		public SystemPathProcessStartInfoBuilder(string environmentFilePath)
		{
			_environmentFilePath = environmentFilePath ?? throw new ArgumentNullException(nameof(environmentFilePath), nameof(SystemPathProcessStartInfoBuilder));


		}

		public static implicit operator ProcessStartInfo(SystemPathProcessStartInfoBuilder info) => info.ProcessStartInfo;

		private string BuildPath(SystemPathProcessStartInfoBuilder input)
		{
			var fileName = NormalizeFilename(input?._environmentFilePath);

			var rawPath = Environment.GetEnvironmentVariable("PATH");
			var availablePaths = rawPath?.Split(';') ?? new string[0];

			fileName = $"{fileName.TrimEnd(".exe".ToCharArray())}.exe";

			foreach (var path in availablePaths)
			{
				var fullPath = Path.Combine(path, fileName);

				if (File.Exists(fullPath))
					return fullPath;
			}

			return input._environmentFilePath;
		}
	}
}
