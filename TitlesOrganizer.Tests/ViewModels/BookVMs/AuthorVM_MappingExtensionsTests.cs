using AutoMapper;
using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class AuthorVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToBase_AuthorVM_Author()
        {
            var books = new List<IForListVM<Book>>()
            {
                new BookForListVM()
                {
                    Id = 1,
                    Description = "Title1"
                },
                new BookForListVM()
                {
                    Id = 2,
                    Description = "Title2"
                }
            };
            var authorVM = new AuthorVM()
            {
                Id = 1,
                Name = "Author Name",
                LastName = "Author Last Name",
                Books = new PartialList<Book>()
                {
                    Values = books,
                    Paging = new Application.ViewModels.Helpers.Paging() { Count = 2, CurrentPage = 1, PageSize = 1 }
                }
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
            var author = Helpers.GetAuthor();
            author.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, paging);

            result.Should().NotBeNull().And.BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
            result.Name.Should().Be(author.Name);
            result.LastName.Should().Be(author.LastName);
            result.Books.Paging.Should().Be(paging);
            result.Books.Paging.CurrentPage.Should().Be(currentPage);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.Count.Should().Be(count);
            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Theory]
        [InlineData(1, 1, "Title1", 2, "Title2")]
        [InlineData(3, 5, "Title5", 6, "Title6")]
        public void MapFromBase_AuthorWithBooks_AuthorVMWithProperBooks(int currentPage, int firstBookId, string firstBookTitle, int secondBookId, string secondBookTitle)
        {
            int count = 9, pageSize = 2, pageCount = 2;
            var author = Helpers.GetAuthor();
            author.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, paging);

            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
            result.Books.Values[0].Id.Should().Be(firstBookId);
            result.Books.Values[0].Description.Should().Be(firstBookTitle);
            result.Books.Values[1].Id.Should().Be(secondBookId);
            result.Books.Values[1].Description.Should().Be(secondBookTitle);
        }

        [Fact]
        public void MapFromBase_AuthorWithBooksLastPage_AuthorVMWithProperNumberOfBooks()
        {
            int count = 9, currentPage = 5, pageSize = 2, pageCount = 1;
            var author = Helpers.GetAuthor();
            author.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, paging);

            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapFromBase_AuthorAndBooks_AuthorVM()
        {
            int count = 3, currentPage = 1, pageSize = 2, pageCount = 2;
            var author = Helpers.GetAuthor();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Should().NotBeNull().And.BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
            result.Name.Should().Be(author.Name);
            result.LastName.Should().Be(author.LastName);
            result.Books.Paging.Should().Be(paging);
            result.Books.Paging.CurrentPage.Should().Be(currentPage);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.Count.Should().Be(count);
            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapFromBase_AuthorAndNullBooks_AuthorVM()
        {
            int currentPage = 1, pageSize = 2;
            var author = Helpers.GetAuthor();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, null, paging);

            result.Should().NotBeNull().And.BeOfType<AuthorVM>();
            result.Id.Should().Be(author.Id);
            result.Name.Should().Be(author.Name);
            result.LastName.Should().Be(author.LastName);
            result.Books.Paging.Should().Be(paging);
            result.Books.Paging.CurrentPage.Should().Be(currentPage);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.Count.Should().Be(0);
            result.Books.Values.Should().NotBeNull().And.BeEmpty();
        }

        [Theory]
        [InlineData(1, 1, "Title1", 2, "Title2")]
        [InlineData(3, 5, "Title5", 6, "Title6")]
        public void MapFromBase_AuthorAndBooks_AuthorVMWithProperBooks(int currentPage, int firstBookId, string firstBookTitle, int secondBookId, string secondBookTitle)
        {
            int count = 9, pageSize = 2, pageCount = 2;
            var author = Helpers.GetAuthor();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
            result.Books.Values[0].Id.Should().Be(firstBookId);
            result.Books.Values[0].Description.Should().Be(firstBookTitle);
            result.Books.Values[1].Id.Should().Be(secondBookId);
            result.Books.Values[1].Description.Should().Be(secondBookTitle);
        }

        [Fact]
        public void MapFromBase_AuthorAndBooksLastPage_AuthorVMWithProperNumberOfBooks()
        {
            int count = 9, currentPage = 5, pageSize = 2, pageCount = 1;
            var author = Helpers.GetAuthor();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthorMappings>());
            IMapper mapper = config.CreateMapper();

            var result = author.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
        }
    }
}