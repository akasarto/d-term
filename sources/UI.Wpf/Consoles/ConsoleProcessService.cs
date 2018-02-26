using Processes.Core;
using Splat;
using System;
using System.Diagnostics;
using System.IO;

namespace Processes.SystemDiagnostics
{
	public class ConsoleProcessService : IProcessFactory
	{
		private readonly IProcessTracker _processTracker = null;
		private readonly IProcessPathBuilder _processPathBuilder = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessService(IProcessTracker processTracker = null, IProcessPathBuilder processPathBuilder = null)
		{
			_processTracker = processTracker ?? Locator.CurrentMutable.GetService<IProcessTracker>();
			_processPathBuilder = processPathBuilder ?? Locator.CurrentMutable.GetService<IProcessPathBuilder>();
		}

		public bool CanCreate(ProcessBasePath processBasePath, string processExecutableName)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		public IProcess Create(IProcessDescriptor processDescriptor)
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

			var consoleInstance = new ConsoleProcess(processStartInfo, processDescriptor.StartupTimeoutInSeconds);

			consoleInstance.Start();

			if (consoleInstance.IsStarted)
			{
				_processTracker.Track(consoleInstance.Id);
			}

			return consoleInstance;
		}
	}
}
