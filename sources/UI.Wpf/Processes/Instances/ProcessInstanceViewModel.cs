using Processes.Core;
using ReactiveUI;
using System;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process instance view model interface.
	/// </summary>
	public interface IProcessInstanceViewModel
	{
		int Pid { get; }
		string Name { get; set; }
		IProcessHost Host { get; }
		IntPtr MainWindowHandle { get; }
	}

	/// <summary>
	/// App process instance view model implementation.
	/// </summary>
	public class ProcessInstanceViewModel : ReactiveObject, IProcessInstanceViewModel
	{
		//
		private readonly IProcess _process;
		private readonly IProcessHostFactory _processHostFactory;

		//
		private string _name;
		private IProcessHost _processHost;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceViewModel(IProcess process, IProcessHostFactory processHostFactory)
		{
			_process = process ?? throw new ArgumentNullException(nameof(process), nameof(ProcessInstanceViewModel));
			_processHostFactory = processHostFactory ?? throw new ArgumentNullException(nameof(processHostFactory), nameof(ProcessInstanceViewModel));
		}

		/// <summary>
		/// Gets the underlying process id (PID).
		/// </summary>
		public int Pid => _process.Id;

		/// <summary>
		/// Gets or sets the instance name.
		/// </summary>
		public string Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		/// <summary>
		/// Gets the instance Win32 process host.
		/// </summary>
		public IProcessHost Host
		{
			get
			{
				if (_processHost == null)
				{
					_processHost = _processHostFactory.Create(_process);
				}

				return _processHost;
			}
		}

		/// <summary>
		/// Gets the underlying process main window handle.
		/// </summary>
		public IntPtr MainWindowHandle => _process.MainWindowHandle;
	}
}
