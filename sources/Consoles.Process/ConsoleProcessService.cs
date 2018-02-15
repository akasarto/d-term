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

		public IConsoleProcess Create(IProcessDescriptor processDescriptor)
		{
			var pathBuilder = GetPathBuilder(processDescriptor);
			var startupArgs = processDescriptor.Console.ProcessPathExeStartupArgs;

			if (pathBuilder == null)
			{
				throw new InvalidOperationException(nameof(Create), new ArgumentNullException(nameof(pathBuilder), nameof(ConsoleProcessService)));
			}

			var processStartInfo = new ProcessStartInfo(pathBuilder.Build())
			{
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = startupArgs
			};

			var consoleInstance = new ConsoleProcess(processStartInfo, processDescriptor.StartupTimeoutInSeconds)
			{
				SourceSpecifications = processDescriptor.Console
			};

			consoleInstance.Start();

			if (consoleInstance.IsStarted)
			{
				var wardenProcess = WardenProcess.GetProcessFromId(consoleInstance.Id);

				wardenProcess.OnStateChange += (object sender, StateEventArgs e) =>
				{
					
				};
			}

			return consoleInstance;
		}

		private IPathBuilder GetPathBuilder(IProcessDescriptor processDescriptor)
		{
			var pathBuilder = processDescriptor.Console.ProcessPathBuilder;
			var pathExeFilename = processDescriptor.Console.ProcessPathExeFilename;

			switch (pathBuilder)
			{
				case PathBuilder.Physical:
					return new PhysicalFilePathBuilder(pathExeFilename);
				case PathBuilder.ProgramFilesFolder:
					return new ProgramFilesFolderPathBuilder(pathExeFilename);
				case PathBuilder.System32Folder:
					return new System32FolderPathBuilder(pathExeFilename);
				case PathBuilder.SystemPathVar:
					return new SystemPathVarPathBuilder(pathExeFilename);
			}

			return null;
		}
	}
}
