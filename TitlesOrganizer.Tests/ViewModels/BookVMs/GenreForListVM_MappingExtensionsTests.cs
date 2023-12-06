// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Tests.Helpers;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class GenreForListVM_MappingExtensionsTests
    {
        [Fact]
        public void Map_LiteratureGenre_GenreForListVM()
        {
            var genre = BookModuleHelpers.GetGenre();

            var result = genre.Map();

            result.Should().NotBeNull().And.BeOfType<GenreForListVM>();
            result.Id.Should().Be(genre.Id);
            result.Description.Should().NotBeNullOrWhiteSpace().And.Be(genre.Name);
        }

        [Fact]
        public void Map_IQueryableLiteratureGenre_IQueryableIForListVMLiteratureGenre()
        {
            var genres = BookModuleHelpers.GetGenresList(4).AsQueryable();

            var result = genres.Map();

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<IForListVM<LiteratureGenre>>>().And
                .HaveCount(4);
        }

        [Fact]
        public void Map_IEnumerableLiteratureGenre_ListIForListVMLiteratureGenre()
        {
            var genres = BookModuleHelpers.GetGenresList(4).AsEnumerable();

            var result = genres.Map();

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<LiteratureGenre>>>().And
                .HaveCount(4);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPaging_IQueryableIForListVMLiteratureGenreWithOrderedElements()
        {
            var genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };

            var result = genres.MapToList(ref paging);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<IForListVM<LiteratureGenre>>>().And
                .HaveCount(genres.Count()).And
                .BeInAscendingOrder(g => g.Description);
        }

        [Fact]
        public void MapToList_IEnumerableLiteratureGenreAndPaging_ListIForListVMLiteratureGenreWithOrderedElements()
        {
            var genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsEnumerable();
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };

            var result = genres.MapToList(ref paging);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<LiteratureGenre>>>().And
                .HaveCount(genres.Count()).And
                .BeInAscendingOrder(g => g.Description);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithOrderedValues()
        {
            var genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering();

            var result = genres.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<LiteratureGenre>>>().And
                .HaveCount(genres.Count()).And
                .BeInAscendingOrder(g => g.Description);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFilteringSortByDescending_ListGenreForListVMWithValuesInDescOrder()
        {
            var genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = genres.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<List<IForListVM<LiteratureGenre>>>().And
                .HaveCount(genres.Count()).And
                .BeInDescendingOrder(g => g.Description);
        }
    }
}