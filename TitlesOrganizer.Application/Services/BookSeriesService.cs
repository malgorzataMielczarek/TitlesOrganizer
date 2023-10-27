// Ignore Spelling: Upsert

using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class BookSeriesService : IBookSeriesService
    {
        private readonly IBookCommandsRepository _commandsRepository;
        private readonly IBookQueriesRepository _queriesRepository;

        private readonly IMapper _mapper;

        public BookSeriesService(IBookCommandsRepository bookCommandsRepository, IBookQueriesRepository bookQueriesRepository, IMapper mapper)
        {
            _commandsRepository = bookCommandsRepository;
            _queriesRepository = bookQueriesRepository;
            _mapper = mapper;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public SeriesVM Get(int id, int booksPageSize, int booksPageNo)
        {
            throw new NotImplementedException();
        }

        public SeriesDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo)
        {
            throw new NotImplementedException();
        }

        public ListSeriesForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListSeriesForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public PartialList<SeriesForListVM> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public PartialList<SeriesForListVM> GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public void SelectForBook(int bookId, int? selectedIds)
        {
            throw new NotImplementedException();
        }

        public int Upsert(SeriesVM series)
        {
            throw new NotImplementedException();
        }
    }
}