using TitlesOrganizer.Domain.Interfaces.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface IAuthorCommandsRepository : ICommandsRepository<Author>
    {
        void UpdateAuthorBooksRelation(Author author);
    }
}