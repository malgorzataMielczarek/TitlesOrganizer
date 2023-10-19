// Ignore Spelling: Upsert

using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.Mapping;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs.UpsertModelVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.DetailsVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
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

        public int AddAuthor(AuthorVM author)
        {
            return _bookRepository.AddNewAuthor(author.BookId, author.MapToBase(_mapper));
        }

        public void SelectAuthorsForBook(ListAuthorForBookVM listAuthorForBook)
        {
            Book? book = _bookRepository.GetBookById(listAuthorForBook.BookId);
            if (book == null)
            {
                return;
            }

            var selectedIds = listAuthorForBook.SelectedAuthors.Select(a => a.Id).ToList();
            // Compare lists in case JavaScript function didn't work
            foreach (var author in listAuthorForBook.List)
            {
                if (author.IsForBook)
                {
                    if (!selectedIds.Contains(author.Id))
                    {
                        selectedIds.Add(author.Id);
                    }
                }
                else
                {
                    if (selectedIds.Contains(author.Id))
                    {
                        selectedIds.Remove(author.Id);
                    }
                }
            }

            // Update list of selected authors
            foreach (var author in book.Authors.ToList())
            {
                if (selectedIds.Contains(author.Id))
                {
                    selectedIds.Remove(author.Id);
                }
                else
                {
                    book.Authors.Remove(author);
                }
            }

            foreach (var authorId in selectedIds)
            {
                Author? author = _bookRepository.GetAuthorById(authorId);
                if (author != null)
                {
                    book.Authors.Add(author);
                }
            }

            _bookRepository.UpdateAuthorsOfBook(book);
        }

        public int UpsertBook(BookVM book)
        {
            if (book == null || string.IsNullOrWhiteSpace(book.Title))
            {
                return default;
            }

            return _bookRepository.UpsertBook(book.MapToBase(_mapper));
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

        public int AddNewSeries(SeriesVM newSeries)
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

        public ListAuthorForBookVM GetAllAuthorsForBookList(int bookId, ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            Book? book = _bookRepository.GetBookById(bookId);
            if (book == null)
            {
                return new ListAuthorForBookVM();
            }

            string bookTitle = book.Title;

            return _bookRepository.GetAllAuthorsWithBooks().OrderBy(a => a.LastName).MapToList(bookId, bookTitle, sortBy, pageSize, pageNo, searchString);
        }

        public ListAuthorForListVM GetAllAuthorsForList(ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllAuthorsWithBooks().OrderBy(a => a.LastName).MapToList(sortBy, pageSize, pageNo, searchString);

        public ListBookForListVM GetAllBooksForList(ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            var books = _bookRepository.GetAllBooks();

            return books.MapToList(_mapper, sortBy, pageSize, pageNo, searchString);
        }

        public ListGenreVM GetAllGenres(ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllGenres().MapToList(sortBy, pageSize, pageNo, searchString);

        public ListGenreForBookVM GetAllGenresForBookList(int bookId, ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllGenresWithBooks().MapToList(bookId, sortBy, pageSize, pageNo, searchString);

        public ListSeriesForBookVM GetAllSeriesForBookList(int bookId, ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllSeriesWithBooks().MapToList(bookId, sortBy, pageSize, pageNo, searchString);

        public ListSeriesForListVM GetAllSeriesForList(ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAllSeriesWithBooks().MapToList(sortBy, pageSize, pageNo, searchString);

        public AuthorDetailsVM GetAuthorDetails(int id, ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetAuthorById(id)?.MapToDetails(_mapper, sortBy, pageSize, pageNo, searchString) ?? new AuthorDetailsVM();

        public BookVM GetBook(int id) => _bookRepository.GetBookById(id)?.MapFromBase(_mapper) ?? new BookVM();

        public BookDetailsVM GetBookDetails(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book?.BookSeries != null)
            {
                book.BookSeries = _bookRepository.GetSeriesById(book.BookSeries.Id);
            }

            return book?.MapToDetails() ?? new BookDetailsVM();
        }

        public GenreDetailsVM GetGenreDetails(int id, ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetGenreById(id)?.MapToDetails(_mapper, sortBy, pageSize, pageNo, searchString) ?? new GenreDetailsVM();

        public SeriesDetailsVM GetSeriesDetails(int id, ViewModels.Helpers.SortByEnum sortBy, int pageSize, int pageNo, string searchString) => _bookRepository.GetSeriesById(id)?.MapToDetails(_mapper, sortBy, pageSize, pageNo, searchString) ?? new SeriesDetailsVM();

        public ListAuthorForBookVM GetAllAuthorsForBookList(ListAuthorForBookVM listAuthorForBook) => GetAllAuthorsForBookList(
            listAuthorForBook.BookId,
            listAuthorForBook.SortBy,
            listAuthorForBook.PageSize,
            listAuthorForBook.CurrentPage,
            listAuthorForBook.SearchString!);
    }
}