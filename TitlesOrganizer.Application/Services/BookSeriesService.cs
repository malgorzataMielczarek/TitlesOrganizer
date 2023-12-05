// Ignore Spelling: Upsert

using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.Mappings.Abstract;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.Services
{
    public class BookSeriesService(IBookSeriesCommandsRepository _commands, IBookModuleQueriesRepository _queries, IBookVMsMappings _mappings)
        : IBookSeriesService
    {
        public void Delete(int id)
        {
            var series = _queries.GetBookSeries(id);
            if (series != null)
            {
                SelectBooks(id, []);
                _commands.Delete(series);
            }
        }

        public SeriesVM Get(int id, int booksPageSize, int booksPageNo)
        {
            var series = _queries.GetBookSeriesWithBooks(id);
            if (series != null)
            {
                var seriesVM = _mappings.Map<BookSeries, SeriesVM>(series);
                var paging = new Paging() { CurrentPage = booksPageNo, PageSize = booksPageSize };
                seriesVM.Books = _mappings.Map(series.Books, paging);
                return seriesVM;
            }
            else
            {
                return new SeriesVM()
                {
                    Books = new PartialListVM(booksPageSize)
                };
            }
        }

        public SeriesDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo)
        {
            var series = _queries.GetBookSeries(id);
            if (series != null)
            {
                var result = _mappings.Map<BookSeries, SeriesDetailsVM>(series);

                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                    .Where(b => b.SeriesId == id)
                    .ToList();
                var paging = new Paging() { CurrentPage = booksPageNo, PageSize = booksPageSize };
                result.Books = _mappings.Map(books, paging);

                var authors = books.SelectMany(b => b.Authors)
                    .DistinctBy(a => a.Id);
                result.Authors = _mappings.Map(authors);

                var genres = books.SelectMany(b => b.Genres)
                    .DistinctBy(g => g.Id);
                result.Genres = _mappings.Map(genres);

                return result;
            }
            else
            {
                return new SeriesDetailsVM()
                {
                    Books = new PartialListVM(booksPageSize)
                };
            }
        }

        public IListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var series = _queries.GetAllBookSeries();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.Map(series, paging, filtering);
        }

        public IListForItemVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var book = _queries.GetBook(bookId) ?? new Book() { Title = string.Empty };
            var series = _queries.GetAllBookSeriesWithBooks();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.MapToListForItem(series, book, paging, filtering);
        }

        public IPartialListVM GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            var author = _queries.GetAuthor(authorId);
            if (author != null)
            {
                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                    .Where(b => b.Authors.Any(a => a.Id == authorId))
                    .ToList();
                var series = books
                    .Where(b => b.Series != null)
                    .Select(b => b.Series!)
                    .DistinctBy(s => s.Id);
                var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

                return _mappings.Map(series, paging);
            }
            else
            {
                return new PartialListVM(pageSize);
            }
        }

        public IPartialListVM GetPartialListForGenre(int genreId, int pageSize, int pageNo)
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
                var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

                return _mappings.Map(series, paging);
            }
            else
            {
                return new PartialListVM(pageSize);
            }
        }

        public void SelectBooks(int seriesId, int[] selectedIds)
        {
            var series = _queries.GetBookSeriesWithBooks(seriesId);

            if (series != null)
            {
                var booksToDelete = series.Books.Where(s => !selectedIds.Contains(s.Id)).ToList();
                foreach (var book in booksToDelete)
                {
                    series.Books.Remove(book);
                }

                foreach (var id in selectedIds)
                {
                    if (!series.Books.Any(b => b.Id == id))
                    {
                        var book = _queries.GetBook(id);
                        if (book != null)
                        {
                            series.Books.Add(book);
                        }
                    }
                }

                _commands.UpdateBookSeriesBooksRelation(series);
            }
        }

        public int Upsert(SeriesVM series)
        {
            if (series != null)
            {
                var entity = _mappings.Map<SeriesVM, BookSeries>(series);
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
    }
}