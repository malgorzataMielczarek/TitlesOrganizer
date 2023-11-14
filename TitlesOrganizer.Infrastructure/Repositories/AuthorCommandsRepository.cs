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
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }

        public int Insert(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges(true);

            return author.Id;
        }

        public void Update(Author author)
        {
            _context.Attach(author);
            _context.Entry(author).Property(nameof(Author.Name)).IsModified = true;
            _context.Entry(author).Property(nameof(Author.LastName)).IsModified = true;
            _context.SaveChanges();
        }

        public void UpdateAuthorBooksRelation(Author author)
        {
            _context.Attach(author);
            _context.Entry(author).Collection(nameof(Author.Books)).IsModified = true;
            _context.SaveChanges();
        }
    }
}