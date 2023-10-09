using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Text;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Mapping
{
    public static class BookMappingsExtensions
    {
        public static IQueryable<AuthorForBookVM> Map(this IQueryable<Author> authors, int bookId)
        {
            return authors.Select(a => new AuthorForBookVM()
            {
                Id = a.Id,
                FullName = a.Name + " " + a.LastName,
                IsForBook = a.Books.Any(b => b.Id == bookId),
                OtherBooks = string.Join(", ", a.Books.Where(b => b.Id != bookId).OrderBy(b => b.Title).Select(b => b.Title))
            });
        }

        public static IQueryable<AuthorForListVM> Map(this IQueryable<Author> authors)
        {
            return authors.Select(a => new AuthorForListVM()
            {
                Id = a.Id,
                FullName = a.Name + " " + a.LastName,
                Books = string.Join(", ", a.Books.OrderBy(b => b.Title).Select(b => b.Title))
            });
        }

        public static IQueryable<BookForListVM> Map(this IQueryable<Book> books, IMapper mapper)
        {
            return books.ProjectTo<BookForListVM>(mapper.ConfigurationProvider);
        }

        public static IQueryable<GenreForBookVM> Map(this IQueryable<LiteratureGenre> genres, int bookId)
        {
            return genres.Select(g => new GenreForBookVM()
            {
                Id = g.Id,
                Name = g.Name,
                IsForBook = g.Books != null && g.Books.Any(b => b.Id == bookId)
            });
        }

        public static IQueryable<GenreVM> Map(this IQueryable<LiteratureGenre> genres)
        {
            return genres.Select(g => new GenreVM() { Id = g.Id, Name = g.Name });
        }

        public static IQueryable<SeriesForBookVM> Map(this IQueryable<BookSeries> series, int bookId)
        {
            return series.Select(s => new SeriesForBookVM()
            {
                Id = s.Id,
                Title = s.Title ?? string.Empty,
                IsForBook = s.Books.Any(b => b.Id == bookId),
                OtherBooks = string.Join(", ", s.Books.Where(b => b.Id != bookId).OrderBy(b => b.Title).Select(b => b.NumberInSeries))
            });
        }

        public static IQueryable<SeriesForListVM> Map(this IQueryable<BookSeries> series)
        {
            return series.Select(s => new SeriesForListVM()
            {
                Id = s.Id,
                Title = s.Title ?? string.Empty,
                Books = string.Join(", ", s.Books.OrderBy(b => b.Title).Select(b => b.NumberInSeries))
            });
        }

        public static BookVM MapForUpdate(this Book book, IMapper mapper)
        {
            var bookVM = mapper.Map<BookVM>(book);
            bookVM.Authors = string.Join(", ", book.Authors.Select(a => a.Name + " " + a.LastName));
            bookVM.Genres = string.Join(", ", book.Genres.Select(g => g.Name));
            bookVM.Series = book.BookSeries?.Title;

            return bookVM;
        }

        public static Author MapToBase(this NewAuthorVM authorVM, IMapper mapper)
        {
            return mapper.Map<Author>(authorVM);
        }

        public static Book MapToBase(this BookVM bookVM, IMapper mapper)
        {
            return mapper.Map<Book>(bookVM);
        }

        public static LiteratureGenre MapToBase(this GenreVM genreVM, IMapper mapper)
        {
            return mapper.Map<LiteratureGenre>(genreVM);
        }

        public static BookSeries MapToBase(this NewSeriesVM seriesVM, IMapper mapper)
        {
            return mapper.Map<BookSeries>(seriesVM);
        }

        public static AuthorDetailsVM MapToDetails(this Author author, IMapper mapper, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return new AuthorDetailsVM()
            {
                Id = author.Id,
                FullName = author.Name + " " + author.LastName,
                Books = author.Books.AsQueryable().MapToList(mapper, sortBy, pageSize, pageNo, searchString)
            };
        }

        public static BookDetailsVM MapToDetails(this Book book)
        {
            return new BookDetailsVM()
            {
                Id = book.Id,
                Title = book.Title,
                OriginalTitle = book.OriginalTitle ?? string.Empty,
                OriginalLanguage = book.OriginalLanguage?.Name ?? string.Empty,
                Authors = new Dictionary<int, string>(book.Authors.Select(a => new KeyValuePair<int, string>(a.Id, a.Name + " " + a.LastName))),
                Genres = new Dictionary<int, string>(book.Genres.Select(g => new KeyValuePair<int, string>(g.Id, g.Name))),
                Description = book.Description?.Replace("\\r\\n", "\r\n").Replace("\\n\\r", "\n\r").Replace("\\n", "\n") ?? string.Empty,
                Year = book.Year?.ToString() ?? string.Empty,
                Edition = book.Edition ?? string.Empty,
                SeriesId = book.BookSeriesId,
                SeriesTitle = book.BookSeries?.Title,
                InSeries = InSeries(book.NumberInSeries, book.BookSeries)
            };
        }

        public static GenreDetailsVM MapToDetails(this LiteratureGenre genre, IMapper mapper, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return new GenreDetailsVM()
            {
                Id = genre.Id,
                Name = genre.Name,
                Books = genre.Books?.AsQueryable().MapToList(mapper, sortBy, pageSize, pageNo, searchString) ?? new ListBookForListVM()
            };
        }

        public static SeriesDetailsVM MapToDetails(this BookSeries series, IMapper mapper, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            var details = mapper.Map<SeriesDetailsVM>(series);
            details.Books = series.Books?.AsQueryable().MapToList(mapper, sortBy, pageSize, pageNo, searchString) ?? new ListBookForListVM();

            return details;
        }

        public static ListAuthorForBookVM MapToList(this IQueryable<Author> authors, int bookId, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            searchString ??= string.Empty;

            var list = authors
                .Sort(sortBy, a => a.LastName, a => a.Name)
                .Map(bookId);
            int count = list.Count();
            var limitedList = list
                .Where(a => a.FullName.Contains(searchString))
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize);

            return new ListAuthorForBookVM(limitedList, count, sortBy, pageSize, pageNo, searchString)
            {
                BookId = bookId,
                SelectedAuthors = list.Where(a => a.IsForBook).ToList()
            };
        }

        public static ListAuthorForListVM MapToList(this IQueryable<Author> authors, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            searchString ??= string.Empty;

            var list = authors
                .Sort(sortBy, a => a.LastName, a => a.Name)
                .Map()
                .Where(a => a.FullName.Contains(searchString));
            int count = list.Count();
            var limitedList = list
                .Skip(pageSize * (pageSize - 1))
                .Take(pageSize);

            return new ListAuthorForListVM(limitedList, count, sortBy, pageSize, pageNo, searchString);
        }

        public static ListBookForListVM MapToList(this IQueryable<Book> books, IMapper mapper, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            searchString ??= string.Empty;

            var list = books
                .Where(b => b.Title.Contains(searchString))
                .Sort(sortBy, b => b.Title);
            int count = list.Count();
            var limitedList = list
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize)
                .Map(mapper);

            return new ListBookForListVM(limitedList, count, sortBy, pageSize, pageNo, searchString);
        }

        public static ListGenreVM MapToList(this IQueryable<LiteratureGenre> genres, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            searchString ??= string.Empty;

            var list = genres
                .Where(g => g.Name.Contains(searchString))
                .Sort(sortBy, g => g.Name)
                .Map();
            int count = list.Count();
            var limitedList = list
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize);

            return new ListGenreVM(limitedList, count, sortBy, pageSize, pageNo, searchString);
        }

        public static ListGenreForBookVM MapToList(this IQueryable<LiteratureGenre> genres, int bookId, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            searchString ??= string.Empty;

            var list = genres
                .Map(bookId)
                .Where(g => g.Name.Contains(searchString) || g.IsForBook)
                .Sort(SortByEnum.Descending, g => g.IsForBook,
                (SortBy: sortBy, Selector: g => g.Name));
            int count = list.Count();
            var limitedList = list
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize);

            return new ListGenreForBookVM(limitedList, count, sortBy, pageSize, pageNo, searchString)
            {
                BookId = bookId
            };
        }

        public static ListSeriesForListVM MapToList(this IQueryable<BookSeries> series, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            var list = series
                .Where(s => string.IsNullOrEmpty(searchString)
                || (s.Title != null && s.Title.Contains(searchString))
                || s.Books.Any(b => b.Title.Contains(searchString)))
                .Map()
                .Sort(sortBy, s => s.Title, s => s.Books);
            int count = list.Count();
            var limitedList = list
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize);

            return new ListSeriesForListVM(limitedList, count, sortBy, pageSize, pageNo, searchString);
        }

        public static ListSeriesForBookVM MapToList(this IQueryable<BookSeries> series, int bookId, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            var list = series
                .Map(bookId)
                .Where(s => s.IsForBook
                || string.IsNullOrEmpty(searchString)
                || (s.Title != null && s.Title.Contains(searchString))
                || s.OtherBooks.Contains(searchString))
                .Sort(SortByEnum.Descending, s => s.IsForBook,
                (SortBy: sortBy, Selector: s => s.Title),
                (SortBy: sortBy, Selector: s => s.OtherBooks));
            int count = list.Count();
            var limitedList = list
                .Skip(pageSize * (pageNo - 1))
                .Take(pageSize);

            return new ListSeriesForBookVM(limitedList, count, sortBy, pageSize, pageNo, searchString)
            {
                BookId = bookId
            };
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
                }
            }
            else if (bookSeries.Title != null)
            {
                result.Append("Part of ");
            }

            return result.ToString();
        }
    }

    public class BookMappings : Profile
    {
        public BookMappings()
        {
            CreateMap<NewAuthorVM, Author>().ForMember(dest => dest.Books, opt => opt.Ignore());
            CreateMap<BookVM, Book>()
                .ForMember(dest => dest.Authors, opt => opt.Ignore())
                .ForMember(dest => dest.BookSeries, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore()).ReverseMap();
            CreateProjection<Book, BookForListVM>();
            CreateMap<GenreVM, LiteratureGenre>().ForMember(dest => dest.Books, opt => opt.Ignore());
            CreateMap<BookSeries, SeriesDetailsVM>().ForMember(dest => dest.Books, opt => opt.Ignore());
            CreateMap<NewSeriesVM, BookSeries>().ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}