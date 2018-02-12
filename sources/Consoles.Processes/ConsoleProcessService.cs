using Consoles.Core;
using Consoles.Processes.PathBuilders;
using System;
using System.Diagnostics;

namespace Consoles.Processes
{
	public class ConsoleProcessService : IConsolesProcessService
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
				Arguments = descriptor.ExeStartupArgs
			};

			var consoleInstance = new ConsoleProcess(processStartInfo, descriptor.StartupTimeoutInSeconds);

			return consoleInstance;
		}

		private IPathBuilder GetPathBuilder(IProcessDescriptor descriptor)
		{
			switch (descriptor.PathBuilder)
			{
				case PathBuilder.Physical:
					return new PhysicalFilePathBuilder(descriptor.ExeFilename);
				case PathBuilder.ProgramFilesFolder:
					return new ProgramFilesFolderPathBuilder(descriptor.ExeFilename);
				case PathBuilder.System32Folder:
					return new System32FolderPathBuilder(descriptor.ExeFilename);
				case PathBuilder.SystemPathVar:
					return new SystemPathVarPathBuilder(descriptor.ExeFilename);
			}

			return null;
		}
	}
}
