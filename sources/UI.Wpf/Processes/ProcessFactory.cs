using Processes.Core;
using Processes.Systems.Local;
using Splat;
using System;
using System.Diagnostics;
using System.IO;

namespace UI.Wpf.Processes
{
	//
	public interface IProcessFactory
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName, string processStartupArgs = null, bool startAsAdmin = false);
		IProcess Create(ProcessBasePath processBasePath, string processExecutableName, string processStartupArgs = null, bool startAsAdmin = false);
		IProcess Create(IProcessViewModel processViewModel, bool startAsAdmin = false);
	}

	//
	public class ProcessFactory : IProcessFactory
	{
		private readonly IProcessPathBuilder _processPathBuilder;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessFactory(IProcessPathBuilder processPathBuilder = null)
		{
			_processPathBuilder = processPathBuilder ?? Locator.CurrentMutable.GetService<IProcessPathBuilder>();
		}

		public bool CanCreate(ProcessBasePath processBasePath, string processExecutableName, string processStartupArgs = null, bool startAsAdmin = false)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		public IProcess Create(ProcessBasePath processBasePath, string processExecutableName, string processStartupArgs = null, bool startAsAdmin = false)
		{
			var fullPath = _processPathBuilder.Build(processBasePath, processExecutableName);

			var processStartInfo = new ProcessStartInfo(fullPath)
			{
				UseShellExecute = true,
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				Arguments = processStartupArgs,
				Verb = startAsAdmin ? "runas" : string.Empty
			};

			return new SystemProcess(processStartInfo);
		}

		public IProcess Create(IProcessViewModel consoleOptionViewModel, bool startAsAdmin = false)
		{
			consoleOptionViewModel = consoleOptionViewModel ?? throw new ArgumentNullException(nameof(consoleOptionViewModel), nameof(ProcessFactory));

			if (!CanCreate(consoleOptionViewModel.ProcessBasePath, consoleOptionViewModel.ProcessExecutableName))
			{
				return null;
			}

			return Create(
				consoleOptionViewModel.ProcessBasePath,
				consoleOptionViewModel.ProcessExecutableName,
				consoleOptionViewModel.ProcessStartupArgs,
				startAsAdmin
			);
		}
	}
}
