using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Domain.Interfaces.Base
{
    public interface ICommandsRepository<T> where T : class, IBaseModel
    {
        void Delete(T item);

        int Insert(T item);

        void Update(T item);
    }
}