using Notebook.Core;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteDeletedMessage
	{
		public NoteDeletedMessage(NoteEntity deletedNoteEntity)
		{
			DeletedNote = deletedNoteEntity ?? throw new ArgumentNullException(nameof(deletedNoteEntity), nameof(NoteAddedMessage));
		}

		public NoteEntity DeletedNote { get; private set; }
	}
}
