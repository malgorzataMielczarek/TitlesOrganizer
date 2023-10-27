// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.ReferencesVMs.ForAuthorVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs.ReferencesVMs.ForAuthorVMs
{
    public class MappingExtensionsTests_Book
    {
        [Fact]
        public void MapForAuthor_IQueryableBookWithAuthorsAndAuthorId_IQueryableBookForAuthorVM()
        {
            var books = GetBooksWithAuthors();
            int authorId = 1;

            var result = books.MapForAuthor(authorId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<BookForAuthorVM>>().And.AllBeOfType<BookForAuthorVM>().And.HaveCount(books.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).Title.Should().Be("Title");
            result.ElementAt(1).Title.Should().Be("Example");
            result.ElementAt(2).Title.Should().Be("Test");
            result.ElementAt(3).Title.Should().Be("Another Title");
            result.ElementAt(0).IsForAuthor.Should().BeTrue();
            result.ElementAt(1).IsForAuthor.Should().BeFalse();
            result.ElementAt(2).IsForAuthor.Should().BeTrue();
            result.ElementAt(3).IsForAuthor.Should().BeTrue();
        }

        [Theory]
        [InlineData(3, 0, 4, 3, 1, 3)]
        [InlineData(3, 0, 4, 3, 2, 1)]
        [InlineData(1, 3, 1, 3, 1, 1)]
        public void MapToList_IQueryableBookWithAuthorsAndAuthorAndPagingAndFiltering_ListBookForAuthorVMWithCorrectAmountOfElementsAndCorrectPaging(int authorId, int selectedCount, int notSelectedCount, int pageSize, int pageNo, int pageCount)
        {
            var books = GetBooksWithAuthors();
            var author = Helpers.GetAuthor(authorId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = books.MapForAuthorToList(author, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForAuthorVM>();
            result.Author.Should().NotBeNull().And.BeOfType<AuthorForListVM>();
            result.SelectedBooks.Should().NotBeNull().And.BeOfType<List<BookForAuthorVM>>().And.HaveCount(selectedCount);
            result.NotSelectedBooks.Should().NotBeNull().And.BeOfType<List<BookForAuthorVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(notSelectedCount);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookWithAuthorsAndAuthorAndPagingAndFiltering_ListBookForAuthorVMWithOrderedSelectedBooks()
        {
            var books = GetBooksWithAuthors();
            var author = Helpers.GetAuthor(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForAuthorToList(author, paging, filtering);

            result.SelectedBooks.ElementAt(0).Id.Should().Be(4);
            result.SelectedBooks.ElementAt(1).Id.Should().Be(3);
            result.SelectedBooks.ElementAt(2).Id.Should().Be(1);
            result.SelectedBooks.ElementAt(0).Title.Should().Be("Another Title");
            result.SelectedBooks.ElementAt(1).Title.Should().Be("Test");
            result.SelectedBooks.ElementAt(2).Title.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookWithAuthorsAndAuthorAndPagingAndFiltering_ListBookForAuthorVMWithOrderedNotSelectedBooks()
        {
            var books = GetBooksWithAuthors();
            var author = Helpers.GetAuthor(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForAuthorToList(author, paging, filtering);

            result.NotSelectedBooks.ElementAt(0).Id.Should().Be(4);
            result.NotSelectedBooks.ElementAt(1).Id.Should().Be(2);
            result.NotSelectedBooks.ElementAt(2).Id.Should().Be(3);
            result.NotSelectedBooks.ElementAt(3).Id.Should().Be(1);
            result.NotSelectedBooks.ElementAt(0).Title.Should().Be("Another Title");
            result.NotSelectedBooks.ElementAt(1).Title.Should().Be("Example");
            result.NotSelectedBooks.ElementAt(2).Title.Should().Be("Test");
            result.NotSelectedBooks.ElementAt(3).Title.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookWithAuthorsAndAuthorAndPagingAndFiltering_ListBookForAuthorVMWithFilteredNotSelectedBooks()
        {
            var books = GetBooksWithAuthors();
            var author = Helpers.GetAuthor(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SearchString = "Title" };

            var result = books.MapForAuthorToList(author, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForAuthorVM>();
            result.NotSelectedBooks.Should().NotBeNull().And.HaveCount(2);
            result.NotSelectedBooks.ElementAt(0).Id.Should().Be(4);
            result.NotSelectedBooks.ElementAt(1).Id.Should().Be(1);
            result.NotSelectedBooks.ElementAt(0).Title.Should().Be("Another Title");
            result.NotSelectedBooks.ElementAt(1).Title.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableBookWithAuthorsAndAuthorAndPagingAndFiltering_ListBookForAuthorVMWithNotSelectedBooksOrderedDescending()
        {
            var books = GetBooksWithAuthors();
            var author = Helpers.GetAuthor(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapForAuthorToList(author, paging, filtering);

            result.NotSelectedBooks.ElementAt(0).Id.Should().Be(1);
            result.NotSelectedBooks.ElementAt(1).Id.Should().Be(3);
            result.NotSelectedBooks.ElementAt(2).Id.Should().Be(2);
            result.NotSelectedBooks.ElementAt(3).Id.Should().Be(4);
            result.NotSelectedBooks.ElementAt(0).Title.Should().Be("Title");
            result.NotSelectedBooks.ElementAt(1).Title.Should().Be("Test");
            result.NotSelectedBooks.ElementAt(2).Title.Should().Be("Example");
            result.NotSelectedBooks.ElementAt(3).Title.Should().Be("Another Title");
        }

        private IQueryable<Book> GetBooksWithAuthors()
        {
            var author1 = Helpers.GetAuthor(1);
            var author2 = Helpers.GetAuthor(2);

            return new List<Book>()
            {
                new Book(){ Id = 1, Title = "Title", Authors = new List<Author>() { author1 } },
                new Book(){ Id = 2, Title = "Example", Authors = new List<Author>() { author2 } },
                new Book(){ Id = 3, Title = "Test", Authors = new List<Author>() { author1, author2 } },
                new Book(){ Id = 4, Title = "Another Title", Authors = new List<Author>() { author1, author2 } }
            }.AsQueryable();
        }
    }
}