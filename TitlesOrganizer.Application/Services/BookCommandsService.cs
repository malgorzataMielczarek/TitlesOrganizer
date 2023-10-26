// Ignore Spelling: Upsert

using AutoMapper;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs;
using TitlesOrganizer.Domain.Interfaces;

namespace TitlesOrganizer.Application.Services
{
    public class BookCommandsService : IBookCommandsService
    {
        private readonly IBookCommandsRepository _repository;

        private readonly IMapper _mapper;

        public BookCommandsService(IBookCommandsRepository bookCommandsRepository, IMapper mapper)
        {
            _repository = bookCommandsRepository;
            _mapper = mapper;
        }

        public void DeleteAuthor(int id)
        {
            var author = _repository.GetAuthor(id);

            if (author != null)
            {
                _repository.Delete(author);
            }
        }

        public void DeleteBook(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteGenre(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteSeries(int id)
        {
            throw new NotImplementedException();
        }

        public void SelectAuthorsForBook(int bookId, List<int> authorsIds)
        {
            throw new NotImplementedException();
        }

        public void SelectBooksForAuthor(int authorId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public void SelectBooksForGenre(int genreId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public void SelectBooksForSeries(int seriesId, List<int> booksIds)
        {
            throw new NotImplementedException();
        }

        public void SelectGenresForBook(int bookId, List<int> genresIds)
        {
            throw new NotImplementedException();
        }

        public void SelectSeriesForBook(int bookId, int? seriesIds)
        {
            throw new NotImplementedException();
        }

        public int UpsertAuthor(AuthorVM author)
        {
            throw new NotImplementedException();
        }

        public int UpsertBook(BookVM book)
        {
            throw new NotImplementedException();
        }

        public int UpsertGenre(GenreVM genre)
        {
            throw new NotImplementedException();
        }

        public int UpsertSeries(SeriesVM series)
        {
            throw new NotImplementedException();
        }
    }
}