using TitlesOrganizer.Application.ViewModels.BookVMs;

namespace TitlesOrganizer.Application.Interfaces
{
    public interface IBookService
    {
        int AddBook(NewBookVM book);

        ListAuthorForBook GetAllAuthorsForList();

        ListBookForListVM GetAllBooksForList();

        BookDetailsVM GetBookDetails(int id);
    }
}