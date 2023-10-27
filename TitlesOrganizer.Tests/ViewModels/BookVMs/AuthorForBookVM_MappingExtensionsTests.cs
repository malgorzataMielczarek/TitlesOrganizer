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
            result.ElementAt(0).FullName.Should().Be("Amanda Popiołek");
            result.ElementAt(1).FullName.Should().Be("Amanda Adamska");
            result.ElementAt(2).FullName.Should().Be("Michał Popiołek");
            result.ElementAt(3).FullName.Should().Be("Piotr Krasowski");
            result.ElementAt(0).IsForBook.Should().BeTrue();
            result.ElementAt(1).IsForBook.Should().BeFalse();
            result.ElementAt(2).IsForBook.Should().BeTrue();
            result.ElementAt(3).IsForBook.Should().BeTrue();
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
            result.Book.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.SelectedAuthors.Should().NotBeNull().And.BeOfType<List<AuthorForBookVM>>().And.HaveCount(selectedCount);
            result.NotSelectedAuthors.Should().NotBeNull().And.BeOfType<List<AuthorForBookVM>>().And.HaveCount(pageCount);
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

            result.SelectedAuthors.ElementAt(0).Id.Should().Be(4);
            result.SelectedAuthors.ElementAt(1).Id.Should().Be(1);
            result.SelectedAuthors.ElementAt(2).Id.Should().Be(3);
            result.SelectedAuthors.ElementAt(0).FullName.Should().Be("Piotr Krasowski");
            result.SelectedAuthors.ElementAt(1).FullName.Should().Be("Amanda Popiołek");
            result.SelectedAuthors.ElementAt(2).FullName.Should().Be("Michał Popiołek");
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithOrderedNotSelectedAuthors()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering();

            var result = authors.MapForBookToList(book, paging, filtering);

            result.NotSelectedAuthors.ElementAt(0).Id.Should().Be(2);
            result.NotSelectedAuthors.ElementAt(1).Id.Should().Be(4);
            result.NotSelectedAuthors.ElementAt(2).Id.Should().Be(1);
            result.NotSelectedAuthors.ElementAt(3).Id.Should().Be(3);
            result.NotSelectedAuthors.ElementAt(0).FullName.Should().Be("Amanda Adamska");
            result.NotSelectedAuthors.ElementAt(1).FullName.Should().Be("Piotr Krasowski");
            result.NotSelectedAuthors.ElementAt(2).FullName.Should().Be("Amanda Popiołek");
            result.NotSelectedAuthors.ElementAt(3).FullName.Should().Be("Michał Popiołek");
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
            result.NotSelectedAuthors.Should().NotBeNull().And.HaveCount(2);
            result.NotSelectedAuthors.ElementAt(0).Id.Should().Be(1);
            result.NotSelectedAuthors.ElementAt(1).Id.Should().Be(3);
            result.NotSelectedAuthors.ElementAt(0).FullName.Should().Be("Amanda Popiołek");
            result.NotSelectedAuthors.ElementAt(1).FullName.Should().Be("Michał Popiołek");
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

            result.NotSelectedAuthors.ElementAt(0).Id.Should().Be(3);
            result.NotSelectedAuthors.ElementAt(1).Id.Should().Be(1);
            result.NotSelectedAuthors.ElementAt(2).Id.Should().Be(4);
            result.NotSelectedAuthors.ElementAt(3).Id.Should().Be(2);
            result.NotSelectedAuthors.ElementAt(0).FullName.Should().Be("Michał Popiołek");
            result.NotSelectedAuthors.ElementAt(1).FullName.Should().Be("Amanda Popiołek");
            result.NotSelectedAuthors.ElementAt(2).FullName.Should().Be("Piotr Krasowski");
            result.NotSelectedAuthors.ElementAt(3).FullName.Should().Be("Amanda Adamska");
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