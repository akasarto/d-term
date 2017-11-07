using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class ProgramFilesFolderProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		string _programFilesFolderRelativeFileName;

		public ProgramFilesFolderProcessStartInfoBuilder(string programFilesFolderRelativeFileName)
		{
			_programFilesFolderRelativeFileName = programFilesFolderRelativeFileName ?? throw new ArgumentNullException(nameof(programFilesFolderRelativeFileName), nameof(ProgramFilesFolderProcessStartInfoBuilder));
		}

		public static implicit operator ProcessStartInfo(ProgramFilesFolderProcessStartInfoBuilder builder) => builder.GetProcessStartInfo();

		internal override ProcessStartInfo GetProcessStartInfo()
		{
			var fileInfo = new FileInfo(_programFilesFolderRelativeFileName);

			return new ProcessStartInfo(fileInfo.FullName);
		}
	}
}
