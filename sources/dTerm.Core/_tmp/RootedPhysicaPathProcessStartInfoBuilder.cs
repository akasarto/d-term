using System;
using System.Diagnostics;

namespace dTerm.Core
{
	public class RootedPhysicaPathProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		string _rootedPhysicalFileName;

		public RootedPhysicaPathProcessStartInfoBuilder(string rootedPhysicalFileName)
		{
			_rootedPhysicalFileName = rootedPhysicalFileName ?? throw new ArgumentNullException(nameof(rootedPhysicalFileName), nameof(RootedPhysicaPathProcessStartInfoBuilder));
		}

		public static implicit operator ProcessStartInfo(RootedPhysicaPathProcessStartInfoBuilder builder) => builder.GetProcessStartInfo();

		internal override ProcessStartInfo GetProcessStartInfo()
		{
			var normalizedFilename = NormalizeFilename(
				_rootedPhysicalFileName
			);

			return new ProcessStartInfo(normalizedFilename);
		}
	}
}
