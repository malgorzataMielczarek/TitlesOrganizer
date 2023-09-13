using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.Mapping;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Services
{//TODO: include related data when puling data from database
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public int AddAuthor(NewAuthorVM author)
        {
            return _bookRepository.AddNewAuthor(author.BookId, author.MapToBase(_mapper));
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
            return _bookRepository.AddBook(book.MapToBase(_mapper));
        }

        public int AddGenre(GenreVM genre)
        {
            return AddGenre(default, genre);
        }

        public int AddGenre(int bookId, GenreVM genre)
        {
            return _bookRepository.AddNewGenre(bookId, genre.MapToBase(_mapper));
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

        public ListAuthorForBookVM GetAllAuthorsForBookList(int bookId) => _bookRepository.GetAllAuthorsWithBooks().OrderBy(a => a.LastName).MapToList(bookId);

        public ListAuthorForListVM GetAllAuthorsForList() => _bookRepository.GetAllAuthorsWithBooks().OrderBy(a => a.LastName).MapToList();

        public ListBookForListVM GetAllBooksForList() => _bookRepository.GetAllBooks().OrderBy(b => b.Title).MapToList(_mapper);

        public List<GenreVM> GetAllGenres() => _bookRepository.GetAllGenres().OrderBy(g => g.Name).Map().ToList();

        public ListGenreForBookVM GetAllGenresForBookList(int bookId) => _bookRepository.GetAllGenresWithBooks().MapToList(bookId);

        public AuthorDetailsVM GetAuthorDetails(int id) => _bookRepository.GetAuthorById(id)?.MapToDetails(_mapper) ?? new AuthorDetailsVM();

        public BookDetailsVM GetBookDetails(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book?.BookSeries != null)
            {
                book.BookSeries = _bookRepository.GetSeriesById(book.BookSeries.Id);
            }

            return book?.MapToDetails() ?? new BookDetailsVM();
        }

        public GenreDetailsVM GetGenreDetails(int id) => _bookRepository.GetGenreById(id)?.MapToDetails(_mapper) ?? new GenreDetailsVM();

        public GenreDetailsVM GetSeriesDetails(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(BookVM bookVM)
        {
            Book? oldBook = _bookRepository.GetBookById(bookVM.Id);
            int id = _bookRepository.UpdateBook(bookVM.MapToBase(_mapper, oldBook));
        }
    }
}