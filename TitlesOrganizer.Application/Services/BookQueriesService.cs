using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.DetailsVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForAuthorVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForBookVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForGenreVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForSeriesVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class BookQueriesService : IBookQueriesService
    {
        private readonly IBookQueriesRepository _bookQueriesRepository;

        private readonly IMapper _mapper;

        public BookQueriesService(IBookQueriesRepository bookQueriesRepository, IMapper mapper)
        {
            _bookQueriesRepository = bookQueriesRepository;
            _mapper = mapper;
        }

        public AuthorVM GetAuthor(int id, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public AuthorDetailsVM GetAuthorDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo)
        {
            throw new NotImplementedException();
        }

        public ListAuthorForListVM GetAuthorsList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListAuthorForBookVM GetAuthorsListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public PartialList<AuthorForListVM> GetAuthorsPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public BookVM GetBook(int id)
        {
            throw new NotImplementedException();
        }

        public BookDetailsVM GetBookDetails(int id)
        {
            throw new NotImplementedException();
        }

        public ListBookForListVM GetBooksList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListBookForAuthorVM GetBooksListForAuthor(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListBookForGenreVM GetBooksListForGenre(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListBookForSeriesVM GetBooksListForSeries(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public PartialList<BookForListVM> GetBooksPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public PartialList<BookForListVM> GetBooksPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public GenreVM GetGenre(int id, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public GenreDetailsVM GetGenreDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo)
        {
            throw new NotImplementedException();
        }

        public ListGenreForListVM GetGenresList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListGenreForBookVM GetGenresListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public PartialList<GenreForListVM> GetGenresPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public SeriesVM GetSeries(int id, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public SeriesDetailsVM GetSeriesDetails(int id, int booksPageSize, int booksPageNo)
        {
            throw new NotImplementedException();
        }

        public ListSeriesForListVM GetSeriesList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListSeriesForBookVM GetSeriesListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public PartialList<SeriesForListVM> GetSeriesPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public PartialList<SeriesForListVM> GetSeriesPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }
    }
}