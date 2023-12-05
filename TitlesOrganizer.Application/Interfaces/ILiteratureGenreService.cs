using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface ILiteratureGenreService
    {
        void Delete(int id);

        GenreVM Get(int id, int booksPageSize, int booksPageNo);

        GenreDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int authorsPageSize, int authorsPageNo, int seriesPageSize, int seriesPageNo);

        IListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IDoubleListForItemVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IPartialListVM GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        void SelectBooks(int genreId, int[] selectedIds);

        int Upsert(GenreVM genre);
    }
}