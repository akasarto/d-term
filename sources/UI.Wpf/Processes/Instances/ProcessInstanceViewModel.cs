using Processes.Core;
using System;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process instance view model interface.
	/// </summary>
	public interface IProcessInstanceViewModel
	{
		int Pid { get; }
		IProcessInstanceHost Host { get; }
		IntPtr MainWindowHandle { get; }
	}

	/// <summary>
	/// App process instance view model implementation.
	/// </summary>
	public class ProcessInstanceViewModel : IProcessInstanceViewModel
	{
		private IProcessInstance _processInstance;
		private IProcessHostFactory _processHostFactory;
		private IProcessInstanceHost _processHost;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceViewModel(IProcessInstance processInstance, IProcessHostFactory processHostFactory)
		{
			_processInstance = processInstance ?? throw new ArgumentNullException(nameof(processInstance), nameof(ProcessInstanceViewModel));
			_processHostFactory = processHostFactory ?? throw new ArgumentNullException(nameof(processHostFactory), nameof(ProcessInstanceViewModel));

			_processHost = _processHostFactory.Create(processInstance);
		}

		/// <summary>
		/// Gets the process instance id (PID).
		/// </summary>
		public int Pid => _processInstance.Id;

		/// <summary>
		/// Gets the Win32 process instance host.
		/// </summary>
		public IProcessInstanceHost Host => _processHost;

		/// <summary>
		/// Gets the process instance main window handle.
		/// </summary>
		public IntPtr MainWindowHandle => _processInstance.MainWindowHandle;
	}
}
