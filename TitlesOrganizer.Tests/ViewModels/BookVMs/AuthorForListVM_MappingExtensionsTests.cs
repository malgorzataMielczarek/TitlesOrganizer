// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
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
            result.Description.Should().NotBeNullOrWhiteSpace().And.Be($"{author.Name} {author.LastName}");
        }

        [Fact]
        public void Map_Author_IForListVMAuthor()
        {
            var author = Helpers.GetAuthor();

            var result = author.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IForListVM<Author>>();
            result.Id.Should().Be(author.Id);
            result.Description.Should().NotBeNullOrWhiteSpace().And.Be($"{author.Name} {author.LastName}");
        }

        [Fact]
        public void Map_IQueryableAuthor_IQueryableIForListVMAuthor()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count).AsQueryable();

            var result = authors.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<IForListVM<Author>>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.Description.Should().Be($"{author.Name} {author.LastName}");
            }
        }

        [Fact]
        public void Map_ICollectionAuthor_ListIForListVMAuthor()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count) as ICollection<Author>;

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<IForListVM<Author>>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.Description.Should().Be($"{author.Name} {author.LastName}");
            }
        }

        [Fact]
        public void Map_IEnumerableAuthor_ListIForListVMAuthor()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count).AsEnumerable();

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<IForListVM<Author>>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.Description.Should().Be($"{author.Name} {author.LastName}");
            }
        }

        [Fact]
        public void Map_ListAuthor_ListIForListVMAuthor()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count);

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<IForListVM<Author>>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.Description.Should().Be($"{author.Name} {author.LastName}");
            }
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPaging_IQueryableIForListVMAuthorWithOrderedElements()
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

            result.Should()
                .BeAssignableTo<IQueryable<IForListVM<Author>>>().And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(2);
                        first.Description.Should().Be("Amanda Adamska");
                    },
                    second =>
                    {
                        second.Id.Should().Be(5);
                        second.Description.Should().Be("Piotr Krasowski");
                    },
                    third =>
                    {
                        third.Id.Should().Be(1);
                        third.Description.Should().Be("Amanda Popiołek");
                    },
                    fourth =>
                    {
                        fourth.Id.Should().Be(3);
                        fourth.Description.Should().Be("Michał Popiołek");
                    },
                    fifth =>
                    {
                        fifth.Id.Should().Be(4);
                        fifth.Description.Should().Be("Jerzy Szczur");
                    }
                );
        }

        [Fact]
        public void MapToList_IEnumerableAuthorAndPaging_ListIForListVMAuthorWithOrderedElements()
        {
            var authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsEnumerable(); // After ordering: 2, 5, 1, 3, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };

            var result = authors.MapToList(ref paging);

            result.Should()
                .BeAssignableTo<List<IForListVM<Author>>>().And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(2);
                        first.Description.Should().Be("Amanda Adamska");
                    },
                    second =>
                    {
                        second.Id.Should().Be(5);
                        second.Description.Should().Be("Piotr Krasowski");
                    },
                    third =>
                    {
                        third.Id.Should().Be(1);
                        third.Description.Should().Be("Amanda Popiołek");
                    },
                    fourth =>
                    {
                        fourth.Id.Should().Be(3);
                        fourth.Description.Should().Be("Michał Popiołek");
                    },
                    fifth =>
                    {
                        fifth.Id.Should().Be(4);
                        fifth.Description.Should().Be("Jerzy Szczur");
                    }
                );
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithOrderedElements()
        {
            var authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable(); // After ordering: 2, 5, 1, 3, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering();

            var result = authors.MapToList(paging, filtering);

            result.Should().BeOfType<ListAuthorForListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(authors.Count()).And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(2);
                        first.Description.Should().Be("Amanda Adamska");
                    },
                    second =>
                    {
                        second.Id.Should().Be(5);
                        second.Description.Should().Be("Piotr Krasowski");
                    },
                    third =>
                    {
                        third.Id.Should().Be(1);
                        third.Description.Should().Be("Amanda Popiołek");
                    },
                    fourth =>
                    {
                        fourth.Id.Should().Be(3);
                        fourth.Description.Should().Be("Michał Popiołek");
                    },
                    fifth =>
                    {
                        fifth.Id.Should().Be(4);
                        fifth.Description.Should().Be("Jerzy Szczur");
                    }
                );
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPagingAndFilteringWithSortByDescending_ListAuthorForListVMWithValuesInDescendingOrder()
        {
            var authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable(); // After ordering: 2, 5, 1, 3, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = authors.MapToList(paging, filtering);

            result.Should().BeOfType<ListAuthorForListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(authors.Count()).And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(4);
                        first.Description.Should().Be("Jerzy Szczur");
                    },
                    second =>
                    {
                        second.Id.Should().Be(3);
                        second.Description.Should().Be("Michał Popiołek");
                    },
                    third =>
                    {
                        third.Id.Should().Be(1);
                        third.Description.Should().Be("Amanda Popiołek");
                    },
                    fourth =>
                    {
                        fourth.Id.Should().Be(5);
                        fourth.Description.Should().Be("Piotr Krasowski");
                    },
                    fifth =>
                    {
                        fifth.Id.Should().Be(2);
                        fifth.Description.Should().Be("Amanda Adamska");
                    }
                );
        }
    }