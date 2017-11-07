using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class ProgramFilesFolderProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		string _physicalPath;

		public ProgramFilesFolderProcessStartInfoBuilder(string physicalPath)
		{
			_physicalPath = physicalPath ?? throw new ArgumentNullException(nameof(physicalPath), nameof(ProgramFilesFolderProcessStartInfoBuilder));
		}

		public static implicit operator ProcessStartInfo(ProgramFilesFolderProcessStartInfoBuilder input)
		{
			if (input == null || input._physicalPath == null)
			{
				return null;
			}

			var fileInfo = new FileInfo(input._physicalPath);

			return new ProcessStartInfo(fileInfo.FullName);
		}
	}
}
