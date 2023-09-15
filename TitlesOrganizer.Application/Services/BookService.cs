using AutoMapper;
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

        public int AddNewSeries(NewSeriesVM newSeries)
        {
            return _bookRepository.AddNewSeries(newSeries.BookId, newSeries.MapToBase(_mapper));
        }

        public void AddSeriesForBook(int bookId, int seriesId)
        {
            _bookRepository.AddExistingSeries(bookId, seriesId);
        }

        public void DeleteBook(int id)
        {
            _bookRepository.DeleteBook(id);
        }

        public ListAuthorForBookVM GetAllAuthorsForBookList(int bookId, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllAuthorsWithBooks().OrderBy(a => a.LastName).MapToList(bookId, sortBy, pageSize, pageNo, searchString);

        public ListAuthorForListVM GetAllAuthorsForList(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllAuthorsWithBooks().OrderBy(a => a.LastName).MapToList(sortBy, pageSize, pageNo, searchString);

        public ListBookForListVM GetAllBooksForList(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            var books = _bookRepository.GetAllBooks();

            return books.MapToList(_mapper, sortBy, pageSize, pageNo, searchString);
        }

        public ListGenreVM GetAllGenres(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllGenres().MapToList(sortBy, pageSize, pageNo, searchString);

        public ListGenreForBookVM GetAllGenresForBookList(int bookId, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllGenresWithBooks().MapToList(bookId, sortBy, pageSize, pageNo, searchString);

        public ListSeriesForBookVM GetAllSeriesForBookList(int bookId, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllSeriesWithBooks().MapToList(bookId, sortBy, pageSize, pageNo, searchString);

        public ListSeriesForListVM GetAllSeriesForList(ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllSeriesWithBooks().MapToList(sortBy, pageSize, pageNo, searchString);

        public AuthorDetailsVM GetAuthorDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAuthorById(id)?.MapToDetails(_mapper, sortBy, pageSize, pageNo, searchString) ?? new AuthorDetailsVM();

        public BookDetailsVM GetBookDetails(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book?.BookSeries != null)
            {
                book.BookSeries = _bookRepository.GetSeriesById(book.BookSeries.Id);
            }

            return book?.MapToDetails() ?? new BookDetailsVM();
        }

        public GenreDetailsVM GetGenreDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetGenreById(id)?.MapToDetails(_mapper, sortBy, pageSize, pageNo, searchString) ?? new GenreDetailsVM();

        public SeriesDetailsVM GetSeriesDetails(int id, ViewModels.Common.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetSeriesById(id)?.MapToDetails(_mapper, sortBy, pageSize, pageNo, searchString) ?? new SeriesDetailsVM();

        public void UpdateBook(BookVM bookVM)
        {
            Book? oldBook = _bookRepository.GetBookById(bookVM.Id);
            int id = _bookRepository.UpdateBook(bookVM.MapToBase(_mapper, oldBook));
        }
    }
}