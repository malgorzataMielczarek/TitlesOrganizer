using AutoMapper;
using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs.CommandVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Book.ViewModels.CommandVMs
{
    public class MappingExtensionsTests
    {
        [Fact]
        public void MapToBase_AuthorVM_Author()
        {
            var books = new List<BookForListVM>()
            {
                new BookForListVM()
                {
                    Id = 1,
                    Title = "Title1"
                },
                new BookForListVM()
                {
                    Id = 2,
                    Title = "Title2"
                }
            };
            var authorVM = new AuthorVM()
            {
                Id = 1,
                Name = "Author Name",
                LastName = "Author Last Name",
                Books = books,
                BooksPaging = new Application.ViewModels.Helpers.Paging() { Count = 2, CurrentPage = 1, PageSize = 1 }
            };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = authorVM.MapToBase(mapper);

            result.Should().NotBeNull().And.BeOfType<Author>();
            result.Id.Should().Be(authorVM.Id);
            result.Name.Should().Be(authorVM.Name);
            result.LastName.Should().Be(authorVM.LastName);
            result.Books.Should().BeNullOrEmpty();
        }

        [Fact]
        public void MapFromBase_AuthorWithBooks_AuthorVM()
        {
            int count = 3, currentPage = 1, pageSize = 2, pageCount = 2;
            var author = GetAuthor();
            author.Books = GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, paging);

            result.Should().NotBeNull().And.BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
            result.Name.Should().Be(author.Name);
            result.LastName.Should().Be(author.LastName);
            result.BooksPaging.Should().Be(paging);
            result.BooksPaging.CurrentPage.Should().Be(currentPage);
            result.BooksPaging.PageSize.Should().Be(pageSize);
            result.BooksPaging.Count.Should().Be(count);
            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Theory]
        [InlineData(1, 1, "Title1", 2, "Title2")]
        [InlineData(3, 5, "Title5", 6, "Title6")]
        public void MapFromBase_AuthorWithBooks_AuthorVMWithProperBooks(int currentPage, int firstBookId, string firstBookTitle, int secondBookId, string secondBookTitle)
        {
            int count = 9, pageSize = 2, pageCount = 2;
            var author = GetAuthor();
            author.Books = GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, paging);

            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
            result.Books[0].Id.Should().Be(firstBookId);
            result.Books[0].Title.Should().Be(firstBookTitle);
            result.Books[1].Id.Should().Be(secondBookId);
            result.Books[1].Title.Should().Be(secondBookTitle);
        }

        [Fact]
        public void MapFromBase_AuthorWithBooksLastPage_AuthorVMWithProperNumberOfBooks()
        {
            int count = 9, currentPage = 5, pageSize = 2, pageCount = 1;
            var author = GetAuthor();
            author.Books = GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, paging);

            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapFromBase_AuthorAndBooks_AuthorVM()
        {
            int count = 3, currentPage = 1, pageSize = 2, pageCount = 2;
            var author = GetAuthor();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, GetBooksList(count).AsQueryable(), paging);

            result.Should().NotBeNull().And.BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
            result.Name.Should().Be(author.Name);
            result.LastName.Should().Be(author.LastName);
            result.BooksPaging.Should().Be(paging);
            result.BooksPaging.CurrentPage.Should().Be(currentPage);
            result.BooksPaging.PageSize.Should().Be(pageSize);
            result.BooksPaging.Count.Should().Be(count);
            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapFromBase_AuthorAndNullBooks_AuthorVM()
        {
            int count = 3, currentPage = 1, pageSize = 2, pageCount = 2;
            var author = GetAuthor();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, null, paging);

            result.Should().NotBeNull().And.BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
            result.Name.Should().Be(author.Name);
            result.LastName.Should().Be(author.LastName);
            result.BooksPaging.Should().Be(paging);
            result.BooksPaging.CurrentPage.Should().Be(currentPage);
            result.BooksPaging.PageSize.Should().Be(pageSize);
            result.BooksPaging.Count.Should().Be(0);
            result.Books.Should().NotBeNull().And.BeEmpty();
        }

        [Theory]
        [InlineData(1, 1, "Title1", 2, "Title2")]
        [InlineData(3, 5, "Title5", 6, "Title6")]
        public void MapFromBase_AuthorAndBooks_AuthorVMWithProperBooks(int currentPage, int firstBookId, string firstBookTitle, int secondBookId, string secondBookTitle)
        {
            int count = 9, pageSize = 2, pageCount = 2;
            var author = GetAuthor();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, GetBooksList(count).AsQueryable(), paging);

            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
            result.Books[0].Id.Should().Be(firstBookId);
            result.Books[0].Title.Should().Be(firstBookTitle);
            result.Books[1].Id.Should().Be(secondBookId);
            result.Books[1].Title.Should().Be(secondBookTitle);
        }

        [Fact]
        public void MapFromBase_AuthorAndBooksLastPage_AuthorVMWithProperNumberOfBooks()
        {
            int count = 9, currentPage = 5, pageSize = 2, pageCount = 1;
            var author = GetAuthor();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, GetBooksList(count).AsQueryable(), paging);

            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapToBase_BookVM_Book()
        {
            var authors = new List<AuthorForListVM>()
            {
                new AuthorForListVM()
                {
                    Id = 1,
                    FullName = "Author1"
                },
                new AuthorForListVM()
                {
                    Id = 2,
                    FullName = "Author2"
                }
            };
            var genres = new List<GenreForListVM>()
            {
                new GenreForListVM()
                {
                    Id = 1,
                    Name = "Genre1"
                },
                new GenreForListVM()
                {
                    Id = 2,
                    Name = "Genre2"
                }
            };
            var bookVM = new BookVM()
            {
                Id = 1,
                Title = "Book Title",
                Authors = authors,
                Description = "Description",
                Edition = "I",
                Genres = genres,
                NumberInSeries = 1,
                OriginalLanguageCode = "ENG",
                OriginalTitle = "Book Original Title",
                Series = new SeriesForListVM() { Id = 1, Title = "Series1" },
                Year = 2001
            };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<BookMappings>());
            IMapper mapper = config.CreateMapper();

            var result = bookVM.MapToBase(mapper);

            result.Should().NotBeNull().And.BeOfType<Domain.Models.Book>();
            result.Id.Should().Be(bookVM.Id);
            result.Title.Should().Be(bookVM.Title);
            result.OriginalTitle.Should().Be(bookVM.OriginalTitle);
            result.OriginalLanguageCode.Should().Be(bookVM.OriginalLanguageCode);
            result.Year.Should().Be(bookVM.Year);
            result.Edition.Should().Be(bookVM.Edition);
            result.Description.Should().Be(bookVM.Description);
            result.Authors.Should().BeNullOrEmpty();
            result.Genres.Should().BeNullOrEmpty();
            result.BookSeries.Should().BeNull();
            result.NumberInSeries.Should().Be(bookVM.NumberInSeries);
        }

        [Fact]
        public void MapFromBase_BookWithAllRelatedObjects_BookVM()
        {
            int countOfAuthors = 3, countOfGenres = 5;
            var book = GetBook();
            book.Authors = GetAuthorsList(countOfAuthors);
            book.Genres = GetGenresList(countOfGenres);
            book.BookSeries = GetSeries();
            book.BookSeriesId = book.BookSeries.Id;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BookMappings>());
            IMapper mapper = config.CreateMapper();

            var result = book.MapFromBase(mapper);

            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().Be(book.Title);
            result.OriginalTitle.Should().Be(book.OriginalTitle);
            result.OriginalLanguageCode.Should().Be(book.OriginalLanguageCode);
            result.Year.Should().Be(book.Year);
            result.Edition.Should().Be(book.Edition);
            result.Description.Should().Be(book.Description);
            result.Authors.Should().NotBeNull().And.HaveCount(countOfAuthors);
            result.Genres.Should().NotBeNull().And.HaveCount(countOfGenres);
            result.Series.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.NumberInSeries.Should().Be(book.NumberInSeries);
        }

        [Theory]
        [InlineData(3, 5)]
        [InlineData(5, 0)]
        [InlineData(0, 7)]
        [InlineData(0, 0)]
        public void MapFromBase_BookAndAuthorsAndLiteratureGenresAndBookSeries_BookVM(int countOfAuthors, int countOfGenres)
        {
            var book = GetBook();
            var authors = GetAuthorsList(countOfAuthors).AsQueryable();
            var genres = GetGenresList(countOfGenres).AsQueryable();
            var series = GetSeries();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BookMappings>());
            IMapper mapper = config.CreateMapper();

            var result = book.MapFromBase(mapper, authors, genres, series);

            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().Be(book.Title);
            result.OriginalTitle.Should().Be(book.OriginalTitle);
            result.OriginalLanguageCode.Should().Be(book.OriginalLanguageCode);
            result.Year.Should().Be(book.Year);
            result.Edition.Should().Be(book.Edition);
            result.Description.Should().Be(book.Description);
            result.Authors.Should().NotBeNull().And.HaveCount(countOfAuthors);
            result.Genres.Should().NotBeNull().And.HaveCount(countOfGenres);
            result.Series.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.NumberInSeries.Should().Be(book.NumberInSeries);
        }

        [Fact]
        public void MapFromBase_BookAndNullAuthorsAndNullLiteratureGenresAndNullBookSeries_BookVM()
        {
            int countOfAuthors = 3, countOfGenres = 5;
            var book = GetBook();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<BookMappings>());
            IMapper mapper = config.CreateMapper();

            var result = book.MapFromBase(mapper, (IQueryable<Author>)null, (IQueryable<LiteratureGenre>)null, (BookSeries)null);

            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().Be(book.Title);
            result.OriginalTitle.Should().Be(book.OriginalTitle);
            result.OriginalLanguageCode.Should().Be(book.OriginalLanguageCode);
            result.Year.Should().Be(book.Year);
            result.Edition.Should().Be(book.Edition);
            result.Description.Should().Be(book.Description);
            result.Authors.Should().NotBeNull().And.BeEmpty();
            result.Genres.Should().NotBeNull().And.BeEmpty();
            result.Series.Should().BeNull();
            result.NumberInSeries.Should().Be(book.NumberInSeries);
        }

        private List<Domain.Models.Book> GetBooksList(int count)
        {
            var books = new List<Domain.Models.Book>();
            for (int i = 1; i <= count; i++)
            {
                books.Add(GetBook(i));
            }

            return books;
        }

        private Domain.Models.Book GetBook(int index = 1)
        {
            return new Domain.Models.Book()
            {
                Id = index,
                Title = "Title" + index,
                OriginalTitle = "Original Title" + index,
                OriginalLanguageCode = "ENG",
                Description = "Description" + index,
                Edition = "I",
                Year = 2001,
                NumberInSeries = index
            };
        }

        private List<Author> GetAuthorsList(int count)
        {
            var authors = new List<Author>();
            for (int i = 1; i <= count; i++)
            {
                authors.Add(GetAuthor(i));
            }

            return authors;
        }

        private Author GetAuthor(int index = 1)
        {
            return new Author()
            {
                Id = index,
                Name = "Name" + index,
                LastName = "Last Name" + index
            };
        }

        private List<LiteratureGenre> GetGenresList(int count)
        {
            var genres = new List<LiteratureGenre>();
            for (int i = 1; i <= count; i++)
            {
                genres.Add(GetGenre(i));
            }

            return genres;
        }

        private LiteratureGenre GetGenre(int index = 1)
        {
            return new LiteratureGenre()
            {
                Id = index,
                Name = "Name" + index
            };
        }

        private List<BookSeries> GetSeriesList(int count)
        {
            var seriesList = new List<BookSeries>();
            for (int i = 1; i <= count; i++)
            {
                seriesList.Add(GetSeries(i));
            }

            return seriesList;
        }

        private BookSeries GetSeries(int index = 1)
        {
            return new BookSeries()
            {
                Id = index,
                Title = "Title" + index,
                OriginalTitle = "Original Title" + index,
                Description = "Description" + index
            };
        }
    }
}