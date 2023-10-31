// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class AuthorForListVM_MappingExtensionsTests
    {
        [Fact]
        public void Map_Author_AuthorForListVM()
        {
            var author = Helpers.GetAuthor();

            var result = author.Map();

            result.Should().NotBeNull().And.BeOfType<AuthorForListVM>();
            result.Id.Should().Be(author.Id);
            result.Description.Should().NotBeNullOrWhiteSpace().And.Be(author.Name + " " + author.LastName);
        }

        [Fact]
        public void Map_IQueryableAuthor_IQueryableAuthorForListVM()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count).AsQueryable();

            var result = authors.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<AuthorForListVM>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.Description.Should().Be(author.Name + " " + author.LastName);
            }
        }

        [Fact]
        public void Map_ICollectionAuthor_ListAuthorForListVM()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count) as ICollection<Author>;

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<AuthorForListVM>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.Description.Should().Be(author.Name + " " + author.LastName);
            }
        }

        [Fact]
        public void Map_IEnumerableAuthor_ListAuthorForListVM()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count).AsEnumerable();

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<AuthorForListVM>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.Description.Should().Be(author.Name + " " + author.LastName);
            }
        }

        [Fact]
        public void Map_ListAuthor_ListAuthorForListVM()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count);

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<AuthorForListVM>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.Description.Should().Be(author.Name + " " + author.LastName);
            }
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableAuthorAndPaging_IQueryableAuthorForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var authors = Helpers.GetAuthorsList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = authors.MapToList(ref paging);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<AuthorForListVM>>().And.HaveCount(pageCount);
            paging.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            paging.Count.Should().Be(count);
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPaging_IQueryableAuthorForListVMWithOrderedElements()
        {
            IQueryable<Author> authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable(); // After ordering: 2, 5, 1, 3, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };

            var result = authors.MapToList(ref paging);

            result.ElementAt(0).Id.Should().Be(2);
            result.ElementAt(1).Id.Should().Be(5);
            result.ElementAt(2).Id.Should().Be(1);
            result.ElementAt(3).Id.Should().Be(3);
            result.ElementAt(4).Id.Should().Be(4);
            result.ElementAt(0).Description.Should().Be("Amanda Adamska");
            result.ElementAt(1).Description.Should().Be("Piotr Krasowski");
            result.ElementAt(2).Description.Should().Be("Amanda Popiołek");
            result.ElementAt(3).Description.Should().Be("Michał Popiołek");
            result.ElementAt(4).Description.Should().Be("Jerzy Szczur");
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var authors = Helpers.GetAuthorsList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Name" };

            var result = authors.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForListVM>();
            result.Values.Should().HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithOrderedElements()
        {
            IQueryable<Author> authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable(); // After ordering: 2, 5, 1, 3, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };

            var result = authors.MapToList(paging, new Filtering());

            result.Values.ElementAt(0).Id.Should().Be(2);
            result.Values.ElementAt(1).Id.Should().Be(5);
            result.Values.ElementAt(2).Id.Should().Be(1);
            result.Values.ElementAt(3).Id.Should().Be(3);
            result.Values.ElementAt(4).Id.Should().Be(4);
            result.Values.ElementAt(0).Description.Should().Be("Amanda Adamska");
            result.Values.ElementAt(1).Description.Should().Be("Piotr Krasowski");
            result.Values.ElementAt(2).Description.Should().Be("Amanda Popiołek");
            result.Values.ElementAt(3).Description.Should().Be("Michał Popiołek");
            result.Values.ElementAt(4).Description.Should().Be("Jerzy Szczur");
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithFilteredElements()
        {
            IQueryable<Author> authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Ascending, SearchString = "Popiołek" };

            var result = authors.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForListVM>();
            result.Values.Should().NotBeNull().And.HaveCount(2);
            result.Values.ElementAt(0).Id.Should().Be(1);
            result.Values.ElementAt(1).Id.Should().Be(3);
            result.Values.ElementAt(0).Description.Should().Be("Amanda Popiołek");
            result.Values.ElementAt(1).Description.Should().Be("Michał Popiołek");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithElementsOrderedDescending()
        {
            IQueryable<Author> authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable(); // After ordering: 2, 5, 1, 3, 4 - dasc: 4, 3, 1, 5, 2
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = authors.MapToList(paging, filtering);

            result.Values.ElementAt(0).Id.Should().Be(4);
            result.Values.ElementAt(1).Id.Should().Be(3);
            result.Values.ElementAt(2).Id.Should().Be(1);
            result.Values.ElementAt(3).Id.Should().Be(5);
            result.Values.ElementAt(4).Id.Should().Be(2);
            result.Values.ElementAt(0).Description.Should().Be("Jerzy Szczur");
            result.Values.ElementAt(1).Description.Should().Be("Michał Popiołek");
            result.Values.ElementAt(2).Description.Should().Be("Amanda Popiołek");
            result.Values.ElementAt(3).Description.Should().Be("Piotr Krasowski");
            result.Values.ElementAt(4).Description.Should().Be("Amanda Adamska");
        }
    }
}