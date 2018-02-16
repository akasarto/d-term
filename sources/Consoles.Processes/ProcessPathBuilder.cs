using Consoles.Core;
using System;
using System.IO;

namespace Consoles.Processes
{
	public class ProcessPathBuilder : IProcessPathBuilder
	{
		public string Build(ConsoleEntity console)
		{
			var pathBuilder = console.ProcessBasePath;
			var pathExeFilename = console.ProcessExecutableName;

			switch (pathBuilder)
			{
				case BasePath.Physical:
					return PhysicalFilePath(pathExeFilename);
				case BasePath.ProgramFilesFolder:
					return ProgramFilesFolderPath(pathExeFilename);
				case BasePath.System32Folder:
					return System32FolderPath(pathExeFilename);
				case BasePath.SystemPathVar:
					return SystemPathVarPath(pathExeFilename);
			}

			return null;
		}

		private static string NormalizeDirectory(string folderPath) => (folderPath ?? string.Empty).Trim('~', '.', '/', '\\');

		private static string NormalizeFilename(string fileName)
		{
			fileName = (fileName ?? string.Empty).Trim('~', '.', '/', '\\');

			fileName = $"{fileName.TrimEnd(".exe".ToCharArray())}.exe";

			return fileName.Replace("/", "\\");
		}

		public string PhysicalFilePath(string rootedPhysicalFileName)
		{
			var normalizedFilename = NormalizeFilename(
				rootedPhysicalFileName
			);

			return new FileInfo(normalizedFilename).FullName;
		}


		public string ProgramFilesFolderPath(string programFilesFolderRelativeFileName)
		{
			var normalizedFilename = NormalizeFilename(
				programFilesFolderRelativeFileName
			);

			var folders = new string[] {
				NormalizeDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace("(x86)",string.Empty)).Trim(),
				Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
			};

			foreach (var folder in folders)
			{
				var physicalPath = Path.Combine(folder, normalizedFilename);

				if (File.Exists(physicalPath))
				{
					return physicalPath;
				}
			}

			return null;
		}

		public string System32FolderPath(string system32FolderRelativeFileName)
		{
			var normalizedFilename = NormalizeFilename(
				system32FolderRelativeFileName
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
					return physicalPath;
				}
			}

			return null;
		}

		public string SystemPathVarPath(string environmentPathVarRelativeFileName)
		{
			var normalizedFilename = NormalizeFilename(
				environmentPathVarRelativeFileName
			);

			var pathFolders = Environment.GetEnvironmentVariable("PATH");
			var folders = pathFolders?.Split(';') ?? new string[0];

			foreach (var folder in folders)
			{
				var physicalPath = Path.Combine(folder, normalizedFilename);

				if (File.Exists(physicalPath))
				{
					return physicalPath;
				}
			}

			return null;
		}
	}
}
