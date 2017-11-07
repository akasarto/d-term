using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class ProcessStartInfoPhysicalPath
	{
		string _physicalPath;

		public ProcessStartInfoPhysicalPath(string physicalPath)
		{
			_physicalPath = physicalPath ?? throw new ArgumentNullException(nameof(physicalPath), nameof(ProcessStartInfoPhysicalPath));
		}

		public static implicit operator ProcessStartInfo(ProcessStartInfoPhysicalPath input)
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
