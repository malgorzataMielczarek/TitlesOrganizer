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

        public void Delete(Genre genre)
        {
            _context.LiteratureGenres.Remove(genre);
            _context.SaveChanges();
        }

        public int Insert(Genre genre)
        {
            _context.LiteratureGenres.Add(genre);
            _context.SaveChanges();

            return genre.Id;
        }

        public void Update(Genre genre)
        {
            _context.Attach(genre);
            _context.Entry(genre).Property(nameof(Genre.Name)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateLiteratureGenreBooksRelation(Genre genre)
        {
            _context.Attach(genre);
            _context.Entry(genre).Collection(nameof(Genre.Works)).IsModified = true;
            _context.SaveChanges();
        }
    }
}