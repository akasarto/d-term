﻿using Processes.Core;
using System;
using System.IO;

namespace Processes.Systems.Local
{
	public class ProcessPathBuilder : IProcessPathBuilder
	{
		public string Build(ProcessBasePath basePath, string executableName)
		{
			switch (basePath)
			{
				case ProcessBasePath.App:
					return AppFilePath(executableName);

				case ProcessBasePath.Physical:
					return PhysicalFilePath(executableName);

				case ProcessBasePath.ProgramFilesFolder:
					return ProgramFilesFolderPath(executableName);

				case ProcessBasePath.System32Folder:
					return System32FolderPath(executableName);

				case ProcessBasePath.SystemPathVar:
					return SystemPathVarPath(executableName);
			}

			return null;
		}

		private static string NormalizeDirectory(string folderPath) => (folderPath ?? string.Empty).Trim('~', '.', '/', '\\');

		private static string NormalizeFilename(string fileName)
		{
			fileName = (fileName ?? string.Empty).Trim('~', '.', '/', '\\');
			var extensionIndex = fileName.LastIndexOf(".exe");
			extensionIndex = extensionIndex >= 0 ? extensionIndex : fileName.Length;
			fileName = $"{fileName.Substring(0, extensionIndex)}.exe";
			return fileName.Replace("/", "\\");
		}

		private string AppFilePath(string appFolderRelativeFileName)
		{
			var normalizedFilename = NormalizeFilename(
				appFolderRelativeFileName
			);

			var appFolder = AppDomain.CurrentDomain.BaseDirectory;

			var physicalPath = Path.Combine(appFolder, normalizedFilename);

			if (File.Exists(physicalPath))
			{
				return physicalPath;
			}

			return null;
		}

		private string PhysicalFilePath(string rootedPhysicalFileName)
		{
			var normalizedFilename = NormalizeFilename(
				rootedPhysicalFileName
			);

			return new FileInfo(normalizedFilename).FullName;
		}

		private string ProgramFilesFolderPath(string programFilesFolderRelativeFileName)
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

		private string System32FolderPath(string system32FolderRelativeFileName)
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

		private string SystemPathVarPath(string environmentPathVarRelativeFileName)
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
