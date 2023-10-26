// Ignore Spelling: Validator

using AutoMapper;
using FluentValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs
{
    public class BookVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public List<AuthorForListVM> Authors { get; set; } = new List<AuthorForListVM>();

        [DisplayName("Original title")]
        public string? OriginalTitle { get; set; }

        [DisplayName("Original language")]
        public string? OriginalLanguageCode { get; set; }

        public int? Year { get; set; }

        public string? Edition { get; set; }

        public List<GenreForListVM> Genres { get; set; } = new List<GenreForListVM>();

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        public SeriesForListVM? Series { get; set; }

        [DisplayName("Volume number")]
        public int? NumberInSeries { get; set; }
    }

    public class BookValidator : AbstractValidator<BookVM>
    {
        public BookValidator()
        {
            RuleFor(b => b.Id).NotNull();
            RuleFor(b => b.Title).NotNull().NotEmpty().MaximumLength(225);
            RuleFor(b => b.OriginalTitle).MaximumLength(225);
            RuleFor(b => b.OriginalLanguageCode).Length(3).Unless(b => string.IsNullOrEmpty(b.OriginalLanguageCode));
            RuleForEach(b => b.OriginalLanguageCode).InclusiveBetween('A', 'Z');
            RuleFor(b => b.Year)
                .NotEqual(0)
                .LessThanOrEqualTo(DateTime.Now.Year);
            RuleFor(b => b.Edition).MaximumLength(25);
            RuleFor(b => b.Description).MaximumLength(2000);
        }
    }

    public static partial class MappingExtensions
    {
        public static Book MapToBase(this BookVM bookVM, IMapper mapper)
        {
            return mapper.Map<Book>(bookVM);
        }

        public static BookVM MapFromBase(this Book bookWithAllRelatedObjects, IMapper mapper)
        {
            var bookVM = mapper.Map<BookVM>(bookWithAllRelatedObjects);
            bookVM.Authors = bookWithAllRelatedObjects.Authors.Map();
            bookVM.Genres = bookWithAllRelatedObjects.Genres.Map();
            bookVM.Series = bookWithAllRelatedObjects.Series?.Map();

            return bookVM;
        }

        public static BookVM MapFromBase(this Book book, IMapper mapper, IQueryable<Author> authors, IQueryable<LiteratureGenre> genres, BookSeries? series)
        {
            var bookVM = mapper.Map<BookVM>(book);
            bookVM.Authors = authors?.Map().ToList() ?? new List<AuthorForListVM>();
            bookVM.Genres = genres?.Map().ToList() ?? new List<GenreForListVM>();
            bookVM.Series = series?.Map();

            return bookVM;
        }
    }

    public class BookMappings : Profile
    {
        public BookMappings()
        {
            CreateMap<BookVM, Book>()
                .ForMember(dest => dest.Authors, opt => opt.Ignore())
                .ForMember(dest => dest.Series, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Authors, opt => opt.Ignore())
                .ForMember(dest => dest.Series, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore());
        }
    }
}