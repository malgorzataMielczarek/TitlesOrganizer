// Ignore Spelling: Queryable

using FluentAssertions;
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
            result.Title.Should().NotBeNullOrWhiteSpace().And.Be(book.Title);
        }

        [Fact]
        public void Map_IQueryableBook_IQueryableBookForListVM()
        {
            int count = 2;
            var books = Helpers.GetBooksList(count).AsQueryable();

            var result = books.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<BookForListVM>>().And.AllBeOfType<BookForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(books.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(books.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(books.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(books.LastOrDefault()?.Title);
        }

        [Fact]
        public void Map_ICollectionBook_ListBookForListVM()
        {
            int count = 2;
            var books = Helpers.GetBooksList(count) as ICollection<Book>;

            var result = books.Map();

            result.Should().NotBeNull().And.BeOfType<List<BookForListVM>>().And.AllBeOfType<BookForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(books.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(books.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(books.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(books.LastOrDefault()?.Title);
        }

        [Fact]
        public void Map_IEnumerableBook_ListBookForListVM()
        {
            int count = 2;
            var books = Helpers.GetBooksList(count).AsEnumerable();

            var result = books.Map();

            result.Should().NotBeNull().And.BeOfType<List<BookForListVM>>().And.AllBeOfType<BookForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(books.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(books.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(books.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(books.LastOrDefault()?.Title);
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableBookAndPaging_IQueryableBookForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var books = Helpers.GetBooksList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = books.MapToList(ref paging);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<BookForListVM>>().And.AllBeOfType<BookForListVM>().And.HaveCount(pageCount);
            paging.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            paging.Count.Should().Be(count);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPaging_IQueryableBookForListVMWithOrderedElements()
        {
            IQueryable<Book> books = new List<Book>()
            {
                new Book(){Id = 1, Title = "Title"},
                new Book(){Id = 2, Title = "Example"},
                new Book(){Id = 3, Title = "Test"},
                new Book(){Id = 4, Title = "Another Title"},
                new Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };

            var result = books.MapToList(ref paging);

            result.ElementAt(0).Id.Should().Be(4);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(5);
            result.ElementAt(3).Id.Should().Be(3);
            result.ElementAt(4).Id.Should().Be(1);
            result.ElementAt(0).Title.Should().Be("Another Title");
            result.ElementAt(1).Title.Should().Be("Example");
            result.ElementAt(2).Title.Should().Be("One more Title");
            result.ElementAt(3).Title.Should().Be("Test");
            result.ElementAt(4).Title.Should().Be("Title");
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var books = Helpers.GetBooksList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Title" };

            var result = books.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForListVM>();
            result.Books.Should().HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithOrderedElements()
        {
            IQueryable<Book> books = new List<Book>()
            {
                new Book(){Id = 1, Title = "Title"},
                new Book(){Id = 2, Title = "Example"},
                new Book(){Id = 3, Title = "Test"},
                new Book(){Id = 4, Title = "Another Title"},
                new Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };

            var result = books.MapToList(paging, new Filtering());

            result.Books.ElementAt(0).Id.Should().Be(4);
            result.Books.ElementAt(1).Id.Should().Be(2);
            result.Books.ElementAt(2).Id.Should().Be(5);
            result.Books.ElementAt(3).Id.Should().Be(3);
            result.Books.ElementAt(4).Id.Should().Be(1);
            result.Books.ElementAt(0).Title.Should().Be("Another Title");
            result.Books.ElementAt(1).Title.Should().Be("Example");
            result.Books.ElementAt(2).Title.Should().Be("One more Title");
            result.Books.ElementAt(3).Title.Should().Be("Test");
            result.Books.ElementAt(4).Title.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithFilteredElements()
        {
            IQueryable<Book> books = new List<Book>()
            {
                new Book(){Id = 1, Title = "Title"},
                new Book(){Id = 2, Title = "Example"},
                new Book(){Id = 3, Title = "Test"},
                new Book(){Id = 4, Title = "Another Title"},
                new Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Ascending, SearchString = "Title" };

            var result = books.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForListVM>();
            result.Books.Should().NotBeNull().And.HaveCount(3);
            result.Books.ElementAt(0).Id.Should().Be(4);
            result.Books.ElementAt(1).Id.Should().Be(5);
            result.Books.ElementAt(2).Id.Should().Be(1);
            result.Books.ElementAt(0).Title.Should().Be("Another Title");
            result.Books.ElementAt(1).Title.Should().Be("One more Title");
            result.Books.ElementAt(2).Title.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(3);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithElementsOrderedDescending()
        {
            IQueryable<Book> books = new List<Book>()
            {
                new Book(){Id = 1, Title = "Title"},
                new Book(){Id = 2, Title = "Example"},
                new Book(){Id = 3, Title = "Test"},
                new Book(){Id = 4, Title = "Another Title"},
                new Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1 - desc: 1, 3, 5, 2, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapToList(paging, filtering);

            result.Books.ElementAt(0).Id.Should().Be(1);
            result.Books.ElementAt(1).Id.Should().Be(3);
            result.Books.ElementAt(2).Id.Should().Be(5);
            result.Books.ElementAt(3).Id.Should().Be(2);
            result.Books.ElementAt(4).Id.Should().Be(4);
            result.Books.ElementAt(0).Title.Should().Be("Title");
            result.Books.ElementAt(1).Title.Should().Be("Test");
            result.Books.ElementAt(2).Title.Should().Be("One more Title");
            result.Books.ElementAt(3).Title.Should().Be("Example");
            result.Books.ElementAt(4).Title.Should().Be("Another Title");
        }
    }
}