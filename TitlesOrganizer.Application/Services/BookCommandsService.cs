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
    public class BookCommandsService : IBookCommandsService
    {
        private readonly IBookCommandsRepository _commands;
        private readonly IBookModuleQueriesRepository _queries;

        private readonly IMapper _mapper;

        public BookCommandsService(IBookCommandsRepository bookCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper)
        {
            _commands = bookCommandsRepository;
            _queries = bookModuleQueriesRepository;
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

        public void SelectAuthors(int bookId, List<int> selectedIds)
        {
            throw new NotImplementedException();
        }

        public void SelectGenres(int bookId, List<int> selectedIds)
        {
            throw new NotImplementedException();
        }

        public void SelectSeries(int bookId, int? selectedId)
        {
            throw new NotImplementedException();
        }

        public int Upsert(BookVM book)
        {
            throw new NotImplementedException();
        }

        protected virtual Book Map(BookVM book)
        {
            return book.MapToBase(_mapper);
        }
    }
}