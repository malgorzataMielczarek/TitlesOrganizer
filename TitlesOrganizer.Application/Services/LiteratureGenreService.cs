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
                return Map(genre, booksPageSize, booksPageNo);
            }
            else
            {
                return new GenreVM()
                {
                    Name = string.Empty,
                    Books = new PartialList<Book>(booksPageSize)
                };
            }
        }

        public GenreDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo)
        {
            var genre = _queries.GetLiteratureGenre(id);

            if (genre != null)
            {
                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                    .Where(b => b.Genres.Any(g => g.Id == id))
                    .ToList();
                var authors = books.SelectMany(b => b.Authors)
                    .DistinctBy(a => a.Id);
                var series = books.Where(b => b.Series != null)
                    .Select(b => b.Series!)
                    .DistinctBy(s => s.Id);
                var genreDetails = MapToDetails(genre);
                genreDetails = MapGenreDetailsAuthors(genreDetails, authors, authorsPageSize, authorsPageNo);
                genreDetails = MapGenreDetailsBooks(genreDetails, books, booksPageSize, booksPageNo);
                return MapGenreDetailsSeries(genreDetails, series, seriesPageSize, seriesPageNo);
            }
            else
            {
                return new GenreDetailsVM()
                {
                    Authors = new PartialList<Author>(authorsPageSize),
                    Books = new PartialList<Book>(booksPageSize),
                    Series = new PartialList<BookSeries>(seriesPageSize)
                };
            }
        }

        public ListGenreForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var genres = _queries.GetAllLiteratureGenres();

            return MapToList(genres, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public ListGenreForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var genres = _queries.GetAllLiteratureGenresWithBooks();
            var book = _queries.GetBook(bookId) ?? new Book() { Title = string.Empty };

            return MapForBook(genres, book, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public PartialList<LiteratureGenre> GetPartialListForAuthor(int authorId, int pageSize, int pageNo)
        {
            var author = _queries.GetAuthor(authorId);

            if (author != null)
            {
                var books = _queries.GetAllBooksWithAuthorsGenresAndSeries()
                .Where(b => b.Authors.Any(a => a.Id == authorId))
                .ToList();
                var genres = books
                .SelectMany(b => b.Genres)
                .DistinctBy(g => g.Id);

                return MapToPartialList(genres, pageSize, pageNo);
            }
            else
            {
                return new PartialList<LiteratureGenre>(pageSize);
            }
        }

        public void SelectBooks(int genreId, List<int> booksIds)
        {
            var genre = _queries.GetLiteratureGenre(genreId);

            if (genre != null)
            {
                var books = new List<Book>();
                foreach (var id in booksIds)
                {
                    var book = _queries.GetBook(id);
                    if (book != null)
                    {
                        books.Add(book);
                    }
                }

                genre.Books = books;
                _commands.UpdateLiteratureGenreBooksRelation(genre);
            }
        }

        public int Upsert(GenreVM genre)
        {
            var entity = Map(genre);

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

        protected virtual LiteratureGenre Map(GenreVM genre)
        {
            return genre.MapToBase(_mapper);
        }

        protected virtual GenreVM Map(LiteratureGenre genreWithBooks, int booksPageSize, int booksPageNo)
        {
            return genreWithBooks.MapFromBase(
                _mapper,
                new Paging()
                {
                    CurrentPage = booksPageNo,
                    PageSize = booksPageSize
                });
        }

        protected virtual GenreDetailsVM MapToDetails(LiteratureGenre genre)
        {
            return genre.MapToDetails();
        }

        protected virtual GenreDetailsVM MapGenreDetailsAuthors(GenreDetailsVM genreDetails, IEnumerable<Author> authors, int pageSize, int pageNo)
        {
            genreDetails.Authors = authors.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return genreDetails;
        }

        protected virtual GenreDetailsVM MapGenreDetailsBooks(GenreDetailsVM genreDetails, IEnumerable<Book> books, int pageSize, int pageNo)
        {
            genreDetails.Books = books.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return genreDetails;
        }

        protected virtual GenreDetailsVM MapGenreDetailsSeries(GenreDetailsVM genreDetails, IEnumerable<BookSeries> series, int pageSize, int pageNo)
        {
            genreDetails.Series = series.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return genreDetails;
        }

        protected virtual ListGenreForListVM MapToList(IQueryable<LiteratureGenre> genres, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return genres.MapToList(
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

        protected virtual ListGenreForBookVM MapForBook(IQueryable<LiteratureGenre> genresWithBooks, Book book, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return genresWithBooks.MapForItemToList(
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

        protected virtual PartialList<LiteratureGenre> MapToPartialList(IEnumerable<LiteratureGenre> genres, int pageSize, int pageNo)
        {
            return (PartialList<LiteratureGenre>)genres.MapToPartialList(
                new Paging() { CurrentPage = pageNo, PageSize = pageSize });
        }
    }
}