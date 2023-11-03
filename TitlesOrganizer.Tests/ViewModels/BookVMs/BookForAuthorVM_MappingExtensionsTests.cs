// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class BookForAuthorVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForItem_BookWithGivenAuthor_IForItemVMBookAuthor()
        {
            var author = Helpers.GetAuthor();
            var book = new Book()
            {
                Id = 1,
                Title = "Test",
                Authors = { author }
            };

            var result = book.MapForItem(author);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<Book, Author>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Test");
            result.IsForItem.Should().BeTrue();
        }

        [Fact]
        public void MapForItem_BookWithoutGivenAuthor_IForItemVMBookAuthor()
        {
            var author = Helpers.GetAuthor(1);
            var book = new Book()
            {
                Id = 1,
                Title = "Test",
                Authors = { author }
            };
            var otherAuthor = Helpers.GetAuthor(2);

            var result = book.MapForItem(otherAuthor);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<Book, Author>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Test");
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void MapForItemToList_IQueryableBooksAndAuthor_ListBookForAuthorVM()
        {
            var author1 = Helpers.GetAuthor(1);
            var author2 = Helpers.GetAuthor(2);
            var author3 = Helpers.GetAuthor(3);
            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "Title", Authors = new List<Author>() { author1 } },
                new Book(){ Id = 2, Title = "Example", Authors = new List<Author>() { author2 } },
                new Book(){ Id = 3, Title = "Test", Authors = new List<Author>() { author1, author2, author3 } },
                new Book(){ Id = 4, Title = "Another Title", Authors = new List<Author>() { author1, author2 } }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForItemToList(author3, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForAuthorVM>();
            result.SelectedValues.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(1).And
                .Contain(b => b.Id == 3 && b.Description == "Test");
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(3).And
                .AllSatisfy(b => b.IsForItem.Should().BeFalse()).And
                .BeInAscendingOrder(b => b.Description).And
                .NotContain(b => b.Id == 3 && b.Description == "Test");
        }

        [Fact]
        public void MapForItemToList_IQueryableBooksAndAuthorAndSortByDescending_ListBookForAuthorVMWithValuesInDescendingOrder()
        {
            var author1 = Helpers.GetAuthor(1);
            var author2 = Helpers.GetAuthor(2);
            var author3 = Helpers.GetAuthor(3);
            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "Title", Authors = new List<Author>() { author1 } },
                new Book(){ Id = 2, Title = "Example", Authors = new List<Author>() { author2 } },
                new Book(){ Id = 3, Title = "Test", Authors = new List<Author>() { author1, author2, author3 } },
                new Book(){ Id = 4, Title = "Another Title", Authors = new List<Author>() { author1, author2 } }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapForItemToList(author3, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForAuthorVM>();
            result.SelectedValues.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(1).And
                .Contain(b => b.Id == 3 && b.Description == "Test");
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(3).And
                .AllSatisfy(b => b.IsForItem.Should().BeFalse()).And
                .BeInDescendingOrder(b => b.Description).And
                .NotContain(b => b.Id == 3 && b.Description == "Test");
        }
    }
}