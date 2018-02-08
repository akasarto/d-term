using System;

namespace Notebook.Core
{
	public class NoteEntity
	{
		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime UTCCreation { get; set; }
	}
}
