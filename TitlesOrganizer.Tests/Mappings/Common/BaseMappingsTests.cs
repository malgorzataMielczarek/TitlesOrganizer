// Ignore Spelling: Queryable

using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Mappings.Common;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Tests.Mappings.Common
{
    public class BaseMappingsTests
    {
        [Theory]
        [InlineData(5, 1, 3, 3, new[] { 1, 2, 3 }, 1)]
        [InlineData(5, 2, 3, 2, new[] { 4, 5 }, 2)]
        [InlineData(0, 2, 3, 0, new int[] { }, 1)]
        public void SkipAndTake_IEnumerable_CheckPaging(int count, int pageNo, int pageSize, int pageCount, int[] expectedResult, int currPage)
        {
            IEnumerable<int> items = Enumerable.Range(1, count);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = BaseMappings.SkipAndTake(items, ref paging);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<List<int>>().And
                .HaveCount(pageCount).And
                .Equal(expectedResult); ;
            paging.Should().NotBeNull();
            paging.Count.Should().Be(count);
            paging.CurrentPage.Should().Be(currPage);
            paging.PageSize.Should().Be(pageSize);
        }

        [Fact]
        public void Filter_IQueryableEntity_IQueryableEntity()
        {
            var entities = Item.GetItemList(5).AsQueryable();
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);

            var result = mappings.Filter(entities, "");

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IQueryable<Item>>().And
                .NotBeEmpty().And
                .Equal(entities);
        }

        [Fact]
        public void Map_Entity_IForListVM()
        {
            var id = 1;
            var entity = new Item(id);
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);

            var result = mappings.Map(entity);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM>();
            result.Id.Should().Be(id);
        }

        [Fact]
        public void Map_IEnumerableEntity_ListIForListVM()
        {
            int count = 5;
            var entities = Item.GetItemList(count);
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);
            var rnd = new Random();

            var result = mappings.Map(entities.OrderBy(e => rnd.Next()));

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<List<IForListVM>>().And
                .HaveCount(count).And
                .NotContainNulls().And
                .Equal(entities, (it, entity) => it.Id == entity.Id);
        }

        [Fact]
        public void Map_EntityAndItem_IForItemVM()
        {
            var id = 1;
            var entity = new Item(id);
            var item = new Item(5);
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);

            var result = mappings.Map(entity, item);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(id);
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void Map_IEnumerableEntityAndItem_ListIForItemVM()
        {
            int count = 5;
            var rnd = new Random();
            var entities = Item.GetItemList(count).OrderBy(e => rnd.Next()).ToList().AsEnumerable();
            var item = new Item(8);
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);

            var result = mappings.Map(entities, item);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<List<IForItemVM>>().And
                .HaveCount(count).And
                .NotContainNulls().And
                .AllBeAssignableTo<IForItemVM>().And
                .Equal(entities, (it, entity) => it.Id == entity.Id).And
                .AllSatisfy(a => a.IsForItem.Should().BeFalse());
        }

        [Fact]
        public void Map_IQueryableEntityAndPagingAndFiltering_IListVM()
        {
            var paging = new Paging() { CurrentPage = 2, PageSize = 3 };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Test" };
            var entities = Item.GetItemList(5).AsQueryable();
            var mapper = new Mock<IMapper>();
            var mapping = new BaseMappings(mapper.Object);

            var result = mapping.Map(entities, paging, filtering);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IListVM>();
            result.Values.Should()
                .NotBeNull().And
                .BeAssignableTo<List<IForListVM>>().And
                .HaveCount(2).And
                .NotContainNulls().And
                .AllBeAssignableTo<IForListVM>().And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(2);
                    },
                    second =>
                    {
                        second.Id.Should().Be(1);
                    }
                );
            result.Paging.Should()
                .NotBeNull().And
                .Be(paging);
            result.Paging.CurrentPage.Should().Be(2);
            result.Paging.PageSize.Should().Be(3);
            result.Paging.Count.Should().Be(5);
            result.Filtering.Should()
                .NotBeNull().And
                .Be(filtering);
            result.Filtering.SearchString.Should()
                .NotBeNull().And
                .Be("Test");
            result.Filtering.SortBy.Should().Be(SortByEnum.Descending);
        }

        [Fact]
        public void Map_IEnumerableEntityAndRefPaging_ListIForListVM()
        {
            var entities = Item.GetItemList(5);
            var paging = new Paging() { CurrentPage = 2, PageSize = 3 };
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);

            var result = mappings.Map(entities, ref paging);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<List<IForListVM>>().And
                .HaveCount(2).And
                .AllBeAssignableTo<IForListVM>().And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(4);
                    },
                    second =>
                    {
                        second.Id.Should().Be(5);
                    }
                );
            paging.Should().NotBeNull();
            paging.CurrentPage.Should().Be(2);
            paging.PageSize.Should().Be(3);
            paging.Count.Should().Be(5);
        }

        [Fact]
        public void Map_IEnumerableEntityAndPaging_IPartialListVM()
        {
            var entities = Item.GetItemList(5);
            var paging = new Paging() { CurrentPage = 2, PageSize = 3 };
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);

            var result = mappings.Map(entities, paging);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IPartialListVM>();
            result.Values.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(2).And
                .AllBeAssignableTo<IForListVM>().And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(4);
                    },
                    second =>
                    {
                        second.Id.Should().Be(5);
                    }
                );
            result.Paging.Should()
                .NotBeNull().And
                .Be(paging);
            result.Paging.CurrentPage.Should().Be(2);
            result.Paging.PageSize.Should().Be(3);
            result.Paging.Count.Should().Be(5);
        }

        [Fact]
        public void MapToDoubleListForItem_IQueryableEntitiesAndItemAndPagingAndFiltering_IDoubleListForItemVM()
        {
            var entities = Item.GetItemList(5).AsQueryable();
            var item = new Item(2);
            var paging = new Paging() { CurrentPage = 2, PageSize = 1 };
            var filtering = new Filtering() { SearchString = "Test", SortBy = SortByEnum.Descending };
            var mapper = new Mock<IMapper>();
            var mappings = new MapForTest(mapper.Object);
            // 1,2,3,4,5 -sort-> 5,4,3,2,1 -divide-> s:4,2; v:5,3,1 -paging-> s:4,2; v:3

            var result = mappings.MapToDoubleListForItem(entities, item, paging, filtering);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IDoubleListForItemVM>();
            result.Values.Should()
                .NotBeNull().And
                .HaveCount(1).And
                .AllBeAssignableTo<IForItemVM>().And
                .Satisfy(v => v.Id == 3);
            result.SelectedValues.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .AllBeAssignableTo<IForItemVM>().And
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(4);
                    },
                    second =>
                    {
                        second.Id.Should().Be(2);
                    }
                );
            result.Paging.Should()
                .NotBeNull().And
                .Be(paging);
            result.Paging.CurrentPage.Should().Be(2);
            result.Paging.PageSize.Should().Be(1);
            result.Paging.Count.Should().Be(3);
            result.Filtering.Should()
                .NotBeNull().And
                .Be(filtering);
            result.Filtering.SearchString.Should().Be("Test");
            result.Filtering.SortBy.Should().Be(SortByEnum.Descending);
            result.Item.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM>();
            result.Item.Id.Should().Be(item.Id);
        }

        [Theory]
        [InlineData(4, new[] { 4, 3, 2 }, 8)]// 1,2,3,4,5,6 -sort-> 4,6,5,3,2,1 -paging-> 4,3,2
        [InlineData(2, new[] { 6, 4, 2, 3 }, 12)]// 1,2,3,4,5,6 -sort-> 6,4,2,5,3,1 -paging-> 6,4,2,3
        public void MapToListForItem_IQueryableEntitiesAndItemAndPagingAndFiltering_IListForItemVM(int itemId, int[] values, int count)
        {
            var entities = Item.GetItemList(6).AsQueryable();
            var item = new Item(itemId);
            var paging = new Paging() { CurrentPage = 2, PageSize = 3 };
            var filtering = new Filtering() { SearchString = "Test", SortBy = SortByEnum.Descending };
            var mapper = new Mock<IMapper>();
            var mappings = new MapForTest(mapper.Object);

            var result = mappings.MapToListForItem(entities, item, paging, filtering);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IListForItemVM>();
            result.Values.Should()
                .NotBeNull().And
                .HaveCount(values.Length).And
                .AllBeAssignableTo<IForItemVM>().And
                .Equal(values, (v, id) => v.Id == id);
            result.Paging.Should()
                .NotBeNull().And
                .Be(paging);
            result.Paging.CurrentPage.Should().Be(2);
            result.Paging.PageSize.Should().Be(3);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should()
                .NotBeNull().And
                .Be(filtering);
            result.Filtering.SearchString.Should().Be("Test");
            result.Filtering.SortBy.Should().Be(SortByEnum.Descending);
            result.Item.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM>();
            result.Item.Id.Should().Be(item.Id);
        }

        [Fact]
        public void Sort_IQueryableEntityAndSortByAsc_IOrderedQueryableEntity()
        {
            var rnd = new Random();
            var entities = Item.GetItemList(5).OrderBy(e => rnd.Next()).AsQueryable();
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);

            var result = mappings.Sort(entities, SortByEnum.Ascending);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IOrderedQueryable<Item>>().And
                .BeInAscendingOrder(it => it.Id);
        }

        [Fact]
        public void Sort_IQueryableEntityAndSortByDesc_IOrderedQueryableEntity()
        {
            var rnd = new Random();
            var entities = Item.GetItemList(5).OrderBy(e => rnd.Next()).AsQueryable();
            var mapper = new Mock<IMapper>();
            var mappings = new BaseMappings(mapper.Object);

            var result = mappings.Sort(entities, SortByEnum.Descending);

            mapper.VerifyNoOtherCalls();
            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IOrderedQueryable<Item>>().And
                .BeInDescendingOrder(it => it.Id);
        }
    }

    internal class Item(int id) : BaseModel
    {
        public int Id { get; set; } = id;

        public static List<Item> GetItemList(int count)
        {
            var list = new List<Item>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Item(i));
            }

            return list;
        }
    }

    internal class MapForTest(IMapper mapper) : BaseMappings(mapper)
    {
        public override IForItemVM Map<T, ItemT>(T entity, ItemT item)
        {
            return new ForItemVM()
            {
                Id = entity.Id,
                IsForItem = entity.Id % item.Id == 0,
                Description = "Test" + entity.Id
            };
        }
    }
}