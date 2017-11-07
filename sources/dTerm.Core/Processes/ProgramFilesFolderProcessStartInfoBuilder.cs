using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class ProgramFilesFolderProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		string _programFilesFolderRelativeFileName;

		public ProgramFilesFolderProcessStartInfoBuilder(string programFilesFolderRelativeFileName)
		{
			_programFilesFolderRelativeFileName = programFilesFolderRelativeFileName ?? throw new ArgumentNullException(nameof(programFilesFolderRelativeFileName), nameof(ProgramFilesFolderProcessStartInfoBuilder));
		}

		public static implicit operator ProcessStartInfo(ProgramFilesFolderProcessStartInfoBuilder builder) => builder.GetProcessStartInfo();

		internal override ProcessStartInfo GetProcessStartInfo()
		{
			var normalizedFilename = NormalizeFilename(
				_programFilesFolderRelativeFileName
			);

			var folders = new string[] {
				NormalizeFolderPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace("(x86)",string.Empty)).Trim(), // [REVIEW] Force x64 path.
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
			};

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
