using AutoMapper;
using Processes.Core;
using Processes.SystemDiagnostics;
using Splat;
using System;
using System.Diagnostics;
using System.IO;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process instance factory interface.
	/// </summary>
	public interface IProcessInstanceFactory
	{
		bool CanCreate(ProcessBasePath processBasePath, string processExecutableName);
		IProcessInstanceViewModel Create(IProcessViewModel IProcessViewModel, int startupTimeoutInSeconds = 3);
	}

	/// <summary>
	/// App process instance factory implementation.
	/// </summary>
	public class ProcessInstanceFactory : IProcessInstanceFactory
	{
		//
		private readonly IProcessTracker _processTracker;
		private readonly IProcessPathBuilder _processPathBuilder;
		private readonly IProcessHostFactory _processHostFactory;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceFactory(IProcessTracker processTracker = null, IProcessPathBuilder processPathBuilder = null, IProcessHostFactory processHostFactory = null)
		{
			_processTracker = processTracker ?? Locator.CurrentMutable.GetService<IProcessTracker>();
			_processPathBuilder = processPathBuilder ?? Locator.CurrentMutable.GetService<IProcessPathBuilder>();
			_processHostFactory = processHostFactory ?? Locator.CurrentMutable.GetService<IProcessHostFactory>();
		}

		/// <summary>
		/// Checks if the process instance can be created.
		/// </summary>
		/// <param name="processBasePath">The type of base folder path.</param>
		/// <param name="processExecutableName">The process executable file name.</param>
		/// <returns><c>True</c> if can be created, otherwise, <c>false</c>.</returns>
		public bool CanCreate(ProcessBasePath processBasePath, string processExecutableName)
		{
			var path = _processPathBuilder.Build(processBasePath, processExecutableName);

			return !string.IsNullOrWhiteSpace(path) && new FileInfo(path).Exists;
		}

		/// <summary>
		/// Create a new process instance.
		/// </summary>
		/// <param name="processViewModel">The process view model.</param>
		/// <returns>An <see cref="IProcessInstanceViewModel"/> instance.</returns>
		public IProcessInstanceViewModel Create(IProcessViewModel processViewModel, int startupTimeoutInSeconds = 3)
		{
			IProcessInstanceViewModel result = null;

			if (processViewModel == null || !CanCreate(processViewModel.ProcessBasePath, processViewModel.ProcessExecutableName))
			{
				return null;
			}

			var fullPath = _processPathBuilder.Build(processViewModel.ProcessBasePath, processViewModel.ProcessExecutableName);

			var processStartInfo = new ProcessStartInfo(fullPath)
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				Arguments = processViewModel.ProcessStartupArgs
			};

			var process = new SysProcess(processStartInfo, startupTimeoutInSeconds);

			if (process.Start())
			{
				_processTracker.Track(process.Id);

				result = Mapper.Map<IProcessInstanceViewModel>(process);

				result = Mapper.Map(processViewModel, result, typeof(IProcessViewModel), typeof(IProcessInstanceViewModel)) as IProcessInstanceViewModel;
			}

			if (result == null)
			{
				process.Dispose();
			}

			return result;
		}
	}
}
