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
    public class BookSeriesService : IBookSeriesService
    {
        private readonly IBookSeriesCommandsRepository _commands;
        private readonly IBookModuleQueriesRepository _queries;

        private readonly IMapper _mapper;

        public BookSeriesService(IBookSeriesCommandsRepository bookSeriesCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper)
        {
            _commands = bookSeriesCommandsRepository;
            _queries = bookModuleQueriesRepository;
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

        public PartialList<BookSeries> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public PartialList<BookSeries> GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            throw new NotImplementedException();
        }

        public void SelectBooks(int seriesId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public int Upsert(SeriesVM series)
        {
            throw new NotImplementedException();
        }

        protected virtual BookSeries Map(SeriesVM series)
        {
            return series.MapToBase(_mapper);
        }
    }
}