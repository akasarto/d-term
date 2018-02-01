using System;
using System.IO;

namespace dTerm.Consoles.Processes.PathBuilders
{
	public class PhysicalFilePathBuilder : PathBuilderBase, IPathBuilder
	{
		private string _rootedPhysicalFileName;

		public PhysicalFilePathBuilder(string rootedPhysicalFileName)
		{
			_rootedPhysicalFileName = rootedPhysicalFileName ?? throw new ArgumentNullException(nameof(rootedPhysicalFileName), nameof(ProgramFilesFolderPathBuilder));
		}

		public string Build()
		{
			var normalizedFilename = NormalizeFilename(
				_rootedPhysicalFileName
			);

			return new FileInfo(normalizedFilename).FullName;
		}
	}
}
