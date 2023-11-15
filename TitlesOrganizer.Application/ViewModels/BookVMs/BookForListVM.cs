using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookForListVM : BaseForListVM<Book>, IForListVM<Book>
    {
        [DisplayName("Book")]
        public override string Description { get; set; } = null!;
    }

    public class ListBookForListVM : BaseListVM<Book>, IListVM<Book>
    {
        [DisplayName("Books")]
        public override List<IForListVM<Book>> Values { get; set; }

        public ListBookForListVM()
        {
            Values = new List<IForListVM<Book>>();
        }

        public ListBookForListVM(List<IForListVM<Book>> values, Paging paging, Filtering filtering)
        {
            Values = values;
            Paging = paging;
            Filtering = filtering;
        }
    }

    public static partial class MappingExtensions
    {
        public static BookForListVM Map(this Book book)
        {
            return new BookForListVM()
            {
                Id = book.Id,
                Description = book.Title
            };
        }

        public static IQueryable<IForListVM<Book>> Map(this IQueryable<Book> items)
        {
            return items.Select(it => it.Map());
        }

        public static List<IForListVM<Book>> Map(this IEnumerable<Book> items)
        {
            return items.Select(it => (IForListVM<Book>)it.Map()).ToList();
        }

        public static ListBookForListVM MapToList(this IQueryable<Book> books, Paging paging, Filtering filtering)
        {
            var values = books
                .Sort(filtering.SortBy, b => b.Title)
                .Where(b => b.Title.Contains(filtering.SearchString))
                .Map()
                .SkipAndTake(ref paging)
                .ToList();

            return new ListBookForListVM(values, paging, filtering);
        }

        public static List<IForListVM<Book>> MapToList(this IEnumerable<Book> books, ref Paging paging)
        {
            return books
                .OrderBy(b => b.Title)
                .Map()
                .SkipAndTake(ref paging)
                .ToList();
        }

        public static IPartialList<Book> MapToPartialList(this IQueryable<Book> books, Paging paging)
        {
            return new PartialList<Book>()
            {
                Values = books.MapToList(ref paging).ToList(),
                Paging = paging
            };
        }

        public static IPartialList<Book> MapToPartialList(this IEnumerable<Book> books, Paging paging)
        {
            return new PartialList<Book>()
            {
                Values = books.MapToList(ref paging),
                Paging = paging
            };
        }
    }
}