using System;

namespace UI.Wpf.Notebook
{
	public class NoteListItemViewModel
	{
		public bool Adding { get; set; }

		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
}
