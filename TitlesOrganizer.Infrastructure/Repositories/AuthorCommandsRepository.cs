using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class AuthorCommandsRepository : IAuthorCommandsRepository
    {
        private readonly Context _context;

        public AuthorCommandsRepository(Context context)
        {
            _context = context;
        }

        public void Delete(Author author)
        {
            throw new NotImplementedException();
        }

        public int Insert(Author author)
        {
            throw new NotImplementedException();
        }

        public void Update(Author author)
        {
            throw new NotImplementedException();
        }

        public void UpdateAuthorBooksRelation(Author author)
        {
            throw new NotImplementedException();
        }
    }
}