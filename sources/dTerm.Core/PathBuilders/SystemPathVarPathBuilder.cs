using System;
using System.IO;

namespace dTerm.Core.PathBuilders
{
	public class SystemPathVarPathBuilder : PathBuilderBase, IPathBuilder
	{
		private string _environmentPathVarRelativeFileName;

		public SystemPathVarPathBuilder(string environmentPathVarRelativeFileName)
		{
			_environmentPathVarRelativeFileName = environmentPathVarRelativeFileName ?? throw new ArgumentNullException(nameof(environmentPathVarRelativeFileName), nameof(ProgramFilesFolderPathBuilder));
		}

		public string Build()
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
					return physicalPath;
				}
			}

			return null;
		}
	}
}
