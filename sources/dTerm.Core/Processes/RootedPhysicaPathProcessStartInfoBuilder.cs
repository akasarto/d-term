using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class RootedPhysicaPathProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		string _physicalPath;

		public RootedPhysicaPathProcessStartInfoBuilder(string physicalPath)
		{
			_physicalPath = physicalPath ?? throw new ArgumentNullException(nameof(physicalPath), nameof(RootedPhysicaPathProcessStartInfoBuilder));
		}

		public static implicit operator ProcessStartInfo(RootedPhysicaPathProcessStartInfoBuilder input)
		{
			var fileName = NormalizeFilename(input?._physicalPath);

			if (string.IsNullOrWhiteSpace(fileName))
			{
				return null;
			}

			var fileInfo = new FileInfo(fileName);

			return new ProcessStartInfo(fileInfo.FullName);
		}
	}
}
