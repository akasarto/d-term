using Consoles.Core;
using Consoles.Process.PathBuilders;
using System;
using System.Diagnostics;
using Warden.Core;

namespace Consoles.Process
{
	public class ConsoleProcessService : IConsolesProcessService
	{
		static ConsoleProcessService()
		{
			WardenManager.Initialize(new WardenOptions
			{
				CleanOnExit = true,
				DeepKill = true,
				ReadFileHeaders = true
			});
		}

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

			consoleInstance.Start();

			if (consoleInstance.IsStarted)
			{
				var wardenProcess = WardenProcess.GetProcessFromId(consoleInstance.Id);

				wardenProcess.OnStateChange += WardenProcess_OnStateChange;
			}

			return consoleInstance;
		}

		private void WardenProcess_OnStateChange(object sender, StateEventArgs e)
		{
			
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
