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
        public override List<IForListVM<Book>> Values { get; set; } = new List<IForListVM<Book>>();
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

        public static IForListVM<T> Map<T>(this Book book) where T : Book
        {
            return (IForListVM<T>)book.Map();
        }

        public static IQueryable<IForListVM<Book>> Map(this IQueryable<Book> items)
        {
            return items.Select(it => it.Map());
        }

        public static List<IForListVM<Book>> Map(this IEnumerable<Book> items)
        {
            return items.Select(it => it.Map<Book>()).ToList();
        }

        public static ListBookForListVM MapToList(this IQueryable<Book> books, Paging paging, Filtering filtering)
        {
            return (ListBookForListVM)books
                .Sort(filtering.SortBy, b => b.Title)
                .Map()
                .ToList()
                .MapToList<Book, ListBookForListVM>(paging, filtering);
        }

        public static IQueryable<IForListVM<Book>> MapToList(this IQueryable<Book> books, ref Paging paging)
        {
            return books
                .OrderBy(b => b.Title)
                .Map()
                .MapToList<Book>(ref paging);
        }

        public static List<IForListVM<Book>> MapToList(this IEnumerable<Book> books, ref Paging paging)
        {
            return books
                .OrderBy(b => b.Title)
                .Map()
                .MapToList<Book>(ref paging);
        }

        public static IPartialList<Book> MapToPartialList(this IQueryable<Book> books, Paging paging)
        {
            return new PartialList<Book>()
            {
                Values = books.MapToList(ref paging).ToList(),
                Paging = paging
            };
        }
    }
}