using Processes.Core;
using System;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process host factory interface.
	/// </summary>
	public interface IProcessHostFactory
	{
		IProcessHost Create(IProcessInstance processInstance);
	}

	/// <summary>
	/// App process host factory implementation.
	/// </summary>
	public class ProcessHostFactory : IProcessHostFactory
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessHostFactory()
		{
		}

		/// <summary>
		/// Create a new process host instance.
		/// </summary>
		/// <param name="processInstance">The <see cref="IProcessInstance"/> to be hosted.</param>
		/// <returns>An <see cref="IProcessHost"/> instance.</returns>
		public IProcessHost Create(IProcessInstance processInstance)
		{
			processInstance = processInstance ?? throw new ArgumentNullException(nameof(processInstance), nameof(ProcessHostFactory));

			return new ProcessHost(processInstance);
		}
	}
}
