using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface ILiteratureGenreService
    {
        void Delete(int id);

        GenreVM Get(int id, int booksPageSize, int booksPageNo);

        GenreDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo);

        ListGenreForListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        ListGenreForBookVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        PartialListVM<LiteratureGenre> GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        void SelectBooks(int genreId, int[] selectedIds);

        int Upsert(GenreVM genre);
    }
}