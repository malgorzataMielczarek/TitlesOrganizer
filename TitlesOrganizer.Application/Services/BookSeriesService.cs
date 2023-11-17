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
            var series = _queries.GetBookSeries(id);
            if (series != null)
            {
                _commands.Delete(series);
            }
        }

        public SeriesVM Get(int id, int booksPageSize, int booksPageNo)
        {
            var series = _queries.GetBookSeriesWithBooks(id);
            if (series != null)
            {
                return Map(series, booksPageSize, booksPageNo);
            }
            else
            {
                return new SeriesVM() { Books = new PartialList<Book>(booksPageSize) };
            }
        }

        public SeriesDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo)
        {
            var series = _queries.GetBookSeries(id);
            if (series != null)
            {
                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                    .Where(b => b.SeriesId == id)
                    .ToList();
                var authors = books.SelectMany(b => b.Authors)
                    .DistinctBy(a => a.Id);
                var genres = books.SelectMany(b => b.Genres)
                    .DistinctBy(g => g.Id);
                return MapToDetails(series, books, booksPageSize, booksPageNo, authors, genres);
            }
            else
            {
                return new SeriesDetailsVM() { Books = new PartialList<Book>(booksPageSize) };
            }
        }

        public ListSeriesForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var series = _queries.GetAllBookSeries();
            return MapToList(series, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public ListSeriesForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var book = _queries.GetBook(bookId) ?? new Book() { Title = string.Empty };
            var series = _queries.GetAllBookSeriesWithBooks();

            return MapForBook(series, book, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public PartialList<BookSeries> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            var author = _queries.GetAuthor(authorId);
            if (author != null)
            {
                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                    .Where(b => b.Authors.Any(a => a.Id == authorId)).ToList();
                var series = books
                    .Where(b => b.Series != null)
                    .Select(b => b.Series!)
                    .DistinctBy(s => s.Id);
                return MapToPartialList(series, pageSize, pageNo);
            }
            else
            {
                return new PartialList<BookSeries>(pageSize);
            }
        }

        public PartialList<BookSeries> GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            var genre = _queries.GetLiteratureGenre(genreId);
            if (genre != null)
            {
                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                    .Where(b => b.Genres.Any(a => a.Id == genreId))
                    .ToList();
                var series = books
                    .Where(b => b.Series != null)
                    .Select(b => b.Series!)
                    .DistinctBy(s => s.Id);

                return MapToPartialList(series, pageSize, pageNo);
            }
            else
            {
                return new PartialList<BookSeries>(pageSize);
            }
        }

        public void SelectBooks(int seriesId, List<int> selectedIds)
        {
            var series = _queries.GetBookSeries(seriesId);

            if (series != null)
            {
                var books = new List<Book>();
                foreach (var id in selectedIds)
                {
                    var book = _queries.GetBook(id);
                    if (book != null)
                    {
                        books.Add(book);
                    }
                }

                series.Books = books;
                _commands.UpdateBookSeriesBooksRelation(series);
            }
        }

        public int Upsert(SeriesVM series)
        {
            var entity = Map(series);
            if (entity != null)
            {
                if (entity.Id == default)
                {
                    return _commands.Insert(entity);
                }
                else
                {
                    _commands.Update(entity);
                    return entity.Id;
                }
            }

            return -1;
        }

        protected virtual BookSeries Map(SeriesVM series)
        {
            return series.MapToBase(_mapper);
        }

        protected virtual SeriesVM Map(BookSeries seriesWithBooks, int bookPageSize, int bookPageNo)
        {
            return seriesWithBooks.MapFromBase(_mapper, new Paging()
            {
                CurrentPage = bookPageNo,
                PageSize = bookPageSize
            });
        }

        protected virtual SeriesDetailsVM MapToDetails(BookSeries series, IEnumerable<Book> books, int bookPageSize, int bookPageNo, IEnumerable<Author> authors, IEnumerable<LiteratureGenre> genres)
        {
            return series.MapToDetails(
                books,
                new Paging()
                {
                    CurrentPage = bookPageNo,
                    PageSize = bookPageSize
                },
                authors,
                genres);
        }

        protected virtual ListSeriesForListVM MapToList(IQueryable<BookSeries> series, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return series.MapToList(
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

        protected virtual ListSeriesForBookVM MapForBook(IQueryable<BookSeries> series, Book book, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return series.MapForItemToList(
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

        protected virtual PartialList<BookSeries> MapToPartialList(IEnumerable<BookSeries> series, int pageSize, int pageNo)
        {
            return (PartialList<BookSeries>)series.MapToPartialList(
                new Paging()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize
                });
        }
    }
}