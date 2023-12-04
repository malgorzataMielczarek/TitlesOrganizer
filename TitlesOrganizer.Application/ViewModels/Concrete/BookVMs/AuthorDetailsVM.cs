using AutoMapper;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.Concrete.BookVMs
{
    public class AuthorDetailsVM : DetailsVM
    {
        public IPartialListVM Books { get; set; } = new PartialListVM();
        public IPartialListVM Genres { get; set; } = new PartialListVM();
        public IPartialListVM Series { get; set; } = new PartialListVM();
    }

    public class AuthorDetailsMappings : Profile
    {
        public AuthorDetailsMappings()
        {
            CreateMap<Author, AuthorDetailsVM>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(source => $"{source.Name} {source.LastName}"))
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}