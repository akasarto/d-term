using Processes.Core;
using System;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process instance host factory interface.
	/// </summary>
	public interface IProcessHostFactory
	{
		IProcessInstanceHost Create(IProcessInstance processInstance);
	}

	/// <summary>
	/// App process instance host factory implementation.
	/// </summary>
	public class ProcessInstanceHostFactory : IProcessHostFactory
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessInstanceHostFactory()
		{
		}

		/// <summary>
		/// Create a new process host instance.
		/// </summary>
		/// <param name="processInstance">The <see cref="IProcessInstance"/> to be hosted.</param>
		/// <returns>An <see cref="IProcessInstanceHost"/> instance.</returns>
		public IProcessInstanceHost Create(IProcessInstance processInstance)
		{
			processInstance = processInstance ?? throw new ArgumentNullException(nameof(processInstance), nameof(ProcessInstanceHostFactory));

			return new ProcessInstanceHost(processInstance);
		}
	}
}
