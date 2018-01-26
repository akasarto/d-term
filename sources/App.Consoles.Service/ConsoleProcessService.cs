using App.Consoles.Core;
using App.Consoles.Service.PathBuilders;
using System;
using System.Diagnostics;

namespace App.Consoles.Service
{
	public class ConsoleProcessService : IConsoleProcessService
	{
		public IConsoleProcess Create(IProcessDescriptor descriptor)
		{
			var pathBuilder = GetPathBuilder(descriptor);

			if (pathBuilder == null)
			{
				throw new InvalidOperationException(nameof(Create), new ArgumentNullException(nameof(pathBuilder), nameof(ConsoleProcessService)));
			}

			var processStartInfo = new ProcessStartInfo(pathBuilder.Build())
			{
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = descriptor.Args
			};

			var consoleInstance = new ConsoleProcess(processStartInfo, descriptor.StartupTimeoutInSeconds);

			return consoleInstance;
		}

		private IPathBuilder GetPathBuilder(IProcessDescriptor descriptor)
		{
			switch (descriptor.PathType)
			{
				case PathType.Physical:
					return new PhysicalFilePathBuilder(descriptor.FilePath);
				case PathType.ProgramFilesFolder:
					return new ProgramFilesFolderPathBuilder(descriptor.FilePath);
				case PathType.System32Folder:
					return new System32FolderPathBuilder(descriptor.FilePath);
				case PathType.SystemPathVar:
					return new SystemPathVarPathBuilder(descriptor.FilePath);
			}

			return null;
		}
	}
}
