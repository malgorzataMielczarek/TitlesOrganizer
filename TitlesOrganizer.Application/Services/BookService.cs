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
    public class BookService(IBookCommandsRepository _commands, IBookModuleQueriesRepository _queries, ILanguageRepository _language, IBookVMsMappings _mappings)
        : IBookService
    {
        public void Delete(int id)
        {
            var book = _queries.GetBook(id);

            if (book != null)
            {
                _commands.Delete(book);
            }
        }

        public BookVM Get(int id)
        {
            var book = _queries.GetBookWithAuthorsGenresAndSeries(id);

            if (book != null)
            {
                var bookVM = _mappings.Map<Book, BookVM>(book);
                bookVM.Authors = _mappings.Map(book.Authors);
                bookVM.Genres = _mappings.Map(book.Genres);
                if (book.Series != null)
                {
                    bookVM.Series = _mappings.Map(book.Series);
                }

                return bookVM;
            }
            else
            {
                return new BookVM();
            }
        }

        public BookDetailsVM GetDetails(int id)
        {
            var book = _queries.GetBookWithAuthorsGenresAndSeries(id);

            if (book != null)
            {
                var bookDetails = _mappings.Map<Book, BookDetailsVM>(book);
                bookDetails.Authors = _mappings.Map(book.Authors);
                bookDetails.Genres = _mappings.Map(book.Genres);
                if (book.OriginalLanguageCode != null)
                {
                    bookDetails.OriginalLanguage = _language.GetAllLanguages().FirstOrDefault(l => l.Code == book.OriginalLanguageCode)?.Name ?? string.Empty;
                }

                if (book.Series != null && book.SeriesId.HasValue)
                {
                    bookDetails.Series = _mappings.Map(book.Series);
                    int booksInSeries = _queries.GetBookSeriesWithBooks(book.SeriesId.Value)!.Books.Count;
                    bookDetails.InSeries = InSeries(book.NumberInSeries, booksInSeries);
                }

                return bookDetails;
            }
            else
            {
                return new BookDetailsVM();
            }
        }

        public IListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var books = _queries.GetAllBooks();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SearchString = searchString ?? string.Empty, SortBy = sortBy };

            return _mappings.Map(books, paging, filtering);
        }

        public IDoubleListForItemVM GetListForAuthor(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var books = _queries.GetAllBooksWithAuthorsGenresAndSeries();
            var author = _queries.GetAuthor(authorId) ?? new Author();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.MapToDoubleListForItem(books, author, paging, filtering);
        }

        public IDoubleListForItemVM GetListForGenre(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var books = _queries.GetAllBooksWithAuthorsGenresAndSeries();
            var genre = _queries.GetLiteratureGenre(genreId) ?? new LiteratureGenre() { Name = string.Empty };
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.MapToDoubleListForItem(books, genre, paging, filtering);
        }

        public IDoubleListForItemVM GetListForSeries(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var books = _queries.GetAllBooks();
            var series = _queries.GetBookSeries(seriesId) ?? new BookSeries() { Title = string.Empty };
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = sortBy, SearchString = searchString ?? string.Empty };

            return _mappings.MapToDoubleListForItem(books, series, paging, filtering);
        }

        public IPartialListVM GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            var author = _queries.GetAuthorWithBooks(authorId);

            if (author == null)
            {
                return new PartialListVM(pageSize);
            }
            else
            {
                var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
                return _mappings.Map(author.Books, paging);
            }
        }

        public IPartialListVM GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            var genre = _queries.GetLiteratureGenreWithBooks(genreId);

            if (genre == null || genre.Books == null)
            {
                return new PartialListVM(pageSize); ;
            }
            else
            {
                var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
                return _mappings.Map(genre.Books, paging);
            }
        }

        public IPartialListVM GetPartialListForSeries(int seriesId, int pageSize, int pageNo)
        {
            var series = _queries.GetBookSeriesWithBooks(seriesId);

            if (series == null || series.Books == null)
            {
                return new PartialListVM(pageSize); ;
            }
            else
            {
                var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
                return _mappings.Map(series.Books, paging);
            }
        }

        public void SelectAuthors(int bookId, int[] selectedIds)
        {
            var book = _queries.GetBookWithAuthorsGenresAndSeries(bookId);

            if (book != null)
            {
                var authorsToRemove = book.Authors.Where(a => !selectedIds.Contains(a.Id)).ToList();
                foreach (var author in authorsToRemove)
                {
                    book.Authors.Remove(author);
                }

                foreach (var id in selectedIds)
                {
                    if (!book.Authors.Any(a => a.Id == id))
                    {
                        var author = _queries.GetAuthor(id);
                        if (author != null)
                        {
                            book.Authors.Add(author);
                        }
                    }
                }

                _commands.UpdateBookAuthorsRelation(book);
            }
        }

        public void SelectGenres(int bookId, int[] selectedIds)
        {
            var book = _queries.GetBookWithAuthorsGenresAndSeries(bookId);
            var isModified = false;
            if (book != null)
            {
                var genresToRemove = book.Genres.Where(g => !selectedIds.Contains(g.Id)).ToList();
                foreach (var genre in genresToRemove)
                {
                    book.Genres.Remove(genre);
                    isModified = true;
                }

                foreach (var id in selectedIds)
                {
                    if (!book.Genres.Any(g => g.Id == id))
                    {
                        var genre = _queries.GetLiteratureGenre(id);
                        if (genre != null)
                        {
                            book.Genres.Add(genre);
                            isModified = true;
                        }
                    }
                }

                if (isModified)
                {
                    _commands.UpdateBookGenresRelation(book);
                }
            }
        }

        public void SelectSeries(int bookId, int? selectedId)
        {
            var book = _queries.GetBook(bookId);

            if (book != null)
            {
                BookSeries? series = null;
                if (selectedId.HasValue)
                {
                    series = _queries.GetBookSeries(selectedId.Value);
                }

                book.Series = series;
                book.SeriesId = series?.Id;
                _commands.UpdateBookSeriesRelation(book);
            }
        }

        public int Upsert(BookVM book)
        {
            var entity = _mappings.Map<BookVM, Book>(book);

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

        private string InSeries(int? numberInSeries, int? booksInSeries)
        {
            var result = new System.Text.StringBuilder();
            if (numberInSeries.HasValue)
            {
                result.Append(numberInSeries);
                if (booksInSeries.HasValue)
                {
                    result.Append(" of ");
                    result.Append(booksInSeries);
                }

                result.Append(" in ");
                return result.ToString();
            }
            else
            {
                return "Part of ";
            }
        }
    }
}