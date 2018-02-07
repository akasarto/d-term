using AutoMapper;
using Notebook.Core;
using UI.Wpf.Notebook;

namespace UI.Wpf.MapperProfiles
{
	public class DefaultMapProfile : Profile
	{
		public DefaultMapProfile()
		{
			CreateMap<Note, NoteViewModel>();
			CreateMap<NoteViewModel, Note>();

			CreateMap<Note, NoteCardViewModel>();
			CreateMap<NoteCardViewModel, Note>();

			CreateMap<NoteViewModel, NoteCardViewModel>();
			CreateMap<NoteCardViewModel, NoteViewModel>();
		}
	}
}
