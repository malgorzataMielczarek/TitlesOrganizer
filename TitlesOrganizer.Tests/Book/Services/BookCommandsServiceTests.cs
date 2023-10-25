using AutoMapper;
using Moq;
using TitlesOrganizer.Application.Services;
using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Book.Services
{
    public class BookCommandsServiceTests
    {
        [Fact]
        public void DeleteAuthor_ExistingAuthorId()
        {
            Author author = new Author() { Id = 1 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetAuthor(author.Id)).Returns(author);
            repositoryMock.Setup(r => r.Delete(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteAuthor(author.Id);

            repositoryMock.Verify(r => r.GetAuthor(author.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(author), Times.Once);
            repositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void DeleteAuthor_NotExistingAuthorId()
        {
            Author author = new Author() { Id = 1 };
            var repositoryMock = new Mock<IBookCommandsRepository>();
            repositoryMock.Setup(r => r.GetAuthor(author.Id)).Returns((Author?)null);
            repositoryMock.Setup(r => r.Delete(author));
            IMapper mapper = new Mock<IMapper>().Object;

            var service = new BookCommandsService(repositoryMock.Object, mapper);
            service.DeleteAuthor(author.Id);

            repositoryMock.Verify(r => r.GetAuthor(author.Id), Times.Once);
            repositoryMock.Verify(r => r.Delete(author), Times.Never);
            repositoryMock.VerifyNoOtherCalls();
        }
    }
}