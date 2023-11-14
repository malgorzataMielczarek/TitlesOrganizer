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
            _context.LiteratureGenres.Remove(genre);
            _context.SaveChanges();
        }

        public int Insert(LiteratureGenre genre)
        {
            _context.LiteratureGenres.Add(genre);
            _context.SaveChanges();

            return genre.Id;
        }

        public void Update(LiteratureGenre genre)
        {
            _context.Attach(genre);
            _context.Entry(genre).Property(nameof(LiteratureGenre.Name)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateLiteratureGenreBooksRelation(LiteratureGenre genre)
        {
            _context.Attach(genre);
            _context.Entry(genre).Collection(nameof(LiteratureGenre.Books)).IsModified = true;
            _context.SaveChanges();
        }
    }
}