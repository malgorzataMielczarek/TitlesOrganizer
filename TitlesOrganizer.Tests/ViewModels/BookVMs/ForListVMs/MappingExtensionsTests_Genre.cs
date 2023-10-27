// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs.ForListVMs
{
    public class MappingExtensionsTests_Genre
    {
        [Fact]
        public void Map_LiteratureGenre_GenreForListVM()
        {
            var genre = Helpers.GetGenre();

            var result = genre.Map();

            result.Should().NotBeNull().And.BeOfType<GenreForListVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().NotBeNullOrWhiteSpace().And.Be(genre.Name);
        }

        [Fact]
        public void Map_IQueryableLiteratureGenre_IQueryableGenreForListVM()
        {
            int count = 2;
            var genres = Helpers.GetGenresList(count).AsQueryable();

            var result = genres.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<GenreForListVM>>().And.AllBeOfType<GenreForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(genres.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Name.Should().Be(genres.FirstOrDefault()?.Name);
            result.LastOrDefault()?.Id.Should().Be(genres.LastOrDefault()?.Id);
            result.LastOrDefault()?.Name.Should().Be(genres.LastOrDefault()?.Name);
        }

        [Fact]
        public void Map_ICollectionLiteratureGenre_ListGenreForListVM()
        {
            int count = 2;
            var genres = Helpers.GetGenresList(count) as ICollection<LiteratureGenre>;

            var result = genres.Map();

            result.Should().NotBeNull().And.BeOfType<List<GenreForListVM>>().And.AllBeOfType<GenreForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(genres.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Name.Should().Be(genres.FirstOrDefault()?.Name);
            result.LastOrDefault()?.Id.Should().Be(genres.LastOrDefault()?.Id);
            result.LastOrDefault()?.Name.Should().Be(genres.LastOrDefault()?.Name);
        }

        [Fact]
        public void Map_IEnumerableLiteratureGenre_ListGenreForListVM()
        {
            int count = 2;
            var genres = Helpers.GetGenresList(count).AsEnumerable();

            var result = genres.Map();

            result.Should().NotBeNull().And.BeOfType<List<GenreForListVM>>().And.AllBeOfType<GenreForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(genres.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Name.Should().Be(genres.FirstOrDefault()?.Name);
            result.LastOrDefault()?.Id.Should().Be(genres.LastOrDefault()?.Id);
            result.LastOrDefault()?.Name.Should().Be(genres.LastOrDefault()?.Name);
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableLiteratureGenreAndPaging_IQueryableGenreForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var genres = Helpers.GetGenresList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = genres.MapToList(ref paging);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<GenreForListVM>>().And.AllBeOfType<GenreForListVM>().And.HaveCount(pageCount);
            paging.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            paging.Count.Should().Be(count);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPaging_IQueryableGenreForListVMWithOrderedElements()
        {
            IQueryable<LiteratureGenre> genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable(); // After ordering: 3, 5, 1, 2, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };

            var result = genres.MapToList(ref paging);

            result.ElementAt(0).Id.Should().Be(3);
            result.ElementAt(1).Id.Should().Be(5);
            result.ElementAt(2).Id.Should().Be(1);
            result.ElementAt(3).Id.Should().Be(2);
            result.ElementAt(4).Id.Should().Be(4);
            result.ElementAt(0).Name.Should().Be("Comedy");
            result.ElementAt(1).Name.Should().Be("Fantasy");
            result.ElementAt(2).Name.Should().Be("Horror");
            result.ElementAt(3).Name.Should().Be("SF");
            result.ElementAt(4).Name.Should().Be("Thriller");
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var genres = Helpers.GetGenresList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Name" };

            var result = genres.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForListVM>();
            result.Genres.Should().HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithOrderedElements()
        {
            IQueryable<LiteratureGenre> genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable(); // After ordering: 3, 5, 1, 2, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };

            var result = genres.MapToList(paging, new Filtering());

            result.Genres.ElementAt(0).Id.Should().Be(3);
            result.Genres.ElementAt(1).Id.Should().Be(5);
            result.Genres.ElementAt(2).Id.Should().Be(1);
            result.Genres.ElementAt(3).Id.Should().Be(2);
            result.Genres.ElementAt(4).Id.Should().Be(4);
            result.Genres.ElementAt(0).Name.Should().Be("Comedy");
            result.Genres.ElementAt(1).Name.Should().Be("Fantasy");
            result.Genres.ElementAt(2).Name.Should().Be("Horror");
            result.Genres.ElementAt(3).Name.Should().Be("SF");
            result.Genres.ElementAt(4).Name.Should().Be("Thriller");
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithFilteredElements()
        {
            IQueryable<LiteratureGenre> genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Ascending, SearchString = "y" };

            var result = genres.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForListVM>();
            result.Genres.Should().NotBeNull().And.HaveCount(2);
            result.Genres.ElementAt(0).Id.Should().Be(3);
            result.Genres.ElementAt(1).Id.Should().Be(5);
            result.Genres.ElementAt(0).Name.Should().Be("Comedy");
            result.Genres.ElementAt(1).Name.Should().Be("Fantasy");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithElementsOrderedDescending()
        {
            IQueryable<LiteratureGenre> genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable(); // After ordering: 3, 5, 1, 2, 4 - desc: 4, 2, 1, 5, 3
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = genres.MapToList(paging, filtering);

            result.Genres.ElementAt(0).Id.Should().Be(4);
            result.Genres.ElementAt(1).Id.Should().Be(2);
            result.Genres.ElementAt(2).Id.Should().Be(1);
            result.Genres.ElementAt(3).Id.Should().Be(5);
            result.Genres.ElementAt(4).Id.Should().Be(3);
            result.Genres.ElementAt(0).Name.Should().Be("Thriller");
            result.Genres.ElementAt(1).Name.Should().Be("SF");
            result.Genres.ElementAt(2).Name.Should().Be("Horror");
            result.Genres.ElementAt(3).Name.Should().Be("Fantasy");
            result.Genres.ElementAt(4).Name.Should().Be("Comedy");
        }
    }
}