using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class AuthorDetailsVM : BaseDetailsVM<Author>, IDetailsVM<Author>
    {
        public IPartialList<Book> Books { get; set; } = new PartialList<Book>();
        public IPartialList<BookSeries> Series { get; set; } = new PartialList<BookSeries>();
        public IPartialList<LiteratureGenre> Genres { get; set; } = new PartialList<LiteratureGenre>();
    }

    public static partial class MappingExtensions
    {
        public static AuthorDetailsVM MapToDetails(this Author author, IQueryable<Book> books, Paging booksPaging, IQueryable<BookSeries> series, Paging seriesPaging, IQueryable<LiteratureGenre> genres, Paging genresPaging)
        {
            return new AuthorDetailsVM()
            {
                Id = author.Id,
                Title = author.Name + " " + author.LastName,
                Books = new PartialList<Book>()
                {
                    Values = books.MapToList(ref booksPaging).ToList(),
                    Paging = booksPaging
                },
                Series = new PartialList<BookSeries>()
                {
                    Values = series.MapToList(ref seriesPaging).ToList(),
                    Paging = seriesPaging
                },
                Genres = new PartialList<LiteratureGenre>()
                {
                    Values = genres.MapToList(ref genresPaging).ToList(),
                    Paging = genresPaging
                }
            };
        }
    }
}