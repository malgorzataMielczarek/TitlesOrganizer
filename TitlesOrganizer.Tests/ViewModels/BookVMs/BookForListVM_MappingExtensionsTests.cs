// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class BookForListVM_MappingExtensionsTests
    {
        [Fact]
        public void Map_Book_BookForListVM()
        {
            var book = Helpers.GetBook();

            var result = book.Map();

            result.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.Id.Should().Be(book.Id);
            result.Description.Should().NotBeNullOrWhiteSpace().And.Be(book.Title);
        }

        [Fact]
        public void Map_IQueryableBook_IQueryableIForListVMBook()
        {
            var books = Helpers.GetBooksList(4).AsQueryable();

            var result = books.Map();

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<IForListVM<Book>>>().And
                .HaveCount(4);
        }

        [Fact]
        public void Map_IEnumerableBook_ListIForListVMBook()
        {
            var books = Helpers.GetBooksList(4).AsEnumerable();

            var result = books.Map();

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<Book>>>().And
                .HaveCount(4);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPaging_IQueryableIForListVMBookWithOrderedElements()
        {
            var books = new List<Book>()
            {
                new Book(){Id = 1, Title = "Title"},
                new Book(){Id = 2, Title = "Example"},
                new Book(){Id = 3, Title = "Test"},
                new Book(){Id = 4, Title = "Another Title"},
                new Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };

            var result = books.MapToList(ref paging);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<IForListVM<Book>>>().And
                .HaveCount(5).And
                .BeInAscendingOrder(b => b.Description);
        }

        [Fact]
        public void MapToList_IEnumerableBookAndPaging_ListIForListVMBookWithOrderedElements()
        {
            var books = new List<Book>()
            {
                new Book(){Id = 1, Title = "Title"},
                new Book(){Id = 2, Title = "Example"},
                new Book(){Id = 3, Title = "Test"},
                new Book(){Id = 4, Title = "Another Title"},
                new Book(){Id = 5, Title = "One more Title"}
            }.AsEnumerable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };

            var result = books.MapToList(ref paging);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<Book>>>().And
                .HaveCount(5).And
                .BeInAscendingOrder(b => b.Description);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithOrderedValues()
        {
            var books = new List<Book>()
            {
                new Book(){Id = 1, Title = "Title"},
                new Book(){Id = 2, Title = "Example"},
                new Book(){Id = 3, Title = "Test"},
                new Book(){Id = 4, Title = "Another Title"},
                new Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<Book>>>().And
                .HaveCount(5).And
                .BeInAscendingOrder(b => b.Description);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPagingAndFilteringSortByDescending_ListBookForListVMWithValuesInDescendingOrder()
        {
            var books = new List<Book>()
            {
                new Book(){Id = 1, Title = "Title"},
                new Book(){Id = 2, Title = "Example"},
                new Book(){Id = 3, Title = "Test"},
                new Book(){Id = 4, Title = "Another Title"},
                new Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<Book>>>().And
                .HaveCount(5).And
                .BeInDescendingOrder(b => b.Description);
        }
    }
}