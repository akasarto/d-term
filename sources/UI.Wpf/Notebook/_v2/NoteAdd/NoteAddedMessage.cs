using Notebook.Core;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteAddedMessage
	{
		public NoteAddedMessage(NoteEntity newNoteEntity)
		{
			NewNote = newNoteEntity ?? throw new ArgumentNullException(nameof(newNoteEntity), nameof(NoteAddedMessage));
		}

		public NoteEntity NewNote { get; private set; }
	}
}
