using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookService
    {
        void Delete(int id);

        BookVM Get(int id);

        BookDetailsVM GetDetails(int id);

        IListVM GetList(SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IDoubleListForItemVM GetListForAuthor(int authorId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IDoubleListForItemVM GetListForGenre(int genreId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IDoubleListForItemVM GetListForSeries(int seriesId, SortByEnum sortBy, int pageSize, int pageNo, string? searchString);

        IPartialListVM GetPartialListForAuthor(int authorId, int pageSize, int pageNo);

        IPartialListVM GetPartialListForGenre(int genreId, int pageSize, int pageNo);

        IPartialListVM GetPartialListForSeries(int seriesId, int pageSize, int pageNo);

        void SelectAuthors(int bookId, int[] selectedIds);

        void SelectGenres(int bookId, int[] selectedIds);

        void SelectSeries(int bookId, int? selectedId);

        int Upsert(BookVM book);
    }
}