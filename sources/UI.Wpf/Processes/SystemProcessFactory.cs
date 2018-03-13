using Processes.Core;
using Processes.SystemDiagnostics;
using Splat;
using System;
using System.Diagnostics;
using System.IO;

namespace UI.Wpf.Processes
{
	//
	public interface IProcessInstanceFactory
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName);
		IProcess Create(IProcessViewModel processViewModel);
	}

	//
	public class SystemProcessFactory : IProcessInstanceFactory
	{
		private readonly IProcessTracker _processTracker;
		private readonly IProcessPathBuilder _processPathBuilder;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SystemProcessFactory(IProcessTracker processTracker = null, IProcessPathBuilder processPathBuilder = null)
		{
			_processTracker = processTracker ?? Locator.CurrentMutable.GetService<IProcessTracker>();
			_processPathBuilder = processPathBuilder ?? Locator.CurrentMutable.GetService<IProcessPathBuilder>();
		}

		public bool CanCreate(ProcessBasePath processBasePath, string processExecutableName)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		public IProcess Create(IProcessViewModel consoleOptionViewModel)
		{
			consoleOptionViewModel = consoleOptionViewModel ?? throw new ArgumentNullException(nameof(consoleOptionViewModel), nameof(SystemProcessFactory));

			if (CanCreate(consoleOptionViewModel.ProcessBasePath, consoleOptionViewModel.ProcessExecutableName))
			{
				var fullPath = _processPathBuilder.Build(consoleOptionViewModel.ProcessBasePath, consoleOptionViewModel.ProcessExecutableName);

				var processStartInfo = new ProcessStartInfo(fullPath)
				{
					UseShellExecute = true,
					WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
					Arguments = consoleOptionViewModel.ProcessStartupArgs
				};

				return new SystemProcess(processStartInfo, _processTracker);
			}

			return null;
		}
	}
}
