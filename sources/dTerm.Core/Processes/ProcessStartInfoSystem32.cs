using System;
using System.Diagnostics;
using System.IO;

namespace dTerm.Core.Processes
{
	public class ProcessStartInfoSystem32
	{
		string _system32FilePath;

		public ProcessStartInfoSystem32(string system32FilePath)
		{
			_system32FilePath = system32FilePath ?? throw new ArgumentNullException(nameof(system32FilePath), nameof(ProcessStartInfoSystem32));
		}

		public static implicit operator ProcessStartInfo(ProcessStartInfoSystem32 input)
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
