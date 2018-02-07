using System;
using AutoMapper;
using Notebook.Core;
using UI.Wpf.Notebook;

namespace UI.Wpf.Mappings
{
	public class MapProfileNotebook : Profile
	{
		//
		private readonly INotebookRepository _notebookRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MapProfileNotebook(INotebookRepository notebookRepository)
		{
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(MapProfileNotebook));

			SetupMaps();
		}

		private void SetupMaps()
		{
			CreateMap<Note, NoteViewModel>();
			CreateMap<NoteViewModel, Note>();

			CreateMap<Note, NoteAddViewModel>().ConstructUsing(x => new NoteAddViewModel(_notebookRepository));
			CreateMap<NoteAddViewModel, Note>();

			CreateMap<NoteViewModel, NoteAddViewModel>().ConstructUsing(x => new NoteAddViewModel(_notebookRepository));
			CreateMap<NoteAddViewModel, NoteViewModel>();

			CreateMap<Note, NoteDetailsViewModel>().ConstructUsing(x => new NoteDetailsViewModel(_notebookRepository));
			CreateMap<NoteAddViewModel, Note>();

			CreateMap<NoteViewModel, NoteDetailsViewModel>().ConstructUsing(x => new NoteDetailsViewModel(_notebookRepository));
			CreateMap<NoteDetailsViewModel, NoteViewModel>();
		}
	}
}
