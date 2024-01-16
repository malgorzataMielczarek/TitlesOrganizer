using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class CreatorCommandsRepository : ICreatorCommandsRepository
    {
        private readonly Context _context;

        public CreatorCommandsRepository(Context context)
        {
            _context = context;
        }

        public void Delete(Creator creator)
        {
            _context.Creators.Remove(creator);
            _context.SaveChanges();
        }

        public int Insert(Creator creator)
        {
            _context.Creators.Add(creator);
            _context.SaveChanges(true);

            return creator.Id;
        }

        public void Update(Creator creator)
        {
            _context.Attach(creator);
            _context.Entry(creator).Property(nameof(Creator.Name)).IsModified = true;
            _context.Entry(creator).Property(nameof(Creator.LastName)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateCreatorCollectionRelation(Creator creator, string propName)
        {
            _context.Attach(creator);
            _context.Entry(creator).Collection(propName).IsModified = true;
            _context.SaveChanges();
        }
    }
}