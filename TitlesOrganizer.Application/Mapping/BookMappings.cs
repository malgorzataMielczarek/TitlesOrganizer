using AutoMapper;
using System.Text;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Mapping
{
    public class BookMappings : Profile
    {
        public BookMappings()
        {
            // Parameters
            int bookId = default;

            // Author mappings From
            CreateMap<NewAuthorVM, Author>();
            // To
            CreateMap<Author, AuthorDetailsVM>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name + " " + src.LastName));
            CreateProjection<Author, AuthorForBookVM>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name + " " + src.LastName))
                .ForMember(dest => dest.IsForBook, opt => opt.MapFrom(src => src.Books.Any(b => b.Id == bookId)))
                .ForMember(dest => dest.OtherBooks, opt => opt.MapFrom(src => string.Join(", ", src.Books.SkipWhile(b => b.Id == bookId).Select(b => b.Title).Order())));
            CreateProjection<Author, AuthorForListVM>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name + " " + src.LastName))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => string.Join(", ", src.Books.Select(b => b.Title).Order())));

            // Book mappings From
            CreateMap<BookVM, Book>();
            //To
            CreateMap<Book, BookDetailsVM>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => new Dictionary<int, string>(src.Authors.OrderBy(a => a.LastName).ThenBy(a => a.Name).Select(a => new KeyValuePair<int, string>(a.Id, a.Name + " " + a.LastName)))))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year != null ? src.Year.ToString() : string.Empty))
                .ForMember(dest => dest.InSeries, opt => opt.MapFrom(src => InSeries(src.NumberInSeries, src.BookSeries)))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => new Dictionary<int, string>(
                src.Genres.OrderBy(g => g.Name)
                .Select(g => new KeyValuePair<int, string>(g.Id, g.Name)))));
            CreateProjection<Book, BookForListVM>();
            CreateMap<ICollection<Book>, ListBookForListVM>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src != null ? src.OrderBy(b => b.Title).Select(b => new BookForListVM()
                {
                    Id = b.Id,
                    Title = b.Title
                }).ToList() : new List<BookForListVM>()))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src != null ? src.Count : 0));

            // Genre mappings From
            CreateMap<GenreVM, LiteratureGenre>();
            // To
            CreateMap<LiteratureGenre, GenreDetailsVM>();
            CreateProjection<LiteratureGenre, GenreForBookVM>()
                .ForMember(dest => dest.IsForBook, opt => opt.MapFrom(src => src.Books != null ? src.Books.Any(b => b.Id == bookId) : false));
            CreateProjection<LiteratureGenre, GenreVM>();
        }

        private static string InSeries(int? numberInSeries, BookSeries? bookSeries)
        {
            if (bookSeries == null)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();
            if (numberInSeries != null)
            {
                result.Append(numberInSeries);
                int count;
                if ((count = bookSeries.Books.Count) > 0)
                {
                    result.Append(" of ");
                    result.Append(count);
                }

                if (bookSeries.Title != null)
                {
                    result.Append(" in ");
                    result.Append(bookSeries.Title);
                    result.Append(" series");
                }
            }
            else if (bookSeries.Title != null)
            {
                result.Append("Part of ");
                result.Append(bookSeries.Title);
                result.Append(" series");
            }

            return result.ToString();
        }
    }
}