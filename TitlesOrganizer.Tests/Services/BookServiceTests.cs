// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.ViewModels.BookVMs;

namespace TitlesOrganizer.Tests.Services
{
    public class BookServiceTests
    {
        [Fact]
        public void Delete_ExistingBookId()
        {
            Book book = new Book() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            commandsRepo.Setup(r => r.Delete(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(book.Id);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(book), Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingBookId()
        {
            Book book = new Book() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns((Book?)null);
            commandsRepo.Setup(r => r.Delete(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(book.Id);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(book), Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Get_ExistingBookId()
        {
            Book book = Helpers.GetBook(1);
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookWithAllRelatedObjects(book.Id)).Returns(book);
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookServiceForTest(commandsRepo.Object, queriesRepo.Object, mapper);
            var result = service.Get(book.Id);

            queriesRepo.Verify(r => r.GetBookWithAllRelatedObjects(book.Id), Times.Once());
            queriesRepo.VerifyNoOtherCalls();
            commandsRepo.VerifyNoOtherCalls();
            result.Should().NotBeNull().And.BeOfType<BookVM>();
            result.Id.Should().Be(book.Id);
        }

        [Fact]
        public void SelectAuthors_ExistingBookIdAndExistingAuthorsIds()
        {
            Author author1 = new Author() { Id = 1 };
            Author author2 = new Author() { Id = 2 };
            Author author3 = new Author() { Id = 3 };
            Book book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { author1 }
            };
            List<int> authorsIds = new List<int> { author2.Id, author3.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(author2.Id)).Returns(author2);
            queriesRepo.Setup(r => r.GetAuthor(author3.Id)).Returns(author3);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectAuthors(book.Id, authorsIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(author2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(author3.Id), Times.Once());
            book.Authors.Should().HaveCount(authorsIds.Count);
            book.Authors.Should().Contain(author2).And.Contain(author3).And.NotContain(author1);
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == authorsIds.Count &&
                    b.Authors.Contains(author2) && b.Authors.Contains(author3) && !b.Authors.Contains(author1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthors_ExistingBookIdAndNotExistingAuthorsIds()
        {
            Book book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { new Author() { Id = 1 } }
            };
            List<int> authorsIds = new List<int> { 2, 3 };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(2)).Returns((Author?)null);
            queriesRepo.Setup(r => r.GetAuthor(3)).Returns((Author?)null);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectAuthors(book.Id, authorsIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(2), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(3), Times.Once());
            book.Authors.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthors_ExistingBookIdAndEmptyList()
        {
            Book book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { new Author() { Id = 1 } }
            };
            List<int> authorsIds = new List<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectAuthors(book.Id, authorsIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(It.IsAny<int>()), Times.Never());
            book.Authors.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Authors != null && b.Authors.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectAuthors_NotExistingBookIdAndAuthorsIds()
        {
            int bookId = 1;
            Author author = new Author { Id = 1 };
            List<int> authorsIds = new List<int> { author.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectAuthors(bookId, authorsIds);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(It.IsAny<int>()), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenres_ExistingBookIdAndExistingLiteratureGenresIds()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var genre2 = new LiteratureGenre() { Id = 2, Name = "Name2" };
            var genre3 = new LiteratureGenre() { Id = 3, Name = "Name3" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            List<int> genresIds = new List<int>() { genre2.Id, genre3.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre2.Id)).Returns(genre2);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre3.Id)).Returns(genre3);
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectGenres(book.Id, genresIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre3.Id), Times.Once());
            book.Genres.Should().HaveCount(genresIds.Count).And.Contain(genre2).And.Contain(genre3).And.NotContain(genre1);
            commandsRepo.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == genresIds.Count &&
                    b.Genres.Contains(genre2) && b.Genres.Contains(genre3) && !b.Genres.Contains(genre1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenres_ExistingBookIdAndNotExistingLiteratureGenresIds()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            List<int> genresIds = new List<int>() { 2, 3 };
            var commendsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(2)).Returns((LiteratureGenre?)null);
            queriesRepo.Setup(r => r.GetLiteratureGenre(3)).Returns((LiteratureGenre?)null);
            commendsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commendsRepo.Object, queriesRepo.Object, mapper);
            service.SelectGenres(book.Id, genresIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(2), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(3), Times.Once());
            book.Genres.Should().NotBeNull().And.BeEmpty();
            commendsRepo.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == 0)
            ), Times.Once());
            commendsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenres_ExistingBookIdAndEmptyList()
        {
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = new List<LiteratureGenre>() { genre1 }
            };
            List<int> genresIds = new List<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetLiteratureGenre(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectGenres(book.Id, genresIds);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(It.IsAny<int>()), Times.Never());
            book.Genres.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookGenresRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Genres != null && b.Genres.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectGenres_NotExistingBookIdAndLiteratureGenresIds()
        {
            int bookId = 1;
            var genre1 = new LiteratureGenre() { Id = 1, Name = "Name1" };
            List<int> genresIds = new List<int>() { genre1.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre1.Id)).Returns(genre1);
            commandsRepo.Setup(r => r.UpdateBookGenresRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectGenres(bookId, genresIds);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetLiteratureGenre(genre1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookGenresRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeries_ExistingBookIdAndExistingBookSeriesId()
        {
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var series2 = new BookSeries() { Id = 2, Title = "Title2" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series1,
                SeriesId = series1.Id
            };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(series2.Id)).Returns(series2);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectSeries(book.Id, series2.Id);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(series2.Id), Times.Once());
            book.Series.Should().Be(series2);
            book.SeriesId.Should().Be(series2.Id);
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == series2 &&
                    b.SeriesId == series2.Id)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeries_ExistingBookIdAndNotExistingBookSeriesId()
        {
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series1,
                SeriesId = series1.Id
            };
            int newSeriesId = 2;
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(newSeriesId)).Returns((BookSeries?)null);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectSeries(book.Id, newSeriesId);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(newSeriesId), Times.Once());
            book.Series.Should().BeNull();
            book.SeriesId.Should().BeNull();
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == null &&
                    b.SeriesId == null)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeries_ExistingBookIdAndNull()
        {
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Series = series1,
                SeriesId = series1.Id
            };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetBookSeries(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectSeries(book.Id, null);

            queriesRepo.Verify(r => r.GetBook(book.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(It.IsAny<int>()), Times.Never());
            book.Series.Should().BeNull();
            book.SeriesId.Should().BeNull();
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation
            (
                It.Is<Book>(b =>
                    b.Equals(book) &&
                    b.Series == null &&
                    b.SeriesId == null)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectSeries_NotExistingBookIdAndBookSeriesId()
        {
            int bookId = 1;
            var series1 = new BookSeries() { Id = 1, Title = "Title1" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBookSeries(series1.Id)).Returns(series1);
            commandsRepo.Setup(r => r.UpdateBookSeriesRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectSeries(bookId, series1.Id);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetBookSeries(series1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookSeriesRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }
    }

    public class BookServiceForTest : BookCommandsService
    {
        public BookServiceForTest(IBookCommandsRepository bookCommandsRepository, IBookModuleQueriesRepository bookModuleQueriesRepository, IMapper mapper) : base(bookCommandsRepository, bookModuleQueriesRepository, mapper)
        {
        }

        protected override Book Map(BookVM book)
        {
            return new Book()
            {
                Id = book.Id,
                Title = book.Title
            };
        }

        protected override BookVM Map(Book book)
        {
            return new BookVM()
            {
                Id = book.Id
            };
        }
    }
}