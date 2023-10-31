// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class AuthorForBookVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForBook_IQueryableAuthorWithBooksAndBookId_IQueryableAuthorForBookVM()
        {
            var authors = GetAuthorsWithBooks();
            int bookId = 1;

            var result = authors.MapForBook(bookId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<AuthorForBookVM>>().And.AllBeOfType<AuthorForBookVM>().And.HaveCount(authors.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).Description.Should().Be("Amanda Popiołek");
            result.ElementAt(1).Description.Should().Be("Amanda Adamska");
            result.ElementAt(2).Description.Should().Be("Michał Popiołek");
            result.ElementAt(3).Description.Should().Be("Piotr Krasowski");
            result.ElementAt(0).IsForItem.Should().BeTrue();
            result.ElementAt(1).IsForItem.Should().BeFalse();
            result.ElementAt(2).IsForItem.Should().BeTrue();
            result.ElementAt(3).IsForItem.Should().BeTrue();
        }

        [Theory]
        [InlineData(3, 0, 4, 3, 1, 3)]
        [InlineData(3, 0, 4, 3, 2, 1)]
        [InlineData(1, 3, 1, 3, 1, 1)]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithCorrectAmountOfElementsAndCorrectPaging(int bookId, int selectedCount, int notSelectedCount, int pageSize, int pageNo, int pageCount)
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(bookId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = authors.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForBookVM>();
            result.Item.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.SelectedValues.Should().NotBeNull().And.BeOfType<List<AuthorForBookVM>>().And.HaveCount(selectedCount);
            result.Values.Should().NotBeNull().And.BeOfType<List<AuthorForBookVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(notSelectedCount);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithOrderedSelectedAuthors()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering();

            var result = authors.MapForBookToList(book, paging, filtering);

            result.SelectedValues.ElementAt(0).Id.Should().Be(4);
            result.SelectedValues.ElementAt(1).Id.Should().Be(1);
            result.SelectedValues.ElementAt(2).Id.Should().Be(3);
            result.SelectedValues.ElementAt(0).Description.Should().Be("Piotr Krasowski");
            result.SelectedValues.ElementAt(1).Description.Should().Be("Amanda Popiołek");
            result.SelectedValues.ElementAt(2).Description.Should().Be("Michał Popiołek");
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithOrderedNotSelectedAuthors()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering();

            var result = authors.MapForBookToList(book, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(2);
            result.Values.ElementAt(1).Id.Should().Be(4);
            result.Values.ElementAt(2).Id.Should().Be(1);
            result.Values.ElementAt(3).Id.Should().Be(3);
            result.Values.ElementAt(0).Description.Should().Be("Amanda Adamska");
            result.Values.ElementAt(1).Description.Should().Be("Piotr Krasowski");
            result.Values.ElementAt(2).Description.Should().Be("Amanda Popiołek");
            result.Values.ElementAt(3).Description.Should().Be("Michał Popiołek");
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithFilteredNotSelectedAuthors()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SearchString = "Popiołek" };

            var result = authors.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForBookVM>();
            result.Values.Should().NotBeNull().And.HaveCount(2);
            result.Values.ElementAt(0).Id.Should().Be(1);
            result.Values.ElementAt(1).Id.Should().Be(3);
            result.Values.ElementAt(0).Description.Should().Be("Amanda Popiołek");
            result.Values.ElementAt(1).Description.Should().Be("Michał Popiołek");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithNotSelectedAuthorsOrderedDescending()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = authors.MapForBookToList(book, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(3);
            result.Values.ElementAt(1).Id.Should().Be(1);
            result.Values.ElementAt(2).Id.Should().Be(4);
            result.Values.ElementAt(3).Id.Should().Be(2);
            result.Values.ElementAt(0).Description.Should().Be("Michał Popiołek");
            result.Values.ElementAt(1).Description.Should().Be("Amanda Popiołek");
            result.Values.ElementAt(2).Description.Should().Be("Piotr Krasowski");
            result.Values.ElementAt(3).Description.Should().Be("Amanda Adamska");
        }

        private IQueryable<Author> GetAuthorsWithBooks()
        {
            var book1 = Helpers.GetBook(1);
            var book2 = Helpers.GetBook(2);

            return new List<Author>()
            {
                new Author(){ Id = 1, Name = "Amanda", LastName = "Popiołek", Books = new List<Book>() { book1 } },
                new Author(){ Id = 2, Name = "Amanda", LastName = "Adamska", Books = new List<Book>() { book2 } },
                new Author(){ Id = 3, Name = "Michał", LastName = "Popiołek", Books = new List<Book>() { book1, book2 } },
                new Author(){ Id = 4, Name = "Piotr", LastName = "Krasowski", Books = new List<Book>() { book1, book2 } }
            }.AsQueryable();
        }
    }
}