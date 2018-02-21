using Notebook.Core;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteEditedMessage
	{
		public NoteEditedMessage(NoteEntity newNoteEntity, NoteEntity oldNoteEntity)
		{
			NewNote = newNoteEntity ?? throw new ArgumentNullException(nameof(newNoteEntity), nameof(NoteEditedMessage));
			OldNote = oldNoteEntity ?? throw new ArgumentNullException(nameof(oldNoteEntity), nameof(NoteEditedMessage));
		}

		public NoteEntity NewNote { get; private set; }

		public NoteEntity OldNote { get; private set; }
	}
}
