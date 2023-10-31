using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorForBookVM : IForItemVM<Author, Book>
    {
        public bool IsForItem { get; set; }
        public int Id { get; set; }

        [DisplayName("Author")]
        public string Description { get; set; } = null!;
    }

    public class ListAuthorForBookVM : IDoubleListForItemVM<Author, Book>
    {
        [DisplayName("Book")]
        public IForListVM<Book> Item { get; set; } = new BookForListVM();

        [DisplayName("Previously selected authors")]
        public List<AuthorForBookVM> SelectedValues { get; set; } = new List<AuthorForBookVM>();

        [DisplayName("Other authors")]
        public List<AuthorForBookVM> Values { get; set; } = new List<AuthorForBookVM>();

        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IQueryable<AuthorForBookVM> MapForBook(this IQueryable<Author> authorsWithBooks, int bookId)
        {
            return authorsWithBooks.Select(a => new AuthorForBookVM()
            {
                Id = a.Id,
                Description = a.Name + " " + a.LastName,
                IsForItem = a.Books.Any(b => b.Id == bookId)
            });
        }

        public static ListAuthorForBookVM MapForBookToList(this IQueryable<Author> authorsWithBooks, Book book, Paging paging, Filtering filtering)
        {
            var query = authorsWithBooks
                .Sort(filtering.SortBy, a => a.LastName, a => a.Name)
                .MapForBook(book.Id);
            var selectedAuthors = query.Where(a => a.IsForItem).ToList();
            var notSelectedAuthors = query.Where(a => !a.IsForItem && a.Description.Contains(filtering.SearchString));
            paging.Count = notSelectedAuthors.Count();
            var limitedList = notSelectedAuthors
                .Skip(paging.PageSize * (paging.CurrentPage - 1))
                .Take(paging.PageSize)
                .ToList();

            return new ListAuthorForBookVM()
            {
                Item = book.Map(),
                SelectedValues = selectedAuthors,
                Values = limitedList,
                Paging = paging,
                Filtering = filtering
            };
        }
    }
}