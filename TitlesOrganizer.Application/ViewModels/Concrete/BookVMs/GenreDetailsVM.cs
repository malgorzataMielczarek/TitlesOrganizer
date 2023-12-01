using AutoMapper;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.Concrete.BookVMs
{
    public class GenreDetailsVM : BaseDetailsVM
    {
        public IPartialListVM Authors { get; set; } = new PartialListVM();
        public IPartialListVM Books { get; set; } = new PartialListVM();
        public IPartialListVM Series { get; set; } = new PartialListVM();
    }

    public class GenreDetailsMappings : Profile
    {
        public GenreDetailsMappings()
        {
            CreateMap<LiteratureGenre, GenreDetailsVM>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}