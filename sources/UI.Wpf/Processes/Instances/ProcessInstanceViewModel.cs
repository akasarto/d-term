using Processes.Core;
using System;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process instance view model interface.
	/// </summary>
	public interface IProcessInstanceViewModel
	{
		IProcessHost ProcessHost { get; }
	}

	/// <summary>
	/// App process instance view model implementation.
	/// </summary>
	public class ProcessInstanceViewModel : IProcessInstanceViewModel
	{
		private IProcessInstance _processInstance;
		private IProcessHostFactory _processHostFactory;
		private IProcessHost _processHost;

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
		/// Gets the Win32 process host.
		/// </summary>
		public IProcessHost ProcessHost => _processHost;
	}
}
