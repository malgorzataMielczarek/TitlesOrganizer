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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorCommandsRepository _commands;
        private readonly IBookModuleQueriesRepository _queries;

        private readonly IMapper _mapper;

        public AuthorService(IAuthorCommandsRepository authorCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper)
        {
            _commands = authorCommandsRepository;
            _queries = bookModuleQueriesRepository;
            _mapper = mapper;
        }

        public void Delete(int id)
        {
            var author = _queries.GetAuthor(id);

            if (author != null)
            {
                _commands.Delete(author);
            }
        }

        public AuthorVM Get(int id, int bookPageSize, int bookPageNo)
        {
            throw new NotImplementedException();
        }

        public AuthorDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int seriesPageSize, int seriesPageNo, int genresPageSize, int genresPageNo)
        {
            throw new NotImplementedException();
        }

        public ListAuthorForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListAuthorForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public PartialList<Author> GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public void SelectBooks(int authorId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public int Upsert(AuthorVM author)
        {
            throw new NotImplementedException();
        }

        protected virtual Author Map(AuthorVM author)
        {
            return author.MapToBase(_mapper);
        }

        protected virtual AuthorVM Map(Author author, int bookPageSize, int bookPageNo)
        {
            return author.MapFromBase(_mapper, new Paging() { CurrentPage = bookPageNo, PageSize = bookPageSize });
        }

        protected virtual AuthorDetailsVM MapToDetails(Author author, IQueryable<Book> books, int bookPageSize, int bookPageNo, IQueryable<BookSeries> series, int seriesPageSize, int seriesPageNo, IQueryable<LiteratureGenre> genres, int genrePageSize, int genrePageNo)
        {
            return author.MapToDetails(
                books, new Paging() { CurrentPage = bookPageNo, PageSize = bookPageSize },
                series, new Paging() { CurrentPage = seriesPageNo, PageSize = seriesPageSize },
                genres, new Paging() { CurrentPage = genrePageNo, PageSize = genrePageSize });
        }

        protected virtual ListAuthorForListVM MapToList(IQueryable<Author> authorList, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            return authorList.MapToList(
                new Paging()
                {
                    PageSize = pageSize,
                    CurrentPage = pageNo
                },
                new Filtering()
                {
                    SortBy = sortBy,
                    SearchString = searchString ?? string.Empty
                });
        }
    }
}