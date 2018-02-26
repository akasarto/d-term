using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Validation rules for console instances.
	/// </summary>
	public class ConsoleValidator : AbstractValidator<IConsoleViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleValidator()
		{
		}
	}
}
