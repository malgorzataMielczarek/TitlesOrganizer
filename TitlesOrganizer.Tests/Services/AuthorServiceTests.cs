// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Services
{
    public class AuthorServiceTests
    {
        [Fact]
        public void Delete_ExistingAuthorId()
        {
            Author author = new Author() { Id = 1 };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            commandsRepo.Setup(r => r.Delete(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(author.Id);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(author), Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingAuthorId()
        {
            Author author = new Author() { Id = 1 };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns((Author?)null);
            commandsRepo.Setup(r => r.Delete(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(author.Id);

            queriesRepo.Verify(r => r.GetAuthor(author.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(author), Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectForBook_ExistingBookIdAndExistingAuthorsIds()
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
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(author2.Id)).Returns(author2);
            queriesRepo.Setup(r => r.GetAuthor(author3.Id)).Returns(author3);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, authorsIds);

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
        public void SelectForBook_ExistingBookIdAndNotExistingAuthorsIds()
        {
            Book book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { new Author() { Id = 1 } }
            };
            List<int> authorsIds = new List<int> { 2, 3 };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(2)).Returns((Author?)null);
            queriesRepo.Setup(r => r.GetAuthor(3)).Returns((Author?)null);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, authorsIds);

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
        public void SelectForBook_ExistingBookIdAndEmptyList()
        {
            Book book = new Book()
            {
                Id = 1,
                Title = "Title",
                Authors = new List<Author>() { new Author() { Id = 1 } }
            };
            List<int> authorsIds = new List<int>();
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(book.Id)).Returns(book);
            queriesRepo.Setup(r => r.GetAuthor(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(book));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(book.Id, authorsIds);

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
        public void SelectForBook_NotExistingBookIdAndAuthorsIds()
        {
            int bookId = 1;
            Author author = new Author { Id = 1 };
            List<int> authorsIds = new List<int> { author.Id };
            var commandsRepo = new Mock<IBookCommandsRepository>();
            var queriesRepo = new Mock<IBookQueriesRepository>();
            queriesRepo.Setup(r => r.GetBook(bookId)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            commandsRepo.Setup(r => r.UpdateBookAuthorsRelation(It.IsAny<Book>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new AuthorService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectForBook(bookId, authorsIds);

            queriesRepo.Verify(r => r.GetBook(bookId), Times.Once());
            queriesRepo.Verify(r => r.GetAuthor(It.IsAny<int>()), Times.Never());
            commandsRepo.Verify(r => r.UpdateBookAuthorsRelation(It.IsAny<Book>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void UpsertAuthor_NewAuthor_NewId()
        {
            AuthorVM author = new AuthorVM() { Name = "Name", LastName = "Last Name" };
            //var mapMock = Mock.Of<MappingExtensions>()
        }
    }
}