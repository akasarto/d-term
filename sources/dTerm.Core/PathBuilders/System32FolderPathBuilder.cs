using System;
using System.IO;

namespace dTerm.Core.PathBuilders
{
	public class System32FolderPathBuilder : PathBuilderBase, IPathBuilder
	{
		private string _system32FolderRelativeFileName;

		public System32FolderPathBuilder(string system32FolderRelativeFileName)
		{
			_system32FolderRelativeFileName = system32FolderRelativeFileName ?? throw new ArgumentNullException(nameof(system32FolderRelativeFileName), nameof(ProgramFilesFolderPathBuilder));
		}

		public string Build()
		{
			var normalizedFilename = NormalizeFilename(
				_system32FolderRelativeFileName
			);

			var folders = new string[] {
				Environment.GetFolderPath(Environment.SpecialFolder.System),
				Environment.GetFolderPath(Environment.SpecialFolder.SystemX86),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Sysnative"),
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
