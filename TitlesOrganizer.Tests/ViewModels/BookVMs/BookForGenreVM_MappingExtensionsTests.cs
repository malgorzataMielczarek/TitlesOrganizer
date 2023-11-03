// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class BookForGenreVM_MappingExtensionsTests
    {
        [Fact]
        public void MapForItem_BookWithGivenLiteratureGenre_IForItemVMBookLiteratureGenre()
        {
            var genre = Helpers.GetGenre(1);
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = { genre }
            };

            var result = book.MapForItem(genre);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<Book, LiteratureGenre>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Title");
            result.IsForItem.Should().BeTrue();
        }

        [Fact]
        public void MapForItem_BookWithoutGivenLiteratureGenre_IForItemVMBookLiteratureGenre()
        {
            var genre = Helpers.GetGenre(1);
            var otherGenre = Helpers.GetGenre(2);
            var book = new Book()
            {
                Id = 1,
                Title = "Title",
                Genres = { genre }
            };

            var result = book.MapForItem(otherGenre);

            result.Should().NotBeNull().And.BeAssignableTo<IForItemVM<Book, LiteratureGenre>>();
            result.Id.Should().Be(1);
            result.Description.Should().Be("Title");
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void MapForItemToList_IQueryableBookLiteratureGenrePagingFiltering_ListBookForGenreVMWithValuesSortedInAscendingOrder()
        {
            var genre1 = Helpers.GetGenre(1);
            var genre2 = Helpers.GetGenre(2);
            var genre3 = Helpers.GetGenre(3);
            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "Title", Genres = new List<LiteratureGenre>() { genre1 } },
                new Book(){ Id = 2, Title = "Example", Genres = new List<LiteratureGenre>() { genre2 } },
                new Book(){ Id = 3, Title = "Test", Genres = new List < LiteratureGenre >() { genre1, genre2, genre3 } },
                new Book(){ Id = 4, Title = "Another Title", Genres = new List < LiteratureGenre >() { genre1, genre2 } }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering();

            var result = books.MapForItemToList(genre3, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForGenreVM>();
            result.SelectedValues.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(1).And
                .ContainSingle(b => b.Id == 3 && b.Description == "Test");
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(3).And
                .AllSatisfy(b => b.IsForItem.Should().BeFalse()).And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(4);
                        first.Description.Should().Be("Another Title");
                    },
                    second =>
                    {
                        second.Id.Should().Be(2);
                        second.Description.Should().Be("Example");
                    },
                    third =>
                    {
                        third.Id.Should().Be(1);
                        third.Description.Should().Be("Title");
                    }
                );
        }

        [Fact]
        public void MapForItemToList_IQueryableBookLiteratureGenrePagingFilteringSortByDescending_ListBookForGenreVMWithValuesSortedInDescOrder()
        {
            var genre1 = Helpers.GetGenre(1);
            var genre2 = Helpers.GetGenre(2);
            var genre3 = Helpers.GetGenre(3);
            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "Title", Genres = new List<LiteratureGenre>() { genre1 } },
                new Book(){ Id = 2, Title = "Example", Genres = new List<LiteratureGenre>() { genre2 } },
                new Book(){ Id = 3, Title = "Test", Genres = new List < LiteratureGenre >() { genre1, genre2, genre3 } },
                new Book(){ Id = 4, Title = "Another Title", Genres = new List < LiteratureGenre >() { genre1, genre2 } }
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapForItemToList(genre3, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForGenreVM>();
            result.SelectedValues.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(1).And
                .ContainSingle(b => b.Id == 3 && b.Description == "Test");
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(3).And
                .AllSatisfy(b => b.IsForItem.Should().BeFalse()).And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(1);
                        first.Description.Should().Be("Title");
                    },
                    second =>
                    {
                        second.Id.Should().Be(2);
                        second.Description.Should().Be("Example");
                    },
                    third =>
                    {
                        third.Id.Should().Be(4);
                        third.Description.Should().Be("Another Title");
                    }
                );
        }
    }
}