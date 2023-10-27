// Ignore Spelling: Upsert

using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class LiteratureGenreService : ILiteratureGenreService
    {
        private readonly IBookCommandsRepository _commandsRepository;
        private readonly IBookQueriesRepository _queriesRepository;

        private readonly IMapper _mapper;

        public LiteratureGenreService(IBookCommandsRepository bookCommandsRepository, IBookQueriesRepository bookQueriesRepository, IMapper mapper)
        {
            _commandsRepository = bookCommandsRepository;
            _queriesRepository = bookQueriesRepository;
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

        public PartialList<GenreForListVM> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public void SelectForBook(int bookId, List<int> selectedIds)
        {
            throw new NotImplementedException();
        }

        public int Upsert(GenreVM genre)
        {
            throw new NotImplementedException();
        }
    }
}