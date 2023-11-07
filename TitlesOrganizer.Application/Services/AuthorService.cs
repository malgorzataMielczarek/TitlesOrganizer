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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorCommandsRepository _commands;
        private readonly IBookModuleQueriesRepository _queries;

        private readonly IMapper _mapper;

        public AuthorService(IAuthorCommandsRepository authorCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper)
        {
            _commands = authorCommandsRepository;
            _queries = bookModuleQueriesRepository;
            _mapper = mapper;
        }

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
                return Map(author, bookPageSize, bookPageNo);
            }
            else
            {
                return new AuthorVM()
                {
                    Books = new PartialList<Book>()
                    {
                        Paging = new Paging(bookPageSize)
                    }
                };
            }
        }

        public AuthorDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int seriesPageSize, int seriesPageNo, int genresPageSize, int genresPageNo)
        {
            var author = _queries.GetAuthor(id);

            if (author != null)
            {
                var books = _queries.GetAllBooksWithAllRelatedObjects()
                    .Where(b => b.Authors.Any(a => a.Id == id));
                var series = books.Where(b => b.Series != null)
                    .Select(b => b.Series!).Distinct();
                var genres = books.SelectMany(b => b.Genres).Distinct();
                var authorDetails = MapToDetails(author);
                authorDetails = MapAuthorDetailsBooks(
                    authorDetails,
                    books,
                    booksPageSize, booksPageNo);
                authorDetails = MapAuthorDetailsSeries(
                    authorDetails,
                    series,
                    seriesPageSize, seriesPageNo);
                return MapAuthorDetailsGenres(
                    authorDetails,
                    genres,
                    genresPageSize, genresPageNo);
            }
            else
            {
                return new AuthorDetailsVM
                {
                    Books = new PartialList<Book>()
                    {
                        Paging = new Paging(booksPageSize)
                    },
                    Series = new PartialList<BookSeries>()
                    {
                        Paging = new Paging(seriesPageSize)
                    },
                    Genres = new PartialList<LiteratureGenre>()
                    {
                        Paging = new Paging(genresPageSize)
                    }
                };
            }
        }

        public ListAuthorForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var authors = _queries.GetAllAuthors();

            return MapToList(authors, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public ListAuthorForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString)
        {
            var book = _queries.GetBook(bookId) ?? new Book() { Title = string.Empty };
            var authors = _queries.GetAllAuthorsWithBooks();

            return MapForBook(authors, book, sortBy, pageSize, pageNo, searchString ?? string.Empty);
        }

        public PartialList<Author> GetPartialListForGenre(int genreId, int pageSize, int pageNo)
        {
            var genre = _queries.GetLiteratureGenreWithBooks(genreId);
            if (genre != null && genre.Books != null && genre.Books.Any())
            {
                var authors = _queries.GetAllAuthorsWithBooks()
                    .Where(a => a.Books.Any(b => genre.Books.Contains(b)));
                return MapToPartialList(authors, pageSize, pageNo);
            }
            else
            {
                return new PartialList<Author>()
                {
                    Paging = new Paging(pageSize)
                };
            }
        }

        public void SelectBooks(int authorId, List<int> booksIds)
        {
            var author = _queries.GetAuthor(authorId);
            if (author != null)
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

                author.Books = books;
                _commands.UpdateAuthorBooksRelation(author);
            }
        }

        public int Upsert(AuthorVM author)
        {
            if (author != null)
            {
                if (author.Id == default)
                {
                    return _commands.Insert(Map(author));
                }
                else
                {
                    _commands.Update(Map(author));
                    return author.Id;
                }
            }

            return -1;
        }

        protected virtual Author Map(AuthorVM author)
        {
            return author.MapToBase(_mapper);
        }

        protected virtual AuthorVM Map(Author author, int bookPageSize, int bookPageNo)
        {
            return author.MapFromBase(_mapper, new Paging() { CurrentPage = bookPageNo, PageSize = bookPageSize });
        }

        protected virtual AuthorDetailsVM MapToDetails(Author author)
        {
            return author.MapToDetails();
        }

        protected virtual AuthorDetailsVM MapAuthorDetailsBooks(AuthorDetailsVM authorDetails, IQueryable<Book> books, int pageSize, int pageNo)
        {
            authorDetails.Books = books.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return authorDetails;
        }

        protected virtual AuthorDetailsVM MapAuthorDetailsSeries(AuthorDetailsVM authorDetails, IQueryable<BookSeries> series, int pageSize, int pageNo)
        {
            authorDetails.Series = series.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return authorDetails;
        }

        protected virtual AuthorDetailsVM MapAuthorDetailsGenres(AuthorDetailsVM authorDetails, IQueryable<LiteratureGenre> genres, int pageSize, int pageNo)
        {
            authorDetails.Genres = genres.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });

            return authorDetails;
        }

        protected virtual ListAuthorForListVM MapToList(IQueryable<Author> authorList, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return authorList.MapToList(
                new Paging()
                {
                    PageSize = pageSize,
                    CurrentPage = pageNo
                },
                new Filtering()
                {
                    SortBy = sortBy,
                    SearchString = searchString
                });
        }

        protected virtual ListAuthorForBookVM MapForBook(IQueryable<Author> authorsWithBooks, Book book, SortByEnum sortBy, int pageSize, int pageNo, string searchString)
        {
            return authorsWithBooks.MapForItemToList(
                book,
                new Paging()
                {
                    CurrentPage = pageNo,
                    PageSize = pageNo
                },
                new Filtering()
                {
                    SearchString = searchString,
                    SortBy = sortBy
                });
        }

        protected virtual PartialList<Author> MapToPartialList(IQueryable<Author> authors, int pageSize, int pageNo)
        {
            return (PartialList<Author>)authors.MapToPartialList(new Paging()
            {
                CurrentPage = pageNo,
                PageSize = pageSize
            });
        }
    }
}