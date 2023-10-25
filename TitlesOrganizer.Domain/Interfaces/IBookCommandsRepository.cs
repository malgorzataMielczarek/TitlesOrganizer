using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IBookCommandsRepository
    {
        void Delete(Author author);

        Author? GetAuthor(int id);
    }
}