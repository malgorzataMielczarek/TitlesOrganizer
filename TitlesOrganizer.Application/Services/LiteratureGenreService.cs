// Ignore Spelling: Upsert

using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Services
{
    public class LiteratureGenreService : ILiteratureGenreService
    {
        private readonly ILiteratureGenreCommandsRepository _commands;
        private readonly IBookModuleQueriesRepository _queries;

        private readonly IMapper _mapper;

        public LiteratureGenreService(ILiteratureGenreCommandsRepository literatureGenreCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper)
        {
            _commands = literatureGenreCommandsRepository;
            _queries = bookModuleQueriesRepository;
            _mapper = mapper;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public GenreVM Get(int id, int booksPageSize, int booksPageNo)
        {
            throw new NotImplementedException();
        }

        public GenreDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo)
        {
            throw new NotImplementedException();
        }

        public ListGenreForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListGenreForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public PartialList<LiteratureGenre> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public void SelectBooks(int genreId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public int Upsert(GenreVM genre)
        {
            throw new NotImplementedException();
        }

        protected virtual LiteratureGenre Map(GenreVM genre)
        {
            return genre.MapToBase(_mapper);
        }

        protected virtual GenreVM Map(LiteratureGenre genreWithBooks, int booksPageSize, int booksPageNo)
        {
            return genreWithBooks.MapFromBase(
                _mapper,
                new Paging()
                {
                    CurrentPage = booksPageNo,
                    PageSize = booksPageSize
                });
        }

        protected virtual GenreDetailsVM MapToDetails(LiteratureGenre genre)
        {
            return genre.MapToDetails();
        }

        protected virtual GenreDetailsVM MapGenreDetailsAuthors(GenreDetailsVM genreDetails, IQueryable<Author> authors, int pageSize, int pageNo)
        {
            genreDetails.Authors = authors.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return genreDetails;
        }

        protected virtual GenreDetailsVM MapGenreDetailsBooks(GenreDetailsVM genreDetails, IQueryable<Book> books, int pageSize, int pageNo)
        {
            genreDetails.Books = books.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return genreDetails;
        }

        protected virtual GenreDetailsVM MapGenreDetailsSeries(GenreDetailsVM genreDetails, IQueryable<BookSeries> series, int pageSize, int pageNo)
        {
            genreDetails.Series = series.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return genreDetails;
        }

        protected virtual ListGenreForListVM MapToList(IQueryable<LiteratureGenre> genres, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return genres.MapToList(
                new Paging()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize
                },
                new Filtering()
                {
                    SearchString = searchString,
                    SortBy = sortBy
                });
        }

        protected virtual ListGenreForBookVM MapForBook(IQueryable<LiteratureGenre> genresWithBooks, Book book, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return genresWithBooks.MapForItemToList(
                book,
                new Paging()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize
                },
                new Filtering()
                {
                    SearchString = searchString,
                    SortBy = sortBy
                });
        }

        protected virtual PartialList<LiteratureGenre> MapToPartialList(IQueryable<LiteratureGenre> genres, int pageSize, int pageNo)
        {
            return (PartialList<LiteratureGenre>)genres.MapToPartialList(
                new Paging() { CurrentPage = pageNo, PageSize = pageSize });
        }
    }
}