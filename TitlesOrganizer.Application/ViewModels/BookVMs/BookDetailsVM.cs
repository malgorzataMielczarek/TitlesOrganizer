using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookDetailsVM : BaseDetailsVM<Book>, IDetailsVM<Book>
    {
        [ScaffoldColumn(false)]
        public string InSeries { get; set; } = string.Empty;

        [ScaffoldColumn(false)]
        public SeriesForListVM? Series { get; set; }

        public List<IForListVM<Author>> Authors { get; set; } = new List<IForListVM<Author>>();

        public string Description { get; set; } = string.Empty;

        [DisplayName("Original title")]
        public string OriginalTitle { get; set; } = string.Empty;

        [DisplayName("Original language")]
        public string OriginalLanguage { get; set; } = string.Empty;

        public string Year { get; set; } = string.Empty;

        public string Edition { get; set; } = string.Empty;

        public List<IForListVM<LiteratureGenre>> Genres { get; set; } = new List<IForListVM<LiteratureGenre>>();
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
                Authors = bookWithAllRelatedObjects.Authors.OrderBy(a => a.LastName).ThenBy(a => a.Name).Map(),
                Genres = bookWithAllRelatedObjects.Genres.OrderBy(g => g.Name).Map(),
                Description = bookWithAllRelatedObjects.Description ?? string.Empty,
                Year = bookWithAllRelatedObjects.Year?.ToString() ?? string.Empty,
                Edition = bookWithAllRelatedObjects.Edition ?? string.Empty,
                Series = bookWithAllRelatedObjects.Series?.Map(),
                InSeries = InSeries(bookWithAllRelatedObjects.NumberInSeries, bookWithAllRelatedObjects.Series)
            };
        }

        public static BookDetailsVM MapToDetails(this Book book, Language? language, IEnumerable<Author> authors, IEnumerable<LiteratureGenre> genres, BookSeries? seriesWithBooks)
        {
            return new BookDetailsVM()
            {
                Id = book.Id,
                Title = book.Title,
                OriginalTitle = book.OriginalTitle ?? string.Empty,
                OriginalLanguage = language?.Name ?? string.Empty,
                Authors = authors.OrderBy(a => a.LastName).ThenBy(a => a.Name).Map().ToList(),
                Genres = genres.OrderBy(g => g.Name).Map().ToList(),
                Description = book.Description ?? string.Empty,
                Year = book.Year?.ToString() ?? string.Empty,
                Edition = book.Edition ?? string.Empty,
                Series = seriesWithBooks?.Map(),
                InSeries = InSeries(book.NumberInSeries, seriesWithBooks)
            };
        }

        public static BookDetailsVM MapToDetails(this Book book, Language? language, IEnumerable<Author> authors, IEnumerable<LiteratureGenre> genres, BookSeries? series, IEnumerable<Book>? booksInSeries)
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