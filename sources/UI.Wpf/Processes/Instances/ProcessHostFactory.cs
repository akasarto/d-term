using Processes.Core;
using System;

namespace UI.Wpf.Processes
{
	public interface IProcessHostFactory
	{
		IProcessHost Create(IProcess process);
	}

	public class ProcessHostFactory : IProcessHostFactory
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessHostFactory()
		{
		}

		public IProcessHost Create(IProcess process)
		{
			process = process ?? throw new ArgumentNullException(nameof(process), nameof(ProcessHostFactory));

			return new ProcessHost(process);
		}
	}
}
