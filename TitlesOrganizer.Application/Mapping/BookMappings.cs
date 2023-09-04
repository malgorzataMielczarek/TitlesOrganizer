using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Mapping
{
    public class BookMappings
    {
        public Func<Book, BookForListVM> ToBookForListVM = book => new BookForListVM()
        {
            Id = book.Id,
            Title = book.Title
        };

        public Func<Author, AuthorForListVM> ToAuthorForListVM = author => new AuthorForListVM()
        {
            Id = author.Id,
            FullName = author.Name + " " + author.LastName,
            Books = string.Join(", ", author.Books.Select(b => b.Title))
        };

        public Func<int, Author, AuthorForBookVM> ToAuthorForBookVM = (bookId, author) => new AuthorForBookVM()
        {
            Id = author.Id,
            FullName = author.Name + " " + author.LastName,
            IsForBook = author.Books.Any(b => b.Id == bookId),
            OtherBooks = string.Join(", ", author.Books.SkipWhile(b => b.Id == bookId).Select(b => b.Title))
        };

        public Func<GenreVM, LiteratureGenre> FromGenreVM = genre => new LiteratureGenre()
        {
            Id = genre.Id,
            Name = genre.Name
        };

        public Func<BookVM, Book> FromBookVM = book => new Book()
        {
            Title = book.Title,
            OriginalTitle = book.OriginalTitle,
            OriginalLanguageCode = book.OriginalLanguageCode,
            Year = book.Year,
            Edition = book.Edition,
            Description = book.Description
        };

        public Func<NewAuthorVM, Author> FromNewAuthorVM = author => new Author()
        {
            Name = author.Name,
            LastName = author.LastName
        };

        public Func<LiteratureGenre, GenreVM> ToGenreVM = genre => new GenreVM()
        {
            Id = genre.Id,
            Name = genre.Name
        };

        public Func<int, LiteratureGenre, GenreForBookVM> ToGenreForBookVM = (bookId, genre) => new GenreForBookVM()
        {
            Id = genre.Id,
            Name = genre.Name,
            IsForBook = genre.Books?.Any(b => b.Id == bookId) ?? false
        };
    }
}