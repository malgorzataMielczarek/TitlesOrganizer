using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueriesVMs.DetailsVMs
{
    public class AuthorDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string FullName { get; set; } = null!;

        public ListBookForListVM Books { get; set; } = new ListBookForListVM();

        public ListSeriesForListVM Series { get; set; } = new ListSeriesForListVM();

        public ListGenreForListVM Genres { get; set; } = new ListGenreForListVM();
    }

    public static partial class MappingExtensions
    {
        public static AuthorDetailsVM MapToDetails(this Author author, ListData<Book> books, ListData<BookSeries> series, ListData<LiteratureGenre> genres)
        {
            return new AuthorDetailsVM()
            {
                Id = author.Id,
                FullName = author.Name + " " + author.LastName,
                Books = books.MapToList(),
                Series = series.MapToList(),
                Genres = genres.MapToList()
            };
        }

        public static AuthorDetailsVM MapToDetails(this Author authorWithAllRelatedObjects, Paging booksPaging, Filtering booksFiltering, Paging seriesPaging, Filtering seriesFiltering, Paging genresPaging, Filtering genresFiltering)
        {
            return new AuthorDetailsVM()
            {
                Id = authorWithAllRelatedObjects.Id,
                FullName = authorWithAllRelatedObjects.Name + " " + authorWithAllRelatedObjects.LastName,
                Books = authorWithAllRelatedObjects.Books.AsQueryable().MapToList(booksPaging, booksFiltering),
                Series = authorWithAllRelatedObjects.Books.AsQueryable().Where(b => b.BookSeries != null).Select(b => b.BookSeries!).MapToList(seriesPaging, seriesFiltering),
                Genres = authorWithAllRelatedObjects.Books.SelectMany(b => b.Genres).AsQueryable().MapToList(genresPaging, genresFiltering)
            };
        }
    }
}