using TitlesOrganizer.Domain.Interfaces.Base;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Domain.Interfaces
{
    public interface ICreatorCommandsRepository : ICommandsRepository<Creator>
    {
        void UpdateCreatorCollectionRelation(Creator creator, string propName);
    }
}