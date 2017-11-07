using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class ProcessStartInfoEnvFile
	{
		string _environmentFilePath;

		public ProcessStartInfoEnvFile(string environmentFilePath)
		{
			_environmentFilePath = environmentFilePath ?? throw new ArgumentNullException(nameof(environmentFilePath), nameof(ProcessStartInfoEnvFile));
		}

		public static implicit operator ProcessStartInfo(ProcessStartInfoEnvFile input)
		{
			if (input == null || input._environmentFilePath == null)
			{
				return null;
			}

			var path = BuildPath(input);

			return new ProcessStartInfo(path);
		}

		private static string BuildPath(ProcessStartInfoEnvFile input)
		{
			var fileName = input?._environmentFilePath ?? string.Empty;

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
