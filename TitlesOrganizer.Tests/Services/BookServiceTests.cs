// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Services
{
    public class BookServiceTests
    {
        [Fact]
        public void Delete_ExistingBookId()
        {
            Book book = new Book() { Id = 1, Title = "Title" };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
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
            var queriesRepo = new Mock<IBookQueriesRepository>();
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
        public void SelectForAuthor_ExistingAuthorIdAndExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var book2 = new Book() { Id = 2, Title = "Title2" };
            var book3 = new Book() { Id = 3, Title = "Title3" };
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { book2.Id, book3.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            queriesRepo.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForAuthor(author.Id, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book3.Id), Times.Once());
            author.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            commandsRepo.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == booksIds.Count &&
                    a.Books.Contains(book2) && a.Books.Contains(book3) && !a.Books.Contains(book1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForAuthor_ExistingAuthorIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            queriesRepo.Setup(r => r.GetBook(2)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3)).Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForAuthor(author.Id, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(2), Times.Once());
            queriesRepo.Verify(r => r.GetBook(3), Times.Once());
            author.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForAuthor_ExistingAuthorIdAndEmptyList()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var author = new Author()
            {
                Id = 1,
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForAuthor(author.Id, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(It.IsAny<int>()), Times.Never());
            author.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateAuthorBooksRelation
            (
                It.Is<Author>(a =>
                    a.Equals(author) &&
                    a.Books != null && a.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForAuthor_NotExistingAuthorIdAndBooksIds()
        {
            int authorId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(authorId)).Returns((Author?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            commandsRepo.Setup(r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForAuthor(authorId, booksIds);

            queriesRepo.Verify(r => r.GetAuthor(authorId), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateAuthorBooksRelation(It.IsAny<Author>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForGenre_ExistingLiteratureGenreIdAndExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var book2 = new Book() { Id = 2, Title = "Title2" };
            var book3 = new Book() { Id = 3, Title = "Title3" };
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { book2.Id, book3.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            queriesRepo.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForGenre(genre.Id, booksIds);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book3.Id), Times.Once());
            genre.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            commandsRepo.Verify(r => r.UpdateLiteratureGenreBooksRelation
            (
                It.Is<LiteratureGenre>(g =>
                    g.Equals(genre) &&
                    g.Books != null && g.Books.Count == booksIds.Count &&
                    g.Books.Contains(book2) && g.Books.Contains(book3) && !g.Books.Contains(book1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForGenre_ExistingLiteratureGenreIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            queriesRepo.Setup(r => r.GetBook(2)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3)).Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForGenre(genre.Id, booksIds);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(2), Times.Once());
            queriesRepo.Verify(r => r.GetBook(3), Times.Once());
            genre.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateLiteratureGenreBooksRelation
            (
                It.Is<LiteratureGenre>(g =>
                    g.Equals(genre) &&
                    g.Books != null && g.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForGenre_ExistingLiteratureGenreIdAndEmptyList()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForGenre(genre.Id, booksIds);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(It.IsAny<int>()), Times.Never());
            genre.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateLiteratureGenreBooksRelation
            (
                It.Is<LiteratureGenre>(g =>
                    g.Equals(genre) &&
                    g.Books != null && g.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForGenre_NotExistingLiteratureGenreIdAndBooksIds()
        {
            int genreId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genreId)).Returns((LiteratureGenre?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForGenre(genreId, booksIds);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genreId), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForSeries_ExistingBookSeriesIdAndExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var book2 = new Book() { Id = 2, Title = "Title2" };
            var book3 = new Book() { Id = 3, Title = "Title3" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { book2.Id, book3.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            queriesRepo.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForSeries(series.Id, booksIds);

            queriesRepo.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book2.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book3.Id), Times.Once());
            series.Books.Should().HaveCount(booksIds.Count).And.Contain(book2).And.Contain(book3).And.NotContain(book1);
            commandsRepo.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == booksIds.Count &&
                    s.Books.Contains(book2) && s.Books.Contains(book3) && !s.Books.Contains(book1))
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForSeries_ExistingBookSeriesIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            queriesRepo.Setup(r => r.GetBook(2)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3)).Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForSeries(series.Id, booksIds);

            queriesRepo.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(2), Times.Once());
            queriesRepo.Verify(r => r.GetBook(3), Times.Once());
            series.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForSeries_ExistingBookSeriesIdAndEmptyList()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var series = new BookSeries()
            {
                Id = 1,
                Title = "Title",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(series.Id)).Returns(series);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(series));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForSeries(series.Id, booksIds);

            queriesRepo.Verify(r => r.GetBookSeries(series.Id), Times.Once());
            queriesRepo.Verify(r => r.GetBook(It.IsAny<int>()), Times.Never());
            series.Books.Should().NotBeNull().And.BeEmpty();
            commandsRepo.Verify(r => r.UpdateBookSeriesBooksRelation
            (
                It.Is<BookSeries>(s =>
                    s.Equals(series) &&
                    s.Books != null && s.Books.Count == 0)
            ), Times.Once());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForSeries_NotExistingBookSeriesIdAndBooksIds()
        {
            int seriesId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBookSeries(seriesId)).Returns((BookSeries?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            commandsRepo.Setup(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForSeries(seriesId, booksIds);

            queriesRepo.Verify(r => r.GetBookSeries(seriesId), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookSeriesBooksRelation(It.IsAny<BookSeries>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }
    }
}