using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Text;
using TitlesOrganizer.Application.ViewModels.BookVMs;
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

        public static Author MapToBase(this NewAuthorVM authorVM, IMapper mapper)
        {
            return mapper.Map<Author>(authorVM);
        }

        public static Book MapToBase(this BookVM bookVM, IMapper mapper, Book? oldBook = null)
        {
            Book book = mapper.Map<Book>(bookVM);

            if (oldBook != null)
            {
                book.Authors = oldBook.Authors;
                book.BookSeries = oldBook.BookSeries;
                book.BookSeriesId = oldBook.BookSeriesId;
                book.NumberInSeries = oldBook.NumberInSeries;
                book.Genres = oldBook.Genres;
            }

            return book;
        }

        public static LiteratureGenre MapToBase(this GenreVM genreVM, IMapper mapper)
        {
            return mapper.Map<LiteratureGenre>(genreVM);
        }

        public static AuthorDetailsVM MapToDetails(this Author author, IMapper mapper)
        {
            return new AuthorDetailsVM()
            {
                Id = author.Id,
                FullName = author.Name + " " + author.LastName,
                Books = author.Books.AsQueryable().MapToList(mapper)
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
                Description = book.Description ?? string.Empty,
                Year = book.Year?.ToString() ?? string.Empty,
                Edition = book.Edition ?? string.Empty,
                SeriesId = book.BookSeriesId,
                InSeries = InSeries(book.NumberInSeries, book.BookSeries)
            };
        }

        public static GenreDetailsVM MapToDetails(this LiteratureGenre genre, IMapper mapper)
        {
            return new GenreDetailsVM()
            {
                Id = genre.Id,
                Name = genre.Name,
                Books = genre.Books?.AsQueryable().MapToList(mapper) ?? new ListBookForListVM()
            };
        }

        public static ListAuthorForBookVM MapToList(this IQueryable<Author> authors, int bookId)
        {
            List<AuthorForBookVM> authorsForList = authors.Map(bookId).OrderBy(a => a.IsForBook).ToList();

            return new ListAuthorForBookVM()
            {
                Authors = authorsForList,
                Count = authorsForList.Count,
                BookId = bookId
            };
        }

        public static ListAuthorForListVM MapToList(this IQueryable<Author> authors)
        {
            List<AuthorForListVM> authorsForList = authors.Map().ToList();

            return new ListAuthorForListVM()
            {
                Authors = authorsForList,
                Count = authorsForList.Count
            };
        }

        public static ListBookForListVM MapToList(this IQueryable<Book> books, IMapper mapper)
        {
            List<BookForListVM> booksForList = books.Map(mapper).ToList();
            return new ListBookForListVM()
            {
                Books = booksForList,
                Count = booksForList.Count
            };
        }

        public static ListGenreForBookVM MapToList(this IQueryable<LiteratureGenre> genres, int bookId)
        {
            List<GenreForBookVM> genresForBook = genres.Map(bookId).OrderBy(g => g.IsForBook).ToList();

            return new ListGenreForBookVM()
            {
                Genres = genresForBook,
                Count = genresForBook.Count,
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

    public class BookMappings : Profile
    {
        public BookMappings()
        {
            CreateMap<NewAuthorVM, Author>();
            CreateMap<BookVM, Book>();
            CreateProjection<Book, BookForListVM>();
            CreateMap<GenreVM, LiteratureGenre>();
        }
    }
}