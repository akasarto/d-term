using System;

namespace UI.Wpf.Notebook
{
	public class NoteViewModel
	{
		public bool Adding { get; set; }

		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
}
