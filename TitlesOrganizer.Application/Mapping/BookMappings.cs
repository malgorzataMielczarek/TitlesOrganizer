using System.Text;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Mapping
{
    public class BookMappings
    {
        public static Func<Book?, BookVM, Book> FromBookVM = (book, bookVM) => new Book()
        {
            Id = bookVM.Id,
            Title = bookVM.Title,
            Authors = book?.Authors ?? new List<Author>(),
            OriginalTitle = bookVM.OriginalTitle,
            OriginalLanguageCode = bookVM.OriginalLanguageCode,
            Year = bookVM.Year,
            Edition = bookVM.Edition,
            Description = bookVM.Description,
            BookSeries = book?.BookSeries,
            BookSeriesId = book?.BookSeriesId,
            NumberInSeries = book?.NumberInSeries,
            Genres = book?.Genres ?? new List<LiteratureGenre>()
        };

        public static Func<GenreVM, LiteratureGenre> FromGenreVM = genre => new LiteratureGenre()
        {
            Id = genre.Id,
            Name = genre.Name
        };

        public static Func<NewAuthorVM, Author> FromNewAuthorVM = author => new Author()
        {
            Id = author.Id,
            Name = author.Name,
            LastName = author.LastName
        };

        public static Func<Author, AuthorDetailsVM> ToAuthorDetailsVM = author => new AuthorDetailsVM()
        {
            Id = author.Id,
            FullName = author.Name + " " + author.LastName,
            Books = new ListBookForListVM()
            {
                Books = author.Books?.OrderBy(b => b.Title).Select(ToBookForListVM!).ToList() ?? new List<BookForListVM>(),
                Count = author.Books?.Count ?? 0
            }
        };

        public static Func<int, Author, AuthorForBookVM> ToAuthorForBookVM = (bookId, author) => new AuthorForBookVM()
        {
            Id = author.Id,
            FullName = author.Name + " " + author.LastName,
            IsForBook = author.Books.Any(b => b.Id == bookId),
            OtherBooks = string.Join(", ", author.Books.SkipWhile(b => b.Id == bookId).Select(b => b.Title).Order())
        };

        public static Func<Author, AuthorForListVM> ToAuthorForListVM = author => new AuthorForListVM()
        {
            Id = author.Id,
            FullName = author.Name + " " + author.LastName,
            Books = string.Join(", ", author.Books.Select(b => b.Title).Order())
        };

        public static Func<Book, BookDetailsVM> ToBookDetailsVM = book => new BookDetailsVM()
        {
            Id = book.Id,
            Title = book.Title,
            OriginalTitle = book.OriginalTitle ?? string.Empty,
            OriginalLanguage = book.OriginalLanguage?.Name ?? string.Empty,
            Authors = new Dictionary<int, string>(
                book.Authors.OrderBy(a => a.LastName).ThenBy(a => a.Name)
                .Select(a => new KeyValuePair<int, string>(a.Id, a.Name + " " + a.LastName))),
            Edition = book.Edition ?? string.Empty,
            Year = book.Year?.ToString() ?? string.Empty,
            Description = book.Description ?? string.Empty,
            SeriesId = book.BookSeriesId,
            InSeries = InSeries(book.NumberInSeries, book.BookSeries),
            Genres = new Dictionary<int, string>(
                book.Genres.OrderBy(g => g.Name)
                .Select(g => new KeyValuePair<int, string>(g.Id, g.Name)))
        };

        public static Func<Book, BookForListVM> ToBookForListVM = book => new BookForListVM()
        {
            Id = book.Id,
            Title = book.Title
        };

        public static Func<LiteratureGenre, GenreDetailsVM> ToGenreDetailsVM = genre => new GenreDetailsVM()
        {
            Id = genre.Id,
            Name = genre.Name,
            Books = new ListBookForListVM()
            {
                Books = genre.Books?.OrderBy(b => b.Title).Select(ToBookForListVM).ToList() ?? new List<BookForListVM>(),
                Count = genre.Books?.Count ?? 0
            }
        };

        public static Func<int, LiteratureGenre, GenreForBookVM> ToGenreForBookVM = (bookId, genre) => new GenreForBookVM()
        {
            Id = genre.Id,
            Name = genre.Name,
            IsForBook = genre.Books?.Any(b => b.Id == bookId) ?? false
        };

        public static Func<LiteratureGenre, GenreVM> ToGenreVM = genre => new GenreVM()
        {
            Id = genre.Id,
            Name = genre.Name
        };

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