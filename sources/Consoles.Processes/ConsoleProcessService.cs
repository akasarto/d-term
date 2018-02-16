using Consoles.Core;
using System;
using System.Diagnostics;
using System.IO;

namespace Consoles.Processes
{
	public class ConsoleProcessService : IConsoleProcessService
	{
		private readonly IProcessTracker _processTracker = null;
		private readonly IProcessPathBuilder _processPathBuilder = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessService(IProcessTracker processTracker, IProcessPathBuilder processPathBuilder)
		{
			_processTracker = processTracker ?? throw new ArgumentNullException(nameof(processTracker), nameof(ConsoleProcessService));
			_processPathBuilder = processPathBuilder ?? throw new ArgumentNullException(nameof(processPathBuilder), nameof(ConsoleProcessService));
		}

		public bool CanCreate(BasePath processBasePath, string processExecutableName)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		public IConsoleProcess Create(IProcessDescriptor processDescriptor)
		{
			var console = processDescriptor?.ConsoleOption;

			if (console == null || !CanCreate(console.ProcessBasePath, console.ProcessExecutableName))
			{
				return null;
			}

			var fullPath = _processPathBuilder.Build(console.ProcessBasePath, console.ProcessExecutableName);
			var startupArgs = processDescriptor.ConsoleOption.ProcessStartupArgs;

			var processStartInfo = new ProcessStartInfo(fullPath)
			{
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				WindowStyle = ProcessWindowStyle.Hidden,
				Arguments = startupArgs
			};

			var consoleInstance = new ConsoleProcess(processStartInfo, processDescriptor.StartupTimeoutInSeconds)
			{
				Source = processDescriptor.ConsoleOption
			};

			consoleInstance.Start();

			if (consoleInstance.IsStarted)
			{
				_processTracker.Track(consoleInstance.Id);
			}

			return consoleInstance;
		}
	}
}
