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
    public class LiteratureGenreService(ILiteratureGenreCommandsRepository _commands, IBookModuleQueriesRepository _queries, IBookVMsMappings _mappings)
        : ILiteratureGenreService
    {
        public void Delete(int id)
        {
            var genre = _queries.GetLiteratureGenre(id);

            if (genre != null)
            {
                _commands.Delete(genre);
            }
        }

        public GenreVM Get(int id, int booksPageSize, int booksPageNo)
        {
            var genre = _queries.GetLiteratureGenreWithBooks(id);

            if (genre != null)
            {
                var genreVM = _mappings.Map<LiteratureGenre, GenreVM>(genre);
                if (genre.Books != null)
                {
                    var paging = new Paging() { CurrentPage = booksPageNo, PageSize = booksPageSize };
                    genreVM.Books = _mappings.Map(genre.Books, paging);
                }
                else
                {
                    genreVM.Books = new PartialListVM(booksPageSize);
                }

                return genreVM;
            }
            else
            {
                return new GenreVM()
                {
                    Name = string.Empty,
                    Books = new PartialListVM(booksPageSize)
                };
            }
        }

        public GenreDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo)
        {
            var genre = _queries.GetLiteratureGenre(id);

            if (genre != null)
            {
                var result = _mappings.Map<LiteratureGenre, GenreDetailsVM>(genre);

                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                    .Where(b => b.Genres.Any(g => g.Id == id))
                    .ToList();
                var booksPaging = new Paging() { CurrentPage = booksPageNo, PageSize = booksPageSize };
                result.Books = _mappings.Map(books, booksPaging);

                var authors = books.SelectMany(b => b.Creators)
                    .DistinctBy(a => a.Id);
                var authorsPaging = new Paging() { CurrentPage = authorsPageNo, PageSize = authorsPageSize };
                result.Authors = _mappings.Map(authors, authorsPaging);

                var series = books.Where(b => b.Series != null)
                    .Select(b => b.Series!)
                    .DistinctBy(s => s.Id);
                var seriesPaging = new Paging() { CurrentPage = seriesPageNo, PageSize = seriesPageSize };
                result.Series = _mappings.Map(series, seriesPaging);

                return result;
            }
            else
            {
                return new GenreDetailsVM()
                {
                    Authors = new PartialListVM(authorsPageSize),
                    Books = new PartialListVM(booksPageSize),
                    Series = new PartialListVM(seriesPageSize)
                };
            }
        }

        public IListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var genres = _queries.GetAllLiteratureGenres();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.Map(genres, paging, filtering);
        }

        public IDoubleListForItemVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var genres = _queries.GetAllLiteratureGenresWithBooks();
            var book = _queries.GetBook(bookId) ?? new Book() { Title = string.Empty };
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.MapToDoubleListForItem(genres, book, paging, filtering);
        }

        public IPartialListVM GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            var author = _queries.GetAuthor(authorId);

            if (author != null)
            {
                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                .Where(b => b.Creators.Any(a => a.Id == authorId))
                .ToList();
                var genres = books
                .SelectMany(b => b.Genres)
                .DistinctBy(g => g.Id);
                var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

                return _mappings.Map(genres, paging);
            }
            else
            {
                return new PartialListVM(pageSize);
            }
        }

        public void SelectBooks(int genreId, int[] booksIds)
        {
            var genre = _queries.GetLiteratureGenreWithBooks(genreId);
            if (genre != null)
            {
                if (genre.Books == null)
                {
                    genre.Books = new List<Book>();
                }
                else
                {
                    var booksToRemove = genre.Books.Where(b => !booksIds.Contains(b.Id)).ToList();
                    foreach (var book in booksToRemove)
                    {
                        genre.Books.Remove(book);
                    }
                }

                foreach (var id in booksIds)
                {
                    if (!genre.Books.Any(b => b.Id == id))
                    {
                        var book = _queries.GetBook(id);
                        if (book != null)
                        {
                            genre.Books.Add(book);
                        }
                    }
                }

                _commands.UpdateLiteratureGenreBooksRelation(genre);
            }
        }

        public int Upsert(GenreVM genre)
        {
            if (genre != null)
            {
                var entity = _mappings.Map<GenreVM, LiteratureGenre>(genre);
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