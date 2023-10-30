using TitlesOrganizer.Domain.Interfaces;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Infrastructure.Repositories
{
    public class BookCommandsRepository : IBookCommandsRepository
    {
        private readonly Context _context;

        public BookCommandsRepository(Context context)
        {
            _context = context;
        }

        public void Delete(Book book)
        {
            throw new NotImplementedException();
        }

        public int Insert(Book book)
        {
            throw new NotImplementedException();
        }

        public void Update(Book book)
        {
            throw new NotImplementedException();
        }

        public void UpdateBookAuthorsRelation(Book book)
        {
            throw new NotImplementedException();
        }

        public void UpdateBookGenresRelation(Book book)
        {
            throw new NotImplementedException();
        }

        public void UpdateBookSeriesRelation(Book book)
        {
            throw new NotImplementedException();
        }
    }
}