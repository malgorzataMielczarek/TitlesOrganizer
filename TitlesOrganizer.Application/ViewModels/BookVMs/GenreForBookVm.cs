using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class GenreForBookVM : BaseForItemVM<LiteratureGenre, Book>, IForItemVM<LiteratureGenre, Book>
    {
        [DisplayName("Genre")]
        public override string Description { get; set; } = null!;
    }

    public class ListGenreForBookVM : IDoubleListForItemVM<LiteratureGenre, Book>
    {
        [DisplayName("Book")]
        public IForListVM<Book> Item { get; set; } = new BookForListVM();

        [DisplayName("Previously selected genres")]
        public List<IForItemVM<LiteratureGenre, Book>> SelectedValues { get; set; } = new List<IForItemVM<LiteratureGenre, Book>>();

        [DisplayName("Other genres")]
        public List<IForItemVM<LiteratureGenre, Book>> Values { get; set; } = new List<IForItemVM<LiteratureGenre, Book>>();

        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IForItemVM<LiteratureGenre, Book> MapForItem(this LiteratureGenre genreWithBooks, Book book)
        {
            return new GenreForBookVM
            {
                Id = genreWithBooks.Id,
                Description = genreWithBooks.Name,
                IsForItem = genreWithBooks.Books != null && genreWithBooks.Books.Any(b => b.Id == book.Id)
            };
        }

        public static IQueryable<IForItemVM<LiteratureGenre, Book>> MapForItem(this IQueryable<LiteratureGenre> sortedList, Book item)
        {
            return sortedList.Select(it => it.MapForItem(item));
        }

        public static ListGenreForBookVM MapForItemToList(this IQueryable<LiteratureGenre> genresWithBooks, Book book, Paging paging, Filtering filtering)
        {
            return genresWithBooks
                .Sort(filtering.SortBy, g => g.Name)
                .MapForItem(book)
                .MapForItemToDoubleList<LiteratureGenre, Book, ListGenreForBookVM>(book.Map(), paging, filtering);
        }
    }
}