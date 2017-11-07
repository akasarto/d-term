using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class System32FolderProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		string _system32FolderRelativeFileName;

		public System32FolderProcessStartInfoBuilder(string system32FolderRelativeFileName)
		{
			_system32FolderRelativeFileName = system32FolderRelativeFileName ?? throw new ArgumentNullException(nameof(system32FolderRelativeFileName), nameof(System32FolderProcessStartInfoBuilder));
		}

		public static implicit operator ProcessStartInfo(System32FolderProcessStartInfoBuilder builder) => builder.GetProcessStartInfo();

		internal override ProcessStartInfo GetProcessStartInfo()
		{
			//Environment.SpecialFolder.ProgramFiles

			//@"C:\WINDOWS\sysnative\bash.exe"

			var fileInfo = new FileInfo(_system32FolderRelativeFileName);

			return new ProcessStartInfo(fileInfo.FullName);
		}
	}
}
