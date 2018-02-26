using Consoles.Core;
using ReactiveUI;
using System;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console form view model interface.
	/// </summary>
	public interface IConsoleFormViewModel
	{
		event EventHandler OnDelete;
		event EventHandler OnCancel;
		event EventHandler OnSave;

		IConsoleViewModel Data { get; }
	}

	/// <summary>
	/// App console form view model implementation.
	/// </summary>
	public class ConsoleFormViewModel : ReactiveObject, IConsoleFormViewModel
	{
		private IConsoleViewModel _data;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleFormViewModel()
		{
		}

		/// <summary>
		/// Raised when data deletion is requested.
		/// </summary>
		public event EventHandler OnDelete;

		/// <summary>
		/// Raised when data add/edit is canceled.
		/// </summary>
		public event EventHandler OnCancel;

		/// <summary>
		/// Raised when data is saved.
		/// </summary>
		public event EventHandler OnSave;

		/// <summary>
		/// Gets or sets the data to add/edit.
		/// </summary>
		public IConsoleViewModel Data
		{
			get => _data;
			set => this.RaiseAndSetIfChanged(ref _data, value);
		}
	}
}

