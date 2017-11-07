using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class ProcessStartInfoPhysicalFile
	{
		string _physicalPath;

		public ProcessStartInfoPhysicalFile(string physicalPath)
		{
			_physicalPath = physicalPath ?? throw new ArgumentNullException(nameof(physicalPath), nameof(ProcessStartInfoPhysicalFile));
		}

		public static implicit operator ProcessStartInfo(ProcessStartInfoPhysicalFile input)
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
