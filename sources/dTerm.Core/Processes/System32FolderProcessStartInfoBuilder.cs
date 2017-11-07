using System;
using System.Diagnostics;
using System.IO;

// https://msdn.microsoft.com/en-us/library/windows/desktop/aa384187(v=vs.85).aspx

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
			var normalizedFilename = NormalizeFilename(
				_system32FolderRelativeFileName
			);

			var folders = new string[] {
				Environment.GetFolderPath(Environment.SpecialFolder.System),
				Environment.GetFolderPath(Environment.SpecialFolder.SystemX86),
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Sysnative"),
			};

			foreach (var folder in folders)
			{
				var physicalPath = Path.Combine(folder, normalizedFilename);

				if (File.Exists(physicalPath))
				{
					return new ProcessStartInfo(physicalPath);
				}
			}

			return null;
		}
	}
}
