using System;
using System.Collections.Generic;
using Notebook.Core;

namespace Notebook.Data.LiteDB
{
	public class NotebookRepository : INotebookRepository
	{
		public List<Note> GetAll()
		{
			return new List<Note>()
			{
				new Note() { Id = Guid.NewGuid(), Index = 1, Title = "First", Description = "Aaa" },
				new Note() { Id = Guid.NewGuid(), Index = 2, Title = "Second", Description = "Bbb" },
				new Note() { Id = Guid.NewGuid(), Index = 3, Title = "Third", Description = "Ccc" }
			};
		}
	}
}
