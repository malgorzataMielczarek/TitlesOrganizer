using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorForBookVM : BaseForItemVM<Author, Book>, IForItemVM<Author, Book>
    {
        [DisplayName("Author")]
        public override string Description { get; set; } = null!;
    }

    public class ListAuthorForBookVM : IDoubleListForItemVM<Author, Book>
    {
        [DisplayName("Book")]
        public IForListVM<Book> Item { get; set; } = new BookForListVM();

        [DisplayName("Previously selected authors")]
        public List<IForItemVM<Author, Book>> SelectedValues { get; set; } = new List<IForItemVM<Author, Book>>();

        [DisplayName("Other authors")]
        public List<IForItemVM<Author, Book>> Values { get; set; } = new List<IForItemVM<Author, Book>>();

        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IForItemVM<Author, Book> MapForItem(this Author authorWithBooks, Book book)
        {
            return new AuthorForBookVM()
            {
                Id = authorWithBooks.Id,
                Description = authorWithBooks.Name + " " + authorWithBooks.LastName,
                IsForItem = authorWithBooks.Books.Any(b => b.Id == book.Id)
            };
        }

        public static ListAuthorForBookVM MapForItemToList(this IQueryable<Author> authorsWithBooks, Book book, Paging paging, Filtering filtering)
        {
            return authorsWithBooks
                .Sort(filtering.SortBy, a => a.LastName, a => a.Name)
                .MapForItemToDoubleList<Author, Book, ListAuthorForBookVM>(book, paging, filtering);
        }
    }
}