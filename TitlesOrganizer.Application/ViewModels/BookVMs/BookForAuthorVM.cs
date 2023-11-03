using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookForAuthorVM : BaseForItemVM<Book, Author>, IForItemVM<Book, Author>
    {
        [DisplayName("Book")]
        public override string Description { get; set; } = null!;
    }

    public class ListBookForAuthorVM : IDoubleListForItemVM<Book, Author>
    {
        [DisplayName("Author")]
        public IForListVM<Author> Item { get; set; } = new AuthorForListVM();

        [DisplayName("Previously selected books")]
        public List<IForItemVM<Book, Author>> SelectedValues { get; set; } = new List<IForItemVM<Book, Author>>();

        [DisplayName("Other books")]
        public List<IForItemVM<Book, Author>> Values { get; set; } = new List<IForItemVM<Book, Author>>();

        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IForItemVM<Book, Author> MapForItem(this Book bookWithAuthors, Author author)
        {
            return new BookForAuthorVM
            {
                Id = bookWithAuthors.Id,
                Description = bookWithAuthors.Title,
                IsForItem = bookWithAuthors.Authors.Any(a => a.Id == author.Id)
            };
        }

        public static IQueryable<IForItemVM<Book, Author>> MapForItem(this IQueryable<Book> sortedList, Author item)
        {
            return sortedList.Select(it => it.MapForItem(item));
        }

        public static ListBookForAuthorVM MapForItemToList(this IQueryable<Book> booksWithAuthors, Author author, Paging paging, Filtering filtering)
        {
            return booksWithAuthors
                .Sort(filtering.SortBy, b => b.Title)
                .MapForItem(author)
                .MapForItemToDoubleList<Book, Author, ListBookForAuthorVM>(author.Map(), paging, filtering);
        }
    }
}