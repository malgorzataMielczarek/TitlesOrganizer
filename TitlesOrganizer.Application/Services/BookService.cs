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
    public class BookService : IBookService
    {
        private readonly IBookCommandsRepository _commands;
        private readonly IBookModuleQueriesRepository _queries;
        private readonly ILanguageRepository _language;

        private readonly IMapper _mapper;

        public BookService(IBookCommandsRepository bookCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, ILanguageRepository languageRepository, IMapper mapper)
        {
            _commands = bookCommandsRepository;
            _queries = bookModuleQueriesRepository;
            _mapper = mapper;
            _language = languageRepository;
        }

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
                return Map(book);
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
                var authors = book.Authors;
                var genres = book.Genres;
                BookSeries? series = book.Series;
                Language? language = null;
                IEnumerable<Book>? booksInSeries = null;
                if (book.OriginalLanguageCode != null)
                {
                    language = _language.GetAllLanguages().FirstOrDefault(l => l.Code == book.OriginalLanguageCode);
                }

                if (series != null)
                {
                    booksInSeries = _queries.GetBookSeriesWithBooks(book.SeriesId!.Value)!.Books;
                }

                return MapToDetails(book, language, authors, genres, series, booksInSeries);
            }
            else
            {
                return new BookDetailsVM();
            }
        }

        public ListBookForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var books = _queries.GetAllBooks();

            return MapToList(books, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public ListBookForAuthorVM GetListForAuthor(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var books = _queries.GetAllBooksWithAuthorsGenresAndSeries();
            var author = _queries.GetAuthor(authorId) ?? new Author();

            return MapForAuthor(books, author, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public ListBookForGenreVM GetListForGenre(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var books = _queries.GetAllBooksWithAuthorsGenresAndSeries();
            var genre = _queries.GetLiteratureGenre(genreId) ?? new LiteratureGenre() { Name = string.Empty };

            return MapForGenre(books, genre, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public ListBookForSeriesVM GetListForSeries(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var books = _queries.GetAllBooks();
            var series = _queries.GetBookSeries(seriesId) ?? new BookSeries() { Title = string.Empty };

            return MapForSeries(books, series, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public PartialList<Book> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            var author = _queries.GetAuthorWithBooks(authorId);

            if (author == null)
            {
                return new PartialList<Book>(pageSize);
            }
            else
            {
                return MapToPartialList(author.Books, pageSize, pageNo);
            }
        }

        public PartialList<Book> GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            var genre = _queries.GetLiteratureGenreWithBooks(genreId);

            if (genre == null || genre.Books == null)
            {
                return new PartialList<Book>(pageSize); ;
            }
            else
            {
                return MapToPartialList(genre.Books, pageSize, pageNo);
            }
        }

        public PartialList<Book> GetPartialListForSeries(int seriesId, int pageSize, int pageNo)
        {
            var series = _queries.GetBookSeriesWithBooks(seriesId);

            if (series == null || series.Books == null)
            {
                return new PartialList<Book>(pageSize); ;
            }
            else
            {
                return MapToPartialList(series.Books, pageSize, pageNo);
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
            var entity = Map(book);

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

        protected virtual Book Map(BookVM book)
        {
            return book.MapToBase(_mapper);
        }

        protected virtual BookVM Map(Book bookWithRelatedObjects)
        {
            return bookWithRelatedObjects.MapFromBase(_mapper);
        }

        protected virtual BookDetailsVM MapToDetails(Book book, Language? language, IEnumerable<Author> authors, IEnumerable<LiteratureGenre> genres, BookSeries? series, IEnumerable<Book>? booksInSeries)
        {
            return book.MapToDetails(language, authors, genres, series, booksInSeries);
        }

        protected virtual ListBookForListVM MapToList(IQueryable<Book> books, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return books.MapToList(
                new Paging() { CurrentPage = pageNo, PageSize = pageSize },
                new Filtering() { SortBy = sortBy, SearchString = searchString }
                );
        }

        protected virtual ListBookForAuthorVM MapForAuthor(IQueryable<Book> books, Author author, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return books.MapForItemToList(
                author,
                new Paging() { CurrentPage = pageNo, PageSize = pageSize },
                new Filtering() { SortBy = sortBy, SearchString = searchString }
                );
        }

        protected virtual ListBookForGenreVM MapForGenre(IQueryable<Book> books, LiteratureGenre genre, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return books.MapForItemToList(
                genre,
                new Paging() { CurrentPage = pageNo, PageSize = pageSize },
                new Filtering() { SortBy = sortBy, SearchString = searchString }
                );
        }

        protected virtual ListBookForSeriesVM MapForSeries(IQueryable<Book> books, BookSeries series, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return books.MapForItemToList(
                series,
                new Paging() { CurrentPage = pageNo, PageSize = pageSize },
                new Filtering() { SortBy = sortBy, SearchString = searchString }
                );
        }

        protected virtual PartialList<Book> MapToPartialList(ICollection<Book> books, int pageSize, int pageNo)
        {
            return (PartialList<Book>)books.AsQueryable().MapToPartialList(
                new Paging()
                {
                    CurrentPage = pageNo,
                    PageSize = pageSize
                });
        }
    }
}