// Ignore Spelling: Upsert

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Services
{
    public class LiteratureGenreServiceTests
    {
        [Fact]
        public void Delete_ExistingLiteratureGenreId()
        {
            LiteratureGenre genre = new LiteratureGenre() { Id = 1, Name = "Name" };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            commandsRepo.Setup(r => r.Delete(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(genre.Id);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(genre), Times.Once);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void Delete_NotExistingLiteratureGenreId()
        {
            LiteratureGenre genre = new LiteratureGenre() { Id = 1, Name = "Name" };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns((LiteratureGenre?)null);
            commandsRepo.Setup(r => r.Delete(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.Delete(genre.Id);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genre.Id), Times.Once);
            commandsRepo.Verify(r => r.Delete(genre), Times.Never);
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void SelectBooks_ExistingLiteratureGenreIdAndExistingBooksIds()
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
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            queriesRepo.Setup(r => r.GetBook(book2.Id)).Returns(book2);
            queriesRepo.Setup(r => r.GetBook(book3.Id)).Returns(book3);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(genre.Id, booksIds);

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
        public void SelectBooks_ExistingLiteratureGenreIdAndNotExistingBooksIds()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>() { 2, 3 };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            queriesRepo.Setup(r => r.GetBook(2)).Returns((Book?)null);
            queriesRepo.Setup(r => r.GetBook(3)).Returns((Book?)null);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(genre.Id, booksIds);

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
        public void SelectBooks_ExistingLiteratureGenreIdAndEmptyList()
        {
            var book1 = new Book() { Id = 1, Title = "Title1" };
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Name",
                Books = new List<Book>() { book1 }
            };
            List<int> booksIds = new List<int>();
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genre.Id)).Returns(genre);
            queriesRepo.Setup(r => r.GetBook(It.IsAny<int>()));
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(genre));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(genre.Id, booksIds);

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
        public void SelectBooks_NotExistingLiteratureGenreIdAndBooksIds()
        {
            int genreId = 1;
            var book1 = new Book() { Id = 1, Title = "Title1" };
            List<int> booksIds = new List<int>() { book1.Id };
            var commandsRepo = new Mock<ILiteratureGenreCommandsRepository>();
            var queriesRepo = new Mock<IBookModuleQueriesRepository>();
            queriesRepo.Setup(r => r.GetLiteratureGenre(genreId)).Returns((LiteratureGenre?)null);
            queriesRepo.Setup(r => r.GetBook(book1.Id)).Returns(book1);
            commandsRepo.Setup(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new LiteratureGenreService(commandsRepo.Object, queriesRepo.Object, mapper);
            service.SelectBooks(genreId, booksIds);

            queriesRepo.Verify(r => r.GetLiteratureGenre(genreId), Times.Once());
            queriesRepo.Verify(r => r.GetBook(book1.Id), Times.Never());
            commandsRepo.Verify(r => r.UpdateLiteratureGenreBooksRelation(It.IsAny<LiteratureGenre>()), Times.Never());
            commandsRepo.VerifyNoOtherCalls();
            queriesRepo.VerifyNoOtherCalls();
        }
    }
}