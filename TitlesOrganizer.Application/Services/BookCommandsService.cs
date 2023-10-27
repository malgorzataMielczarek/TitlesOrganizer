// Ignore Spelling: Upsert

using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class BookCommandsService : IBookCommandsService
    {
        private readonly IBookCommandsRepository _commandsRepository;
        private readonly IBookQueriesRepository _queriesRepository;

        private readonly IMapper _mapper;

        public BookCommandsService(IBookCommandsRepository bookCommandsRepository, IBookQueriesRepository bookQueriesRepository, IMapper mapper)
        {
            _commandsRepository = bookCommandsRepository;
            _queriesRepository = bookQueriesRepository;
            _mapper = mapper;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public BookVM Get(int id)
        {
            throw new NotImplementedException();
        }

        public BookDetailsVM GetDetails(int id)
        {
            throw new NotImplementedException();
        }

        public ListBookForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListBookForAuthorVM GetListForAuthor(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListBookForGenreVM GetListForGenre(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public ListBookForSeriesVM GetListForSeries(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            throw new NotImplementedException();
        }

        public PartialList<BookForListVM> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public PartialList<BookForListVM> GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public void SelectForAuthor(int authorId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public void SelectForGenre(int genreId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public void SelectForSeries(int seriesId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public int Upsert(BookVM book)
        {
            throw new NotImplementedException();
        }
    }
}