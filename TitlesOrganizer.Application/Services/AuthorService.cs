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
    public class AuthorService(IAuthorCommandsRepository _commands, IBookModuleQueriesRepository _queries, IBookVMsMappings _mappings) : IAuthorService
    {
        public void Delete(int id)
        {
            var author = _queries.GetAuthor(id);

            if (author != null)
            {
                _commands.Delete(author);
            }
        }

        public AuthorVM Get(int id, int bookPageSize, int bookPageNo)
        {
            var author = _queries.GetAuthorWithBooks(id);

            if (author != null)
            {
                var authorVM = _mappings.Map<Author, AuthorVM>(author);
                var paging = new Paging() { CurrentPage = bookPageNo, PageSize = bookPageSize };
                authorVM.Books = _mappings.Map(author.Books, paging);
                return authorVM;
            }
            else
            {
                return new AuthorVM()
                {
                    Books = new PartialListVM(bookPageSize)
                };
            }
        }

        public AuthorDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int seriesPageSize, int seriesPageNo, int genresPageSize, int genresPageNo)
        {
            var author = _queries.GetAuthor(id);

            if (author != null)
            {
                var result = _mappings.Map<Author, AuthorDetailsVM>(author);

                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries().Where(b => b.Authors.Select(a => a.Id).Contains(id)).ToList();
                var booksPaging = new Paging() { CurrentPage = booksPageNo, PageSize = booksPageSize };
                result.Books = _mappings.Map(books, booksPaging);

                var series = books.Where(b => b.Series != null).Select(b => b.Series!).DistinctBy(s => s.Id);
                var seriesPaging = new Paging() { PageSize = seriesPageSize, CurrentPage = seriesPageNo };
                result.Series = _mappings.Map(series, seriesPaging);

                var genres = books.SelectMany(b => b.Genres).DistinctBy(g => g.Id);
                var genresPaging = new Paging() { CurrentPage = genresPageNo, PageSize = genresPageSize };
                result.Genres = _mappings.Map(genres, genresPaging);

                return result;
            }
            else
            {
                return new AuthorDetailsVM
                {
                    Books = new PartialListVM(booksPageSize),
                    Series = new PartialListVM(seriesPageSize),
                    Genres = new PartialListVM(genresPageSize)
                };
            }
        }

        public IListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var authors = _queries.GetAllAuthors();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.Map(authors, paging, filtering);
        }

        public IDoubleListForItemVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var book = _queries.GetBook(bookId) ?? new Book() { Title = string.Empty };
            var authors = _queries.GetAllAuthorsWithBooks();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.MapToDoubleListForItem(authors, book, paging, filtering);
        }

        public IPartialListVM GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            var genre = _queries.GetLiteratureGenre(genreId);
            if (genre != null)
            {
                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                    .Where(b => b.Genres.Any(g => g.Id == genreId))
                    .ToList();
                var authors = books
                    .SelectMany(b => b.Authors)
                    .DistinctBy(a => a.Id);
                var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

                return _mappings.Map(authors, paging);
            }
            else
            {
                return new PartialListVM(pageSize);
            }
        }

        public void SelectBooks(int authorId, int[] booksIds)
        {
            var author = _queries.GetAuthorWithBooks(authorId);
            if (author != null)
            {
                var booksToRemove = author.Books.Where(b => !booksIds.Contains(b.Id)).ToList();
                foreach (var book in booksToRemove)
                {
                    author.Books.Remove(book);
                }

                foreach (var id in booksIds)
                {
                    if (!author.Books.Any(b => b.Id == id))
                    {
                        var book = _queries.GetBook(id);
                        if (book != null)
                        {
                            author.Books.Add(book);
                        }
                    }
                }

                _commands.UpdateAuthorBooksRelation(author);
            }
        }

        public int Upsert(AuthorVM author)
        {
            if (author != null)
            {
                var entity = _mappings.Map<AuthorVM, Author>(author);
                if (author.Id == default)
                {
                    return _commands.Insert(entity);
                }
                else
                {
                    _commands.Update(entity);
                    return author.Id;
                }
            }

            return -1;
        }
    }
}