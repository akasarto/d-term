using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class System32FolderProcessStartInfoBuilder : ProcessStartInfoBuilderBase
	{
		string _system32FilePath;

		public System32FolderProcessStartInfoBuilder(string system32FilePath)
		{
			_system32FilePath = system32FilePath ?? throw new ArgumentNullException(nameof(system32FilePath), nameof(System32FolderProcessStartInfoBuilder));
		}

		public static implicit operator ProcessStartInfo(System32FolderProcessStartInfoBuilder input)
		{
			if (input == null || input._system32FilePath == null)
			{
				return null;
			}

			//Environment.SpecialFolder.ProgramFiles

			//@"C:\WINDOWS\sysnative\bash.exe"

			var fileInfo = new FileInfo(input._system32FilePath);

			return new ProcessStartInfo(fileInfo.FullName);
		}
	}
}
