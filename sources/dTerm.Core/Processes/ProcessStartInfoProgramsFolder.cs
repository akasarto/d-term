using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class ProcessStartInfoProgramsFolder
	{
		string _physicalPath;

		public ProcessStartInfoProgramsFolder(string physicalPath)
		{
			_physicalPath = physicalPath ?? throw new ArgumentNullException(nameof(physicalPath), nameof(ProcessStartInfoProgramsFolder));
		}

		public static implicit operator ProcessStartInfo(ProcessStartInfoProgramsFolder input)
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
