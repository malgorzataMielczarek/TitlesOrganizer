using TitlesOrganizer.Domain.Interfaces.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface ILiteratureGenreCommandsRepository : ICommandsRepository<LiteratureGenre>
    {
        void UpdateLiteratureGenreBooksRelation(LiteratureGenre genre);
    }
}