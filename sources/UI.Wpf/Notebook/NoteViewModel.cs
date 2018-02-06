using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteViewModel : ReactiveObject
	{
		private Guid _id;
		private int _intex;
		private string _title;
		private string _description;

		public Guid Id
		{
			get => _id;
			set => this.RaiseAndSetIfChanged(ref _id, value);
		}

		public int Index
		{
			get => _intex;
			set => this.RaiseAndSetIfChanged(ref _intex, value);
		}

		public string Title
		{
			get => _title;
			set => this.RaiseAndSetIfChanged(ref _title, value);
		}

		public string Description
		{
			get => _description;
			set => this.RaiseAndSetIfChanged(ref _description, value);
		}
	}
}
