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
            return (IForListVM<T>)new BookForListVM()
            {
                Id = book.Id,
                Description = book.Title
            };
        }

        public static ListBookForListVM MapToList(this IQueryable<Book> books, Paging paging, Filtering filtering)
        {
            return (ListBookForListVM)books
                .Sort(filtering.SortBy, b => b.Title)
                .MapToList<Book>(paging, filtering);
        }

        public static IQueryable<IForListVM<Book>> MapToList(this IQueryable<Book> books, ref Paging paging)
        {
            return books
                .OrderBy(b => b.Title)
                .MapToList<Book>(ref paging);
        }
    }
}