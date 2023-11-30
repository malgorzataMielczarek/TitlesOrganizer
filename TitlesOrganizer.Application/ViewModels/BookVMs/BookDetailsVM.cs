using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookDetailsVM : BaseDetailsVM
    {
        public List<IForListVM> Authors { get; set; } = new();

        public string Description { get; set; } = string.Empty;

        public string Edition { get; set; } = string.Empty;

        public List<IForListVM> Genres { get; set; } = new List<IForListVM>();

        [ScaffoldColumn(false)]
        public string InSeries { get; set; } = string.Empty;

        [DisplayName("Original language")]
        public string OriginalLanguage { get; set; } = string.Empty;

        [DisplayName("Original title")]
        public string OriginalTitle { get; set; } = string.Empty;

        [ScaffoldColumn(false)]
        public IForListVM? Series { get; set; }

        public string Year { get; set; } = string.Empty;
    }

    public class BookDetailsMappings : Profile
    {
        public BookDetailsMappings()
        {
            CreateMap<Book, BookDetailsVM>()
                .ForMember(dest => dest.OriginalLanguage, opt => opt.Ignore())
                .ForMember(dest => dest.Authors, opt => opt.Ignore())
                .ForMember(dest => dest.Series, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore());
        }
    }
}