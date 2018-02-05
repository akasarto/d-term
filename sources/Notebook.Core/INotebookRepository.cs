using System.Collections.Generic;

namespace Notebook.Core
{
	public interface INotebookRepository
	{
		List<Note> GetAll();
	}
}
