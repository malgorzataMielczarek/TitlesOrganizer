using System.ComponentModel;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookForGenreVM : BaseForItemVM<Book, LiteratureGenre>, IForItemVM<Book, LiteratureGenre>
    {
        [DisplayName("Book")]
        public override string Description { get; set; } = null!;
    }

    public class ListBookForGenreVM : IDoubleListForItemVM<Book, LiteratureGenre>
    {
        [DisplayName("Genre")]
        public IForListVM<LiteratureGenre> Item { get; set; } = new GenreForListVM();

        [DisplayName("Previously selected books")]
        public List<IForItemVM<Book, LiteratureGenre>> SelectedValues { get; set; } = new List<IForItemVM<Book, LiteratureGenre>>();

        [DisplayName("Other books")]
        public List<IForItemVM<Book, LiteratureGenre>> Values { get; set; } = new List<IForItemVM<Book, LiteratureGenre>>();

        public Paging Paging { get; set; } = new Paging();
        public Filtering Filtering { get; set; } = new Filtering();
    }

    public static partial class MappingExtensions
    {
        public static IForItemVM<Book, LiteratureGenre> MapForItem(this Book bookWithGenres, LiteratureGenre genre)
        {
            return new BookForGenreVM
            {
                Id = bookWithGenres.Id,
                Description = bookWithGenres.Title,
                IsForItem = bookWithGenres.Genres.Any(g => g.Id == genre.Id)
            };
        }

        public static IQueryable<IForItemVM<Book, LiteratureGenre>> MapForItem(this IQueryable<Book> sortedList, LiteratureGenre item)
        {
            return sortedList.Select(it => it.MapForItem(item));
        }

        public static ListBookForGenreVM MapForItemToList(this IQueryable<Book> booksWithGenres, LiteratureGenre genre, Paging paging, Filtering filtering)
        {
            return booksWithGenres
                .Sort(filtering.SortBy, b => b.Title)
                .MapForItem(genre)
                .MapForItemToDoubleList<Book, LiteratureGenre, ListBookForGenreVM>(genre.Map(), paging, filtering);
        }
    }
}