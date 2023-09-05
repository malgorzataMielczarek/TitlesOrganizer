using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.Mapping;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookMappings _bookMappings = new BookMappings();

        public int AddAuthor(NewAuthorVM author)
        {
            Author authorModel = BookMappings.FromNewAuthorVM(author);

            int id = _bookRepository.AddNewAuthor(author.BookId, authorModel);
            return id;
        }

        public void AddAuthorsForBook(int bookId, List<int> authorsIds)
        {
            foreach (var authorId in authorsIds)
            {
                _bookRepository.AddExistingAuthor(bookId, authorId);
            }

            var authorsToRemoveIds = _bookRepository.GetBookById(bookId)?.Authors.Select(a => a.Id).SkipWhile(id => authorsIds.Contains(id));
            if (authorsToRemoveIds?.Any() ?? false)
            {
                foreach (var authorId in authorsToRemoveIds)
                {
                    _bookRepository.RemoveAuthor(bookId, authorId);
                }
            }
        }

        public int AddBook(BookVM book)
        {
            Book bookModel = BookMappings.FromBookVM(null, book);

            var id = _bookRepository.AddBook(bookModel);

            return id;
        }

        public int AddGenre(GenreVM genre)
        {
            return AddGenre(default, genre);
        }

        public int AddGenre(int bookId, GenreVM genre)
        {
            LiteratureGenre genreModel = BookMappings.FromGenreVM(genre);

            int id = _bookRepository.AddNewGenre(bookId, genreModel);

            return id;
        }

        public void AddGenresForBook(int bookId, List<int> genresIds)
        {
            foreach (int genreId in genresIds)
            {
                _bookRepository.AddExistingGenre(bookId, genreId);
            }

            var genresToRemoveIds = _bookRepository.GetBookById(bookId)?.Genres.Select(g => g.Id).SkipWhile(id => genresIds.Contains(id));
            if (genresToRemoveIds?.Any() ?? false)
            {
                foreach (var genreId in genresToRemoveIds)
                {
                    _bookRepository.RemoveGenre(bookId, genreId);
                }
            }
        }

        public void DeleteBook(int id)
        {
            _bookRepository.DeleteBook(id);
        }

        public ListAuthorForBookVM GetAllAuthorsForBookList(int bookId)
        {
            var authors = _bookRepository.GetAllAuthors()
                .OrderBy(a => a.LastName)
                .Select(a => BookMappings.ToAuthorForBookVM(bookId, a))
                .ToList();

            ListAuthorForBookVM list = new ListAuthorForBookVM()
            {
                Authors = authors,
                Count = authors.Count,
                BookId = bookId
            };

            return list;
        }

        public ListAuthorForListVM GetAllAuthorsForList()
        {
            List<AuthorForListVM> authors = _bookRepository.GetAllAuthors()
                .OrderBy(a => a.LastName)
                .Select(BookMappings.ToAuthorForListVM)
                .ToList();

            var list = new ListAuthorForListVM()
            {
                Authors = authors,
                Count = authors.Count
            };

            return list;
        }

        public ListBookForListVM GetAllBooksForList()
        {
            List<BookForListVM> books = _bookRepository.GetAllBooks()
                .Select(BookMappings.ToBookForListVM)
                .OrderBy(bVM => bVM.Title)
                .ToList();

            var list = new ListBookForListVM()
            {
                Books = books,
                Count = books.Count
            };

            return list;
        }

        public List<GenreVM> GetAllGenres()
        {
            var genres = _bookRepository.GetAllGenres()
                .OrderBy(g => g.Name)
                .Select(BookMappings.ToGenreVM)
                .ToList();

            return genres;
        }

        public ListGenreForBookVM GetAllGenresForBookList(int bookId)
        {
            var genres = _bookRepository.GetAllGenres()
                .Select(g => BookMappings.ToGenreForBookVM(bookId, g))
                .OrderBy(gVM => gVM.IsForBook)
                .ToList();

            var list = new ListGenreForBookVM()
            {
                Genres = genres,
                Count = genres.Count,
                BookId = bookId
            };

            return list;
        }

        public AuthorDetailsVM GetAuthorDetails(int id)
        {
            var author = _bookRepository.GetAuthorById(id);
            AuthorDetailsVM authorVM = author != null ? BookMappings.ToAuthorDetailsVM(author) : new AuthorDetailsVM();
            return authorVM;
        }

        public BookDetailsVM GetBookDetails(int id)
        {
            Book? book = _bookRepository.GetBookById(id);
            BookDetailsVM bookVM = book != null ? BookMappings.ToBookDetailsVM(book) : new BookDetailsVM();
            return bookVM;
        }

        public GenreDetailsVM GetGenreDetails(int id)
        {
            LiteratureGenre? genre = _bookRepository.GetGenreById(id);
            GenreDetailsVM genreVM = genre != null ? BookMappings.ToGenreDetailsVM(genre) : new GenreDetailsVM();
            return genreVM;
        }

        public void UpdateBook(BookVM bookVM)
        {
            Book? book = _bookRepository.GetBookById(bookVM.Id);
            book = BookMappings.FromBookVM(book, bookVM);

            int id = _bookRepository.UpdateBook(book);
        }
    }
}