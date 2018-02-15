using System;
using System.IO;

namespace Consoles.Process.PathBuilders
{
	public class ProgramFilesFolderPathBuilder : PathBuilderBase, IPathBuilder
	{
		private string _programFilesFolderRelativeFileName;

		public ProgramFilesFolderPathBuilder(string programFilesFolderRelativeFileName)
		{
			_programFilesFolderRelativeFileName = programFilesFolderRelativeFileName ?? throw new ArgumentNullException(nameof(programFilesFolderRelativeFileName), nameof(ProgramFilesFolderPathBuilder));
		}

		public string Build()
		{
			var normalizedFilename = NormalizeFilename(
				_programFilesFolderRelativeFileName
			);

			var folders = new string[] {
				NormalizeDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace("(x86)",string.Empty)).Trim(),
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
			};

			foreach (var folder in folders)
			{
				var physicalPath = Path.Combine(folder, normalizedFilename);

				if (File.Exists(physicalPath))
				{
					return physicalPath;
				}
			}

			return null;
		}
	}
}
