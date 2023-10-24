// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs
{
    public class SeriesVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        [DisplayName("Original title")]
        public string? OriginalTitle { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();

        [ScaffoldColumn(false)]
        public Paging BooksPaging { get; set; } = new Paging();
    }

    public class SeriesValidator : AbstractValidator<SeriesVM>
    {
        public SeriesValidator()
        {
            RuleFor(s => s.Id).NotNull();
            RuleFor(s => s.Title).NotNull().NotEmpty().MaximumLength(225);
            RuleFor(s => s.OriginalTitle).MaximumLength(225);
            RuleFor(s => s.Description).MaximumLength(2000);
        }
    }

    public static partial class MappingExtensions
    {
        public static BookSeries MapToBase(this SeriesVM seriesVM, IMapper mapper)
        {
            return mapper.Map<BookSeries>(seriesVM);
        }

        public static SeriesVM MapFromBase(this BookSeries seriesWithBooks, IMapper mapper, Paging booksPaging)
        {
            var seriesVM = mapper.Map<SeriesVM>(seriesWithBooks);
            booksPaging.Count = seriesWithBooks.Books.Count;
            seriesVM.Books = seriesWithBooks.Books
                .OrderBy(b => b.Title)
                .Skip(booksPaging.PageSize * (booksPaging.CurrentPage - 1))
                .Take(booksPaging.PageSize)
                .Map();
            seriesVM.BooksPaging = booksPaging;

            return seriesVM;
        }

        public static SeriesVM MapFromBase(this BookSeries series, IMapper mapper, IQueryable<Book> books, Paging booksPaging)
        {
            var seriesVM = mapper.Map<SeriesVM>(series);
            booksPaging.Count = books?.Count() ?? 0;
            seriesVM.Books = books?
                .Map()
                .OrderBy(b => b.Title)
                .Skip(booksPaging.PageSize * (booksPaging.CurrentPage - 1))
                .Take(booksPaging.PageSize)
                .ToList() ?? new List<BookForListVM>();
            seriesVM.BooksPaging = booksPaging;

            return seriesVM;
        }
    }

    public class SeriesMappings : Profile
    {
        public SeriesMappings()
        {
            CreateMap<SeriesVM, BookSeries>()
                .ForMember(dest => dest.Books, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}