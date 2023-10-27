// Ignore Spelling: Upsert

using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IBookCommandsRepository _commandsRepository;
        private readonly IBookQueriesRepository _queriesRepository;

        private readonly IMapper _mapper;

        public AuthorService(IBookCommandsRepository bookCommandsRepository, IBookQueriesRepository bookQueriesRepository, IMapper mapper)
        {
            _commandsRepository = bookCommandsRepository;
            _queriesRepository = bookQueriesRepository;
            _mapper = mapper;
        }

        public void Delete(int id)
        {
            var author = _queriesRepository.GetAuthor(id);

            if (author != null)
            {
                _commandsRepository.Delete(author);
            }
        }

        public AuthorVM Get(int id, int bookPageSize, int bookPageNo)
        {
            throw new NotImplementedException();
        }

        public AuthorDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo)
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

        public PartialList<AuthorForListVM> GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public void SelectForBook(int bookId, List<int> selectedIds)
        {
            throw new NotImplementedException();
        }

        public int Upsert(AuthorVM author)
        {
            throw new NotImplementedException();
        }
    }
}