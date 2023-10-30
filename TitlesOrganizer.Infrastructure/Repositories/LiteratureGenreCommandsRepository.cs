using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class LiteratureGenreCommandsRepository : ILiteratureGenreCommandsRepository
    {
        private readonly Context _context;

        public LiteratureGenreCommandsRepository(Context context)
        {
            _context = context;
        }

        public void Delete(LiteratureGenre genre)
        {
            throw new NotImplementedException();
        }

        public int Insert(LiteratureGenre genre)
        {
            throw new NotImplementedException();
        }

        public void Update(LiteratureGenre genre)
        {
            throw new NotImplementedException();
        }

        public void UpdateLiteratureGenreBooksRelation(LiteratureGenre genre)
        {
            throw new NotImplementedException();
        }
    }
}