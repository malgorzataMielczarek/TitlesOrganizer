// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class AuthorForBookVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForItem_AuthorWithGivenBook_IForItemVMAuthorBook()
        {
            var book = BookModuleHelpers.GetBook();
            var author = new Author()
            {
                Id = 1,
                Name = "Test",
                LastName = "Surname",
                Books = new List<Book>() { book }
            };

            var result = author.MapForItem(book);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<Author, Book>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Test Surname");
            result.IsForItem.Should().BeTrue();
        }

        [Fact]
        public void MapForItem_AuthorWithoutGivenBook_IForItemVMAuthorBook()
        {
            var book = BookModuleHelpers.GetBook();
            var author = new Author()
            {
                Id = 1,
                Name = "Test",
                LastName = "Surname",
                Books = new List<Book>() { book }
            };
            author.Books.Add(book);
            var otherBook = BookModuleHelpers.GetBook(2);

            var result = author.MapForItem(otherBook);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<Author, Book>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Test Surname");
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void MapForItemToList_IQueryableAuthorWithoutGivenBook_ListAuthorForBookVMWithOrderedValues()
        {
            var book1 = BookModuleHelpers.GetBook(1);
            var book2 = BookModuleHelpers.GetBook(2);

            var authors = new List<Author>()
            {
                new Author(){ Id = 1, Name = "Amanda", LastName = "Popiołek", Books = new List<Book>() { book1 } },
                new Author(){ Id = 2, Name = "Amanda", LastName = "Adamska", Books = new List<Book>() { book2 } },
                new Author(){ Id = 3, Name = "Michał", LastName = "Popiołek", Books = new List<Book>() { book1, book2 } },
                new Author(){ Id = 4, Name = "Piotr", LastName = "Krasowski", Books = new List<Book>() { book1, book2 } }
            }.AsQueryable();
            var book = BookModuleHelpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering();

            var result = authors.MapForItemToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForBookVM>();
            result.SelectedValues.Should().NotBeNull().And.BeEmpty();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(4).And
                .AllSatisfy(a => a.IsForItem.Should().BeFalse()).And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(2);
                        first.Description.Should().Be("Amanda Adamska");
                    },
                    second =>
                    {
                        second.Id.Should().Be(4);
                        second.Description.Should().Be("Piotr Krasowski");
                    },
                    third =>
                    {
                        third.Id.Should().Be(1);
                        third.Description.Should().Be("Amanda Popiołek");
                    },
                    fourth =>
                    {
                        fourth.Id.Should().Be(3);
                        fourth.Description.Should().Be("Michał Popiołek");
                    }
                );
        }

        [Fact]
        public void MapForItemToList_IQueryableAuthorWithoutGivenBookAndFilteringSortedByDescending_ListAuthorForBookVMWithValuesInDescendingOrder()
        {
            var book1 = BookModuleHelpers.GetBook(1);
            var book2 = BookModuleHelpers.GetBook(2);

            var authors = new List<Author>()
            {
                new Author(){ Id = 1, Name = "Amanda", LastName = "Popiołek", Books = new List<Book>() { book1 } },
                new Author(){ Id = 2, Name = "Amanda", LastName = "Adamska", Books = new List<Book>() { book2 } },
                new Author(){ Id = 3, Name = "Michał", LastName = "Popiołek", Books = new List<Book>() { book1, book2 } },
                new Author(){ Id = 4, Name = "Piotr", LastName = "Krasowski", Books = new List<Book>() { book1, book2 } }
            }.AsQueryable();
            var book = BookModuleHelpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = authors.MapForItemToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForBookVM>();
            result.SelectedValues.Should().NotBeNull().And.BeEmpty();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(4).And
                .AllSatisfy(a => a.IsForItem.Should().BeFalse()).And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(3);
                        first.Description.Should().Be("Michał Popiołek");
                    },
                    second =>
                    {
                        second.Id.Should().Be(1);
                        second.Description.Should().Be("Amanda Popiołek");
                    },
                    third =>
                    {
                        third.Id.Should().Be(4);
                        third.Description.Should().Be("Piotr Krasowski");
                    },
                    fourth =>
                    {
                        fourth.Id.Should().Be(2);
                        fourth.Description.Should().Be("Amanda Adamska");
                    }
                );
        }
    }
}