using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core
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
			var normalizedFilename = NormalizeFilename(
				_environmentPathVarRelativeFileName
			);

			var pathFolders = Environment.GetEnvironmentVariable("PATH");
			var folders = pathFolders?.Split(';') ?? new string[0];

			foreach (var folder in folders)
			{
				var physicalPath = Path.Combine(folder, normalizedFilename);

				if (File.Exists(physicalPath))
				{
					return new ProcessStartInfo(physicalPath);
				}
			}

			return null;
		}
	}
}
