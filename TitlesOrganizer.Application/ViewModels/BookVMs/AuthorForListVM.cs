using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorForListVM : BaseForListVM<Author>, IForListVM<Author>
    {
        [DisplayName("Author")]
        public override string Description { get; set; } = null!;
    }

    public class ListAuthorForListVM : BaseListVM<Author>, IListVM<Author>
    {
        [DisplayName("Authors")]
        public override List<IForListVM<Author>> Values { get; set; } = new List<IForListVM<Author>>();
    }

    public static partial class MappingExtensions
    {
        public static AuthorForListVM Map(this Author author)
        {
            return new AuthorForListVM()
            {
                Id = author.Id,
                Description = author.Name + " " + author.LastName
            };
        }

        public static IForListVM<T> Map<T>(this Author author) where T : Author
        {
            return (IForListVM<T>)author.Map();
        }

        public static IQueryable<IForListVM<Author>> Map(this IQueryable<Author> items)
        {
            return items.Select(it => it.Map());
        }

        public static List<IForListVM<Author>> Map(this IEnumerable<Author> items)
        {
            return items.Select(it => it.Map<Author>()).ToList();
        }

        public static ListAuthorForListVM MapToList(this IQueryable<Author> authors, Paging paging, Filtering filtering)
        {
            return (ListAuthorForListVM)authors
                .Sort(filtering.SortBy, a => a.LastName, a => a.Name)
                .Map()
                .MapToList<Author, ListAuthorForListVM>(paging, filtering);
        }

        public static IQueryable<IForListVM<Author>> MapToList(this IQueryable<Author> authors, ref Paging paging)
        {
            return authors
                .OrderBy(a => a.LastName).ThenBy(a => a.Name)
                .Map()
                .MapToList<Author>(ref paging);
        }

        public static List<IForListVM<Author>> MapToList(this IEnumerable<Author> authors, ref Paging paging)
        {
            return authors
                .OrderBy(a => a.LastName).ThenBy(a => a.Name)
                .Map()
                .MapToList<Author>(ref paging);
        }
    }
}