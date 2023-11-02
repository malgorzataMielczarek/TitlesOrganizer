// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class GenreForBookVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForBook_IQueryableLiteratureGenreWithBooksAndBookId_IQueryableGenreForBookVM()
        {
            var genres = GetGenresWithBooks();
            int bookId = 1;

            var result = genres.MapForItem(bookId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<GenreForBookVM>>().And.AllBeOfType<GenreForBookVM>().And.HaveCount(genres.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).Description.Should().Be("Crime Comedy");
            result.ElementAt(1).Description.Should().Be("Romantic Comedy");
            result.ElementAt(2).Description.Should().Be("Comedy");
            result.ElementAt(3).Description.Should().Be("Crime Story");
            result.ElementAt(0).IsForItem.Should().BeTrue();
            result.ElementAt(1).IsForItem.Should().BeFalse();
            result.ElementAt(2).IsForItem.Should().BeTrue();
            result.ElementAt(3).IsForItem.Should().BeTrue();
        }

        [Theory]
        [InlineData(3, 0, 4, 3, 1, 3)]
        [InlineData(3, 0, 4, 3, 2, 1)]
        [InlineData(1, 3, 1, 3, 1, 1)]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithCorrectAmountOfElementsAndCorrectPaging(int bookId, int selectedCount, int notSelectedCount, int pageSize, int pageNo, int pageCount)
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(bookId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = genres.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForBookVM>();
            result.Item.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.SelectedValues.Should().NotBeNull().And.BeOfType<List<GenreForBookVM>>().And.HaveCount(selectedCount);
            result.Values.Should().NotBeNull().And.BeOfType<List<GenreForBookVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(notSelectedCount);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithOrderedSelectedGenres()
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering();

            var result = genres.MapForBookToList(book, paging, filtering);

            result.SelectedValues.ElementAt(0).Id.Should().Be(3);
            result.SelectedValues.ElementAt(1).Id.Should().Be(1);
            result.SelectedValues.ElementAt(2).Id.Should().Be(4);
            result.SelectedValues.ElementAt(0).Description.Should().Be("Comedy");
            result.SelectedValues.ElementAt(1).Description.Should().Be("Crime Comedy");
            result.SelectedValues.ElementAt(2).Description.Should().Be("Crime Story");
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithOrderedNotSelectedGenres()
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering();

            var result = genres.MapForBookToList(book, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(3);
            result.Values.ElementAt(1).Id.Should().Be(1);
            result.Values.ElementAt(2).Id.Should().Be(4);
            result.Values.ElementAt(3).Id.Should().Be(2);
            result.Values.ElementAt(0).Description.Should().Be("Comedy");
            result.Values.ElementAt(1).Description.Should().Be("Crime Comedy");
            result.Values.ElementAt(2).Description.Should().Be("Crime Story");
            result.Values.ElementAt(3).Description.Should().Be("Romantic Comedy");
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithFilteredNotSelectedGenres()
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SearchString = "Crime" };

            var result = genres.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForBookVM>();
            result.Values.Should().NotBeNull().And.HaveCount(2);
            result.Values.ElementAt(0).Id.Should().Be(1);
            result.Values.ElementAt(1).Id.Should().Be(4);
            result.Values.ElementAt(0).Description.Should().Be("Crime Comedy");
            result.Values.ElementAt(1).Description.Should().Be("Crime Story");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithNotSelectedGenresOrderedDescending()
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = genres.MapForBookToList(book, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(2);
            result.Values.ElementAt(1).Id.Should().Be(4);
            result.Values.ElementAt(2).Id.Should().Be(1);
            result.Values.ElementAt(3).Id.Should().Be(3);
            result.Values.ElementAt(0).Description.Should().Be("Romantic Comedy");
            result.Values.ElementAt(1).Description.Should().Be("Crime Story");
            result.Values.ElementAt(2).Description.Should().Be("Crime Comedy");
            result.Values.ElementAt(3).Description.Should().Be("Comedy");
        }

        private IQueryable<LiteratureGenre> GetGenresWithBooks()
        {
            var book1 = Helpers.GetBook(1);
            var book2 = Helpers.GetBook(2);

            return new List<LiteratureGenre>()
            {
                new LiteratureGenre(){ Id = 1, Name = "Crime Comedy", Books = new List<Book>() { book1 } },
                new LiteratureGenre(){ Id = 2, Name = "Romantic Comedy", Books = new List<Book>() { book2 } },
                new LiteratureGenre(){ Id = 3, Name = "Comedy", Books = new List<Book>() { book1, book2 } },
                new LiteratureGenre(){ Id = 4, Name = "Crime Story", Books = new List<Book>() { book1, book2 } }
            }.AsQueryable();
        }
    }
}