// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class BookForGenreVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForAuthor_IQueryableBookWithGenresAndGenreId_IQueryableBookForGenreVM()
        {
            var books = GetBooksWithGenres();
            int genreId = 1;

            var result = books.MapForItem(genreId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<BookForGenreVM>>().And.AllBeOfType<BookForGenreVM>().And.HaveCount(books.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).Description.Should().Be("Title");
            result.ElementAt(1).Description.Should().Be("Example");
            result.ElementAt(2).Description.Should().Be("Test");
            result.ElementAt(3).Description.Should().Be("Another Title");
            result.ElementAt(0).IsForItem.Should().BeTrue();
            result.ElementAt(1).IsForItem.Should().BeFalse();
            result.ElementAt(2).IsForItem.Should().BeTrue();
            result.ElementAt(3).IsForItem.Should().BeTrue();
        }

        [Theory]
        [InlineData(3, 0, 4, 3, 1, 3)]
        [InlineData(3, 0, 4, 3, 2, 1)]
        [InlineData(1, 3, 1, 3, 1, 1)]
        public void MapToList_IQueryableBookWithGenresAndLiteratureGenreAndPagingAndFiltering_ListBookForGenreVMWithCorrectAmountOfElementsAndCorrectPaging(int genreId, int selectedCount, int notSelectedCount, int pageSize, int pageNo, int pageCount)
        {
            var books = GetBooksWithGenres();
            var genre = Helpers.GetGenre(genreId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = books.MapForItemToList(genre, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForGenreVM>();
            result.Item.Should().NotBeNull().And.BeOfType<GenreForListVM>();
            result.SelectedValues.Should().NotBeNull().And.BeOfType<List<BookForGenreVM>>().And.HaveCount(selectedCount);
            result.Values.Should().NotBeNull().And.BeOfType<List<BookForGenreVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(notSelectedCount);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookWithGenresAndLiteratureGenreAndPagingAndFiltering_ListBookForGenreVMWithOrderedSelectedBooks()
        {
            var books = GetBooksWithGenres();
            var genre = Helpers.GetGenre(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForItemToList(genre, paging, filtering);

            result.SelectedValues.ElementAt(0).Id.Should().Be(4);
            result.SelectedValues.ElementAt(1).Id.Should().Be(3);
            result.SelectedValues.ElementAt(2).Id.Should().Be(1);
            result.SelectedValues.ElementAt(0).Description.Should().Be("Another Title");
            result.SelectedValues.ElementAt(1).Description.Should().Be("Test");
            result.SelectedValues.ElementAt(2).Description.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookWithGenresAndLiteratureGenreAndPagingAndFiltering_ListBookForGenreVMWithOrderedNotSelectedBooks()
        {
            var books = GetBooksWithGenres();
            var genre = Helpers.GetGenre(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForItemToList(genre, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(4);
            result.Values.ElementAt(1).Id.Should().Be(2);
            result.Values.ElementAt(2).Id.Should().Be(3);
            result.Values.ElementAt(3).Id.Should().Be(1);
            result.Values.ElementAt(0).Description.Should().Be("Another Title");
            result.Values.ElementAt(1).Description.Should().Be("Example");
            result.Values.ElementAt(2).Description.Should().Be("Test");
            result.Values.ElementAt(3).Description.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookWithGenresAndLiteratureGenreAndPagingAndFiltering_ListBookForGenreVMWithFilteredNotSelectedBooks()
        {
            var books = GetBooksWithGenres();
            var genre = Helpers.GetGenre(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SearchString = "Title" };

            var result = books.MapForItemToList(genre, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForGenreVM>();
            result.Values.Should().NotBeNull().And.HaveCount(2);
            result.Values.ElementAt(0).Id.Should().Be(4);
            result.Values.ElementAt(1).Id.Should().Be(1);
            result.Values.ElementAt(0).Description.Should().Be("Another Title");
            result.Values.ElementAt(1).Description.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableBookWithGenresAndLiteratureGenreAndPagingAndFiltering_ListBookForGenreVMWithNotSelectedBooksOrderedDescending()
        {
            var books = GetBooksWithGenres();
            var genre = Helpers.GetGenre(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapForItemToList(genre, paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(1);
            result.Values.ElementAt(1).Id.Should().Be(3);
            result.Values.ElementAt(2).Id.Should().Be(2);
            result.Values.ElementAt(3).Id.Should().Be(4);
            result.Values.ElementAt(0).Description.Should().Be("Title");
            result.Values.ElementAt(1).Description.Should().Be("Test");
            result.Values.ElementAt(2).Description.Should().Be("Example");
            result.Values.ElementAt(3).Description.Should().Be("Another Title");
        }

        private IQueryable<Book> GetBooksWithGenres()
        {
            var genre1 = Helpers.GetGenre(1);
            var genre2 = Helpers.GetGenre(2);

            return new List<Book>()
            {
                new Book(){ Id = 1, Title = "Title", Genres = new List<LiteratureGenre>() { genre1 } },
                new Book(){ Id = 2, Title = "Example", Genres = new List<LiteratureGenre>() { genre2 } },
                new Book(){ Id = 3, Title = "Test", Genres = new List < LiteratureGenre >() { genre1, genre2 } },
                new Book(){ Id = 4, Title = "Another Title", Genres = new List < LiteratureGenre >() { genre1, genre2 } }
            }.AsQueryable();
        }
    }
}