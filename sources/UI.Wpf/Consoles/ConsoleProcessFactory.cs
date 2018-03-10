using Processes.Core;
using Processes.SystemDiagnostics;
using Splat;
using System;
using System.Diagnostics;
using System.IO;

namespace UI.Wpf.Consoles
{
	//
	public interface IConsoleProcessFactory
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName);
		IProcess Create(IConsoleOptionViewModel processViewModel);
	}

	//
	public class ConsoleProcessFactory : IConsoleProcessFactory
	{
		private readonly IProcessTracker _processTracker;
		private readonly IProcessPathBuilder _processPathBuilder;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleProcessFactory(IProcessTracker processTracker = null, IProcessPathBuilder processPathBuilder = null)
		{
			_processTracker = processTracker ?? Locator.CurrentMutable.GetService<IProcessTracker>();
			_processPathBuilder = processPathBuilder ?? Locator.CurrentMutable.GetService<IProcessPathBuilder>();
		}

		public bool CanCreate(ProcessBasePath processBasePath, string processExecutableName)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		public IProcess Create(IConsoleOptionViewModel processViewModel)
		{
			processViewModel = processViewModel ?? throw new ArgumentNullException(nameof(processViewModel), nameof(ConsoleProcessFactory));

			if (CanCreate(processViewModel.ProcessBasePath, processViewModel.ProcessExecutableName))
			{
				var fullPath = _processPathBuilder.Build(processViewModel.ProcessBasePath, processViewModel.ProcessExecutableName);

				var processStartInfo = new ProcessStartInfo(fullPath)
				{
					WindowStyle = ProcessWindowStyle.Hidden,
					WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
					Arguments = processViewModel.ProcessStartupArgs,
					UseShellExecute = true
				};

				return new SystemProcess(processStartInfo, _processTracker);
			}

			return null;
		}
	}
}
