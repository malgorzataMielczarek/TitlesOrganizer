// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class GenreForBookVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForItem_LiteratureGenreWithGivenBook_IForItemVMQueryableLiteratureGenreBook()
        {
            var book = BookModuleHelpers.GetBook(1);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Test",
                Books = new List<Book>() { book }
            };

            var result = genre.MapForItem(book);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<LiteratureGenre, Book>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Test");
            result.IsForItem.Should().BeTrue();
        }

        [Fact]
        public void MapForItem_LiteratureGenreWithoutGivenBook_IForItemVMQueryableLiteratureGenreBook()
        {
            var book = BookModuleHelpers.GetBook(1);
            var anotherBook = BookModuleHelpers.GetBook(2);
            var genre = new LiteratureGenre()
            {
                Id = 1,
                Name = "Test",
                Books = new List<Book>() { book }
            };

            var result = genre.MapForItem(anotherBook);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<LiteratureGenre, Book>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Test");
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void MapForItemToList_IQueryableLiteratureGenreAndBookAndPagingAndFiltering_ListGenreForBookVMWithOrderedValues()
        {
            var book1 = BookModuleHelpers.GetBook(1);
            var book2 = BookModuleHelpers.GetBook(2);
            var book3 = BookModuleHelpers.GetBook(3);
            var genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre()
                {
                    Id = 1,
                    Name = "Crime Comedy",
                    Books = new List<Book>() { book1 }
                },
                new LiteratureGenre()
                {
                    Id = 2,
                    Name = "Romantic Comedy",
                    Books = new List<Book>() { book2 }
                },
                new LiteratureGenre()
                {
                    Id = 3,
                    Name = "Comedy",
                    Books = new List<Book>() { book1, book2, book3 }
                },
                new LiteratureGenre()
                {
                    Id = 4,
                    Name = "Crime Story",
                    Books = new List<Book>() { book1, book2 }
                }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filttering = new Filtering();

            var result = genres.MapForItemToList(book3, paging, filttering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForBookVM>();
            result.SelectedValues.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<LiteratureGenre, Book>>>().And
                .HaveCount(1).And
                .ContainSingle(g => g.Id == 3 && g.Description == "Comedy");
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<LiteratureGenre, Book>>>().And
                .HaveCount(3).And
                .BeInAscendingOrder(g => g.Description);
        }

        [Fact]
        public void MapForItemToList_IQueryableLiteratureGenreAndBookAndPagingAndFilteringSortByDescending_ListGenreForBookVMWithValuesInDescOrder()
        {
            var book1 = BookModuleHelpers.GetBook(1);
            var book2 = BookModuleHelpers.GetBook(2);
            var book3 = BookModuleHelpers.GetBook(3);
            var genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre()
                {
                    Id = 1,
                    Name = "Crime Comedy",
                    Books = new List<Book>() { book1 }
                },
                new LiteratureGenre()
                {
                    Id = 2,
                    Name = "Romantic Comedy",
                    Books = new List<Book>() { book2 }
                },
                new LiteratureGenre()
                {
                    Id = 3,
                    Name = "Comedy",
                    Books = new List<Book>() { book1, book2, book3 }
                },
                new LiteratureGenre()
                {
                    Id = 4,
                    Name = "Crime Story",
                    Books = new List<Book>() { book1, book2 }
                }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filttering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = genres.MapForItemToList(book3, paging, filttering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForBookVM>();
            result.SelectedValues.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<LiteratureGenre, Book>>>().And
                .HaveCount(1).And
                .ContainSingle(g => g.Id == 3 && g.Description == "Comedy");
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForItemVM<LiteratureGenre, Book>>>().And
                .HaveCount(3).And
                .BeInDescendingOrder(g => g.Description);
        }
    }
}