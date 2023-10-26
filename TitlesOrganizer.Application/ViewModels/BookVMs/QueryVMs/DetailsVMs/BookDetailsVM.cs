using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.DetailsVMs
{
    public class BookDetailsVM
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public string Title { get; set; } = null!;

        [ScaffoldColumn(false)]
        public string InSeries { get; set; } = null!;

        [ScaffoldColumn(false)]
        public SeriesForListVM? Series { get; set; }

        public List<AuthorForListVM> Authors { get; set; } = new List<AuthorForListVM>();

        public string Description { get; set; } = null!;

        [DisplayName("Original title")]
        public string OriginalTitle { get; set; } = null!;

        [DisplayName("Original language")]
        public string OriginalLanguage { get; set; } = null!;

        public string Year { get; set; } = null!;

        public string Edition { get; set; } = null!;

        public List<GenreForListVM> Genres { get; set; } = new List<GenreForListVM>();
    }

    public static partial class MappingExtensions
    {
        public static BookDetailsVM MapToDetails(this Book bookWithAllRelatedObjects)
        {
            return new BookDetailsVM()
            {
                Id = bookWithAllRelatedObjects.Id,
                Title = bookWithAllRelatedObjects.Title,
                OriginalTitle = bookWithAllRelatedObjects.OriginalTitle ?? string.Empty,
                OriginalLanguage = bookWithAllRelatedObjects.OriginalLanguage?.Name ?? string.Empty,
                Authors = bookWithAllRelatedObjects.Authors.Map(),
                Genres = bookWithAllRelatedObjects.Genres.Map(),
                Description = bookWithAllRelatedObjects.Description ?? string.Empty,
                Year = bookWithAllRelatedObjects.Year?.ToString() ?? string.Empty,
                Edition = bookWithAllRelatedObjects.Edition ?? string.Empty,
                Series = bookWithAllRelatedObjects.Series?.Map(),
                InSeries = InSeries(bookWithAllRelatedObjects.NumberInSeries, bookWithAllRelatedObjects.Series)
            };
        }

        public static BookDetailsVM MapToDetails(this Book book, Language? language, IQueryable<Author> authors, IQueryable<LiteratureGenre> genres, BookSeries? seriesWithBooks)
        {
            return new BookDetailsVM()
            {
                Id = book.Id,
                Title = book.Title,
                OriginalTitle = book.OriginalTitle ?? string.Empty,
                OriginalLanguage = language?.Name ?? string.Empty,
                Authors = authors.Map().ToList(),
                Genres = genres.Map().ToList(),
                Description = book.Description ?? string.Empty,
                Year = book.Year?.ToString() ?? string.Empty,
                Edition = book.Edition ?? string.Empty,
                Series = seriesWithBooks?.Map(),
                InSeries = InSeries(book.NumberInSeries, seriesWithBooks)
            };
        }

        public static BookDetailsVM MapToDetails(this Book book, Language? language, IQueryable<Author> authors, IQueryable<LiteratureGenre> genres, BookSeries? series, IQueryable<Book>? booksInSeries)
        {
            if (series != null && booksInSeries != null)
            {
                series.Books = booksInSeries.ToList();
            }

            return book.MapToDetails(language, authors, genres, series);
        }

        private static string InSeries(int? numberInSeries, BookSeries? bookSeries)
        {
            if (bookSeries == null)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();
            if (numberInSeries != null)
            {
                result.Append(numberInSeries);
                int count;
                if ((count = bookSeries.Books.Count) > 0)
                {
                    result.Append(" of ");
                    result.Append(count);
                }

                if (bookSeries.Title != null)
                {
                    result.Append(" in ");
                }
            }
            else if (bookSeries.Title != null)
            {
                result.Append("Part of ");
            }

            return result.ToString();
        }
    }
}