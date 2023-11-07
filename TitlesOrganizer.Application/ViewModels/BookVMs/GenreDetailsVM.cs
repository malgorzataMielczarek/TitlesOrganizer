using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class GenreDetailsVM : BaseDetailsVM<LiteratureGenre>, IDetailsVM<LiteratureGenre>
    {
        public IPartialList<Book> Books { get; set; } = new PartialList<Book>();
        public IPartialList<BookSeries> Series { get; set; } = new PartialList<BookSeries>();
        public IPartialList<Author> Authors { get; set; } = new PartialList<Author>();
    }

    public static partial class MappingExtensions
    {
        public static GenreDetailsVM MapToDetails(this LiteratureGenre genre, IQueryable<Book> books, Paging booksPaging, IQueryable<BookSeries> series, Paging seriesPaging, IQueryable<Author> authors, Paging authorsPaging)
        {
            return new GenreDetailsVM()
            {
                Id = genre.Id,
                Title = genre.Name,
                Books = books.MapToPartialList(booksPaging),
                Series = series.MapToPartialList(seriesPaging),
                Authors = authors.MapToPartialList(authorsPaging)
            };
        }

        public static GenreDetailsVM MapToDetails(this LiteratureGenre genre)
        {
            return new GenreDetailsVM()
            {
                Id = genre.Id,
                Title = genre.Name
            };
        }
    }
}