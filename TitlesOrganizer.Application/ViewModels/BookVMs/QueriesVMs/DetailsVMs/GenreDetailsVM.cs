using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.DetailsVMs
{
    public class GenreDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string Name { get; set; } = null!;

        public ListBookForListVM Books { get; set; } = new ListBookForListVM();

        public ListSeriesForListVM Series { get; set; } = new ListSeriesForListVM();

        public ListAuthorForListVM Author { get; set; } = new ListAuthorForListVM();
    }

    public static partial class MappingExtensions
    {
        public static GenreDetailsVM MapToDetails(this LiteratureGenre genre, ListData<Book> books, ListData<BookSeries> series, ListData<Author> authors)
        {
            return new GenreDetailsVM()
            {
                Id = genre.Id,
                Name = genre.Name,
                Books = books.MapToList(),
                Series = series.MapToList(),
                Author = authors.MapToList()
            };
        }

        public static GenreDetailsVM MapToDetails(this LiteratureGenre genreWithAllRelatedObjects, Paging booksPaging, Filtering booksFiltering, Paging seriesPaging, Filtering seriesFiltering, Paging authorsPaging, Filtering authorsFiltering)
        {
            var genre = new GenreDetailsVM()
            {
                Id = genreWithAllRelatedObjects.Id,
                Name = genreWithAllRelatedObjects.Name
            };

            var books = genreWithAllRelatedObjects.Books?.AsQueryable();
            if (books != null)
            {
                genre.Books = books.MapToList(booksPaging, booksFiltering);
                genre.Series = books.Where(b => b.BookSeries != null).Select(b => b.BookSeries!).MapToList(seriesPaging, seriesFiltering);
                genre.Author = books.SelectMany(b => b.Authors).MapToList(authorsPaging, authorsFiltering);
            }

            return genre;
        }
    }
}