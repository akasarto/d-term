using Processes.Core;
using System;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process instance host factory interface.
	/// </summary>
	public interface IProcessHostFactory
	{
		IProcessHost Create(IProcess process);
	}

	/// <summary>
	/// App process instance host factory implementation.
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
		/// <param name="process">The <see cref="IProcess"/> to be hosted.</param>
		/// <returns>An <see cref="IProcessHost"/> instance.</returns>
		public IProcessHost Create(IProcess process)
		{
			process = process ?? throw new ArgumentNullException(nameof(process), nameof(ProcessHostFactory));

			return new ProcessHost(process);
		}
	}
}
