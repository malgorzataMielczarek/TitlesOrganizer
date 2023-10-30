// Ignore Spelling: Upsert

using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
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

        public PartialList<GenreForListVM> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
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
    }
}