// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Tests.ViewModels.Base
{
    public class MappingExtensionsTests
    {
        [Fact]
        public void MapForItemToDoubleList_CheckAssigning()
        {
            var notSelected = ItemForItem.GetItemList(4).AsQueryable();
            var selected = new List<IForItemVM<Item, Item>>
            {
                new ItemForItem()
                {
                    Id = 5,
                    Description = "Desc5",
                    IsForItem = true
                }
            }.AsQueryable();
            var items = notSelected.Concat(selected);
            IForListVM<Item> item = ItemForList.GetItem();
            var paging = new Paging() { CurrentPage = 1, PageSize = 5 };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Desc" };

            var result = items.MapForItemToDoubleList<Item, Item, ListItemForItem>(item, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListItemForItem>();
            result.Paging.Should().NotBeNull().And.Be(paging);
            result.Paging.Count.Should().Be(4);
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(5);
            result.Values.Should()
                .NotBeNull().And
                .HaveCount(4).And
                .Equal(notSelected);
            result.SelectedValues.Should()
                .NotBeNull().And
                .HaveCount(1).And
                .Equal(selected);
            result.Filtering.Should().NotBeNull();
            result.Filtering.SortBy.Should().Be(SortByEnum.Descending);
            result.Filtering.SearchString.Should().Be("Desc");
            result.Item.Should().NotBeNull().And.Be(item);
            result.Item.Id.Should().Be(1);
            result.Item.Description.Should().Be("Desc1");
        }

        [Fact]
        public void MapForItemToDoubleList_CheckSearching()
        {
            var it1 = new ItemForItem() { Id = 1, Description = "Action", IsForItem = false };
            var it2 = new ItemForItem() { Id = 2, Description = "Crime Comedy", IsForItem = false };
            var it3 = new ItemForItem() { Id = 3, Description = "Crime Story", IsForItem = false };
            var it4 = new ItemForItem() { Id = 4, Description = "Fantasy", IsForItem = false };
            var items = new List<IForItemVM<Item, Item>> { it1, it2, it3, it4 }.AsQueryable();
            IForListVM<Item> item = new ItemForList();
            var paging = new Paging() { CurrentPage = 1, PageSize = 4 };
            var filtering = new Filtering() { SearchString = "rim" };

            var result = items.MapForItemToDoubleList<Item, Item, ListItemForItem>(item, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListItemForItem>();
            result.Paging.Should().NotBeNull().And.Be(paging);
            result.Paging.Count.Should().Be(2);
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(4);
            result.Values.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Equal(it2, it3);
            result.SelectedValues.Should().NotBeNull().And.BeEmpty();
            result.Filtering.Should().NotBeNull().And.Be(filtering);
            result.Filtering.SortBy.Should().Be(default);
            result.Filtering.SearchString.Should().Be("rim");
        }

        [Fact]
        public void MapForItemToDoubleList_CheckSplitIntoSelectedAndNot()
        {
            var it1 = new ItemForItem() { Id = 1, Description = "Action", IsForItem = true };
            var it2 = new ItemForItem() { Id = 2, Description = "Crime Comedy", IsForItem = false };
            var it3 = new ItemForItem() { Id = 3, Description = "Crime Story", IsForItem = true };
            var it4 = new ItemForItem() { Id = 4, Description = "Fantasy", IsForItem = false };
            var items = new List<IForItemVM<Item, Item>> { it1, it2, it3, it4 }.AsQueryable();
            IForListVM<Item> item = ItemForList.GetItem();
            var paging = new Paging() { CurrentPage = 1, PageSize = 4 };
            var filtering = new Filtering();

            var result = items.MapForItemToDoubleList<Item, Item, ListItemForItem>(item, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListItemForItem>();
            result.Paging.Should().NotBeNull().And.Be(paging);
            result.Paging.Count.Should().Be(2);
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(4);
            result.Values.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Equal(it2, it4);
            result.SelectedValues.Should()
                .NotBeNull().And
                .HaveCount(2).And
                .Equal(it1, it3);
            result.Filtering.Should().NotBeNull().And.Be(filtering);
        }

        [Fact]
        public void MapForItemToList_CheckAssigning()
        {
            var notSelected = ItemForItem.GetItemList(4).AsQueryable();
            var selected = new List<IForItemVM<Item, Item>>
            {
                new ItemForItem()
                {
                    Id = 5,
                    Description = "Desc5",
                    IsForItem = true
                }
            }.AsQueryable();
            var items = notSelected.Concat(selected);
            var sorted = selected.Concat(notSelected);
            IForListVM<Item> item = ItemForList.GetItem();
            var paging = new Paging() { CurrentPage = 1, PageSize = 5 };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Desc" };

            var result = items.MapForItemToList<Item, Item, ListItemForItem>(item, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListItemForItem>();
            result.Paging.Should().NotBeNull().And.Be(paging);
            result.Paging.Count.Should().Be(5);
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(5);
            result.Values.Should()
                .NotBeNull().And
                .HaveCount(5).And
                .Equal(sorted);
            result.SelectedValues.Should().BeNullOrEmpty();
            result.Filtering.Should().NotBeNull();
            result.Filtering.SortBy.Should().Be(SortByEnum.Descending);
            result.Filtering.SearchString.Should().Be("Desc");
            result.Item.Should().NotBeNull().And.Be(item);
            result.Item.Id.Should().Be(1);
            result.Item.Description.Should().Be("Desc1");
        }

        [Fact]
        public void MapForItemToList_CheckSearching()
        {
            var it1 = new ItemForItem() { Id = 1, Description = "Action", IsForItem = false };
            var it2 = new ItemForItem() { Id = 2, Description = "Crime Comedy", IsForItem = false };
            var it3 = new ItemForItem() { Id = 3, Description = "Crime Story", IsForItem = false };
            var it4 = new ItemForItem() { Id = 4, Description = "Fantasy", IsForItem = true };
            var items = new List<IForItemVM<Item, Item>> { it1, it2, it3, it4 }.AsQueryable();
            IForListVM<Item> item = new ItemForList();
            var paging = new Paging() { CurrentPage = 1, PageSize = 4 };
            var filtering = new Filtering() { SearchString = "rim" };

            var result = items.MapForItemToList<Item, Item, ListItemForItem>(item, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListItemForItem>();
            result.Paging.Should().NotBeNull().And.Be(paging);
            result.Paging.Count.Should().Be(3);
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(4);
            result.Values.Should()
                .NotBeNull().And
                .HaveCount(3).And
                .Equal(it4, it2, it3);
            result.SelectedValues.Should().BeNullOrEmpty();
            result.Filtering.Should().NotBeNull().And.Be(filtering);
            result.Filtering.SortBy.Should().Be(default);
            result.Filtering.SearchString.Should().Be("rim");
        }

        [Fact]
        public void MapForItemToList_CheckSelectedNotSelectedSegregation()
        {
            var it1 = new ItemForItem() { Id = 1, Description = "Action", IsForItem = true };
            var it2 = new ItemForItem() { Id = 2, Description = "Crime Comedy", IsForItem = false };
            var it3 = new ItemForItem() { Id = 3, Description = "Crime Story", IsForItem = true };
            var it4 = new ItemForItem() { Id = 4, Description = "Fantasy", IsForItem = false };
            var items = new List<IForItemVM<Item, Item>> { it1, it2, it3, it4 }.AsQueryable();
            IForListVM<Item> item = new ItemForList();
            var paging = new Paging() { CurrentPage = 1, PageSize = 4 };
            var filtering = new Filtering();

            var result = items.MapForItemToList<Item, Item, ListItemForItem>(item, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListItemForItem>();
            result.Paging.Should().NotBeNull().And.Be(paging);
            result.Paging.Count.Should().Be(4);
            result.Paging.CurrentPage.Should().Be(1);
            result.Paging.PageSize.Should().Be(4);
            result.Values.Should()
                .NotBeNull().And
                .HaveCount(4).And
                .Equal(it1, it3, it2, it4);
            result.SelectedValues.Should().BeNullOrEmpty();
            result.Filtering.Should().NotBeNull().And.Be(filtering);
        }

        [Theory]
        [InlineData(5, 1, 3, 3, new[] { 1, 2, 3 }, 1)]
        [InlineData(5, 2, 3, 2, new[] { 4, 5 }, 2)]
        [InlineData(0, 2, 3, 0, new int[] { }, 1)]
        public void SkipAndTake_List_CheckPaging(int count, int pageNo, int pageSize, int pageCount, int[] ids, int currPage)
        {
            var items = ItemForList.GetItemList(count);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = items.SkipAndTake(ref paging);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<List<IForListVM<Item>>>().And
                .HaveCount(pageCount).And
                .Equal(ids, (it, id) => it.Equals(id)); ;
            paging.Should().NotBeNull();
            paging.Count.Should().Be(count);
            paging.CurrentPage.Should().Be(currPage);
            paging.PageSize.Should().Be(pageSize);
        }
    }

    internal class Item : IBaseModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public static Item GetItem(int id = 1)
        {
            return new Item()
            {
                Id = id,
                Description = $"Desc{id}"
            };
        }

        public static List<Item> GetItemList(int count)
        {
            var list = new List<Item>();
            for (int i = 0; i < count; i++)
            {
                list.Add(GetItem(i));
            }

            return list;
        }
    }

    internal class ItemForList : IForListVM<Item>
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public override bool Equals(object? obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is ItemForList)
            {
                var it = obj as ItemForList;
                return Id == it.Id && Description == it.Description;
            }
            else if (obj is ItemForItem)
            {
                var it = obj as ItemForItem;
                return Id == it.Id && Description == it.Description;
            }
            else if (obj is Item)
            {
                var it = obj as Item;
                return Id == it.Id && Description == it.Description;
            }
            else if (obj is int)
            {
                return Id == (int)obj;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public static ItemForList GetItem(int id = 1)
        {
            return new ItemForList()
            {
                Id = id,
                Description = $"Desc{id}"
            };
        }

        public static List<IForListVM<Item>> GetItemList(int count)
        {
            var list = new List<IForListVM<Item>>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(GetItem(i));
            }

            return list;
        }
    }

    internal class ItemForItem : IForItemVM<Item, Item>
    {
        public bool IsForItem { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }

        public override bool Equals(object? obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is ItemForList)
            {
                var it = obj as ItemForList;
                return Id == it.Id && Description == it.Description;
            }
            else if (obj is ItemForItem)
            {
                var it = obj as ItemForItem;
                return Id == it.Id && Description == it.Description && IsForItem == it.IsForItem;
            }
            else if (obj is Item)
            {
                var it = obj as Item;
                return Id == it.Id && Description == it.Description;
            }
            else if (obj is int)
            {
                return Id == (int)obj;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public static ItemForItem GetItem(int id = 1)
        {
            return new ItemForItem()
            {
                Id = id,
                Description = $"Desc{id}",
                IsForItem = false
            };
        }

        public static List<ItemForItem> GetItemList(int count)
        {
            var list = new List<ItemForItem>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(GetItem(i));
            }

            return list;
        }
    }

    internal class ListItemForItem : IListForItemVM<Item, Item>, IDoubleListForItemVM<Item, Item>
    {
        public IForListVM<Item> Item { get; set; }
        public List<IForItemVM<Item, Item>> Values { get; set; }
        public Paging Paging { get; set; }
        public Filtering Filtering { get; set; }
        public List<IForItemVM<Item, Item>> SelectedValues { get; set; }
    }

    internal class ListItemForList : IListVM<Item>
    {
        public List<IForListVM<Item>> Values { get; set; }
        public Paging Paging { get; set; }
        public Filtering Filtering { get; set; }
    }
}