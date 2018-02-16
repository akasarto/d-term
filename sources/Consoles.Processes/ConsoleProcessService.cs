using Consoles.Core;
using System;
using System.Diagnostics;

namespace Consoles.Processes
{
	public class ConsoleProcessService : IConsolesProcessService
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

		public IConsoleProcess Create(IProcessDescriptor processDescriptor)
		{
			var fullPath = _processPathBuilder.Build(processDescriptor.Console);
			var startupArgs = processDescriptor.Console.ProcessStartupArgs;

			var processStartInfo = new ProcessStartInfo(fullPath)
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
				_processTracker.Track(consoleInstance.Id);
			}

			return consoleInstance;
		}
	}
}
