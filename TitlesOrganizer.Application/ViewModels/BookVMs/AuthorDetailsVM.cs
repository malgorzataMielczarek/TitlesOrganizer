using System.ComponentModel.DataAnnotations;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string FullName { get; set; } = null!;

        public List<BookForListVM> Books { get; set; } = new List<BookForListVM>();

        [ScaffoldColumn(false)]
        public Paging BooksPaging { get; set; } = new Paging();

        public List<SeriesForListVM> Series { get; set; } = new List<SeriesForListVM>();

        [ScaffoldColumn(false)]
        public Paging SeriesPaging { get; set; } = new Paging();

        public List<GenreForListVM> Genres { get; set; } = new List<GenreForListVM>();

        [ScaffoldColumn(false)]
        public Paging GenresPaging { get; set; } = new Paging();
    }

    public static partial class MappingExtensions
    {
        public static AuthorDetailsVM MapToDetails(this Author author, IQueryable<Book> books, Paging booksPaging, IQueryable<BookSeries> series, Paging seriesPaging, IQueryable<LiteratureGenre> genres, Paging genresPaging)
        {
            return new AuthorDetailsVM()
            {
                Id = author.Id,
                FullName = author.Name + " " + author.LastName,
                Books = books.MapToList(ref booksPaging).ToList(),
                BooksPaging = booksPaging,
                Series = series.MapToList(ref seriesPaging).ToList(),
                SeriesPaging = seriesPaging,
                Genres = genres.MapToList(ref genresPaging).ToList(),
                GenresPaging = genresPaging
            };
        }

        public static AuthorDetailsVM MapToDetails(this Author authorWithAllRelatedObjects, Paging booksPaging, Paging seriesPaging, Paging genresPaging)
        {
            return new AuthorDetailsVM()
            {
                Id = authorWithAllRelatedObjects.Id,
                FullName = authorWithAllRelatedObjects.Name + " " + authorWithAllRelatedObjects.LastName,
                Books = authorWithAllRelatedObjects.Books.AsQueryable().DistinctBy(b => b.Id).MapToList(ref booksPaging).ToList(),
                BooksPaging = booksPaging,
                Series = authorWithAllRelatedObjects.Books.AsQueryable().Where(b => b.Series != null).Select(b => b.Series!).DistinctBy(s => s.Id).MapToList(ref seriesPaging).ToList(),
                SeriesPaging = seriesPaging,
                Genres = authorWithAllRelatedObjects.Books.SelectMany(b => b.Genres).AsQueryable().DistinctBy(g => g.Id).MapToList(ref genresPaging).ToList(),
                GenresPaging = genresPaging,
            };
        }
    }
}