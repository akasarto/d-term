using ReactiveUI;
using System;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Process form view model interface.
	/// </summary>
	public interface IProcessFormViewModel
	{
		event EventHandler OnCancel;
		event EventHandler OnSave;

		IProcessViewModel Data { get; set; }
	}

	/// <summary>
	/// App process form view model implementation.
	/// </summary>
	public class ProcessFormViewModel : ReactiveObject, IProcessFormViewModel
	{
		private IProcessViewModel _data;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessFormViewModel()
		{
		}

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
		public IProcessViewModel Data
		{
			get => _data;
			set => this.RaiseAndSetIfChanged(ref _data, value);
		}
	}
}

