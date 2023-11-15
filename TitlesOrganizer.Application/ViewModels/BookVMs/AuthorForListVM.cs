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
        public override List<IForListVM<Author>> Values { get; set; }

        public ListAuthorForListVM()
        {
            Values = new List<IForListVM<Author>>();
        }

        public ListAuthorForListVM(List<IForListVM<Author>> values, Paging paging, Filtering filtering)
        {
            Values = values;
            Paging = paging;
            Filtering = filtering;
        }
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

        public static IQueryable<IForListVM<Author>> Map(this IQueryable<Author> items)
        {
            return items.Select(it => it.Map());
        }

        public static List<IForListVM<Author>> Map(this IEnumerable<Author> items)
        {
            return items.Select(it => (IForListVM<Author>)it.Map()).ToList();
        }

        public static ListAuthorForListVM MapToList(this IQueryable<Author> authors, Paging paging, Filtering filtering)
        {
            var values = authors
                .Sort(filtering.SortBy, a => a.LastName, a => a.Name)
                .Where(a => (a.Name + " " + a.LastName).Contains(filtering.SearchString))
                .SkipAndTake(ref paging)
                .Map();

            return new ListAuthorForListVM(values, paging, filtering);
        }

        public static List<IForListVM<Author>> MapToList(this IEnumerable<Author> authors, ref Paging paging)
        {
            return authors
                .OrderBy(a => a.LastName).ThenBy(a => a.Name)
                .SkipAndTake(ref paging)
                .Map();
        }

        public static IPartialList<Author> MapToPartialList(this IEnumerable<Author> authors, Paging paging)
        {
            return new PartialList<Author>()
            {
                Values = authors.MapToList(ref paging),
                Paging = paging
            };
        }
    }
}