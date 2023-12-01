using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IAuthorService
    {
        void Delete(int id);

        AuthorVM Get(int id, int booksPageSize, int booksPageNo);

        AuthorDetailsVM GetDetails(int id, int booksPageSize, int booksPageNo, int seriesPageSize, int seriesPageNo, int genresPageSize, int genresPageNo);

        IListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IDoubleListForItemVM GetListForBook(int bookId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IPartialListVM GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        void SelectBooks(int authorId, int[] selectedIds);

        int Upsert(AuthorVM author);
    }
}