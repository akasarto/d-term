using System;

namespace UI.Wpf.Consoles
{
	public class ConsoleErrorMessage
	{
		/// <summary>
		/// Gets or sets the exception.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// Gets or sets the friendly error message.
		/// </summary>
		public string Message { get; set; }
	}
}
