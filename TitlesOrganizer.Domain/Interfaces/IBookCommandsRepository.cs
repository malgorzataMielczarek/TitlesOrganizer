using TitlesOrganizer.Domain.Interfaces.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookCommandsRepository : ICommandsRepository<Book>
    {
        void UpdateBookAuthorsRelation(Book book);

        void UpdateBookGenresRelation(Book book);

        void UpdateBookSeriesRelation(Book book);
    }
}