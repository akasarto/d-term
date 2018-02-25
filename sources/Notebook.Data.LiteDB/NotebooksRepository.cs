using LiteDB;
using Notebook.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notebook.Data.LiteDB
{
	public class NotebooksRepository : INotebooksRepository
	{
		//
		private string _notesCollection = "notes";

		//
		private readonly string _connectionString = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NotebooksRepository(string connectionString)
		{
			_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString), nameof(NotebooksRepository));
		}

		public NoteEntity Add(NoteEntity note)
		{
			note.Id = Guid.NewGuid();

			using (var database = new LiteDatabase(_connectionString))
			{
				var notes = database.GetCollection<NoteEntity>(_notesCollection);

				notes.Insert(note);
			}

			return note;
		}

		public void Delete(Guid noteId)
		{
			using (var database = new LiteDatabase(_connectionString))
			{
				var notes = database.GetCollection<NoteEntity>(_notesCollection);

				notes.Delete(n => n.Id == noteId);
			}
		}

		public List<NoteEntity> GetAll()
		{
			using (var database = new LiteDatabase(_connectionString))
			{
				var notes = database.GetCollection<NoteEntity>(_notesCollection);

				return notes.FindAll().ToList();
			}
		}

		public void Update(NoteEntity noteEntity)
		{
			using (var database = new LiteDatabase(_connectionString))
			{
				var notes = database.GetCollection<NoteEntity>(_notesCollection);

				var note = notes.FindOne(n => n.Id == noteEntity.Id);

				if (note != null)
				{
					note.Title = noteEntity.Title;
					note.Description = noteEntity.Description;

					notes.Update(noteEntity);
				}
			}
		}
	}
}

