using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Helpers
{
    public static class BookModuleHelpers
    {
        public static List<Book> GetBooksList(int count)
        {
            var books = new List<Book>();
            for (int i = 1; i <= count; i++)
            {
                books.Add(GetBook(i));
            }

            return books;
        }

        public static Book GetBook(int index = 1)
        {
            return new Book()
            {
                Id = index,
                Title = "Title" + index,
                OriginalTitle = "Original Title" + index,
                OriginalLanguageCode = "ENG",
                Description = "Description" + index,
                Edition = "I",
                Year = 2001,
                NumberInSeries = index
            };
        }

        public static List<Creator> GetAuthorsList(int count)
        {
            var authors = new List<Creator>();
            for (int i = 1; i <= count; i++)
            {
                authors.Add(GetAuthor(i));
            }

            return authors;
        }

        public static Creator GetAuthor(int index = 1)
        {
            return new Creator()
            {
                Id = index,
                Name = "Name" + index,
                LastName = "Last Name" + index
            };
        }

        public static List<Genre> GetGenresList(int count)
        {
            var genres = new List<Genre>();
            for (int i = 1; i <= count; i++)
            {
                genres.Add(GetGenre(i));
            }

            return genres;
        }

        public static Genre GetGenre(int index = 1)
        {
            return new Genre()
            {
                Id = index,
                Name = "Name" + index
            };
        }

        public static List<Series> GetSeriesList(int count)
        {
            var seriesList = new List<Series>();
            for (int i = 1; i <= count; i++)
            {
                seriesList.Add(GetSeries(i));
            }

            return seriesList;
        }

        public static Series GetSeries(int index = 1)
        {
            return new Series()
            {
                Id = index,
                Title = "Title" + index,
                OriginalTitle = "Original Title" + index,
                Description = "Description" + index
            };
        }
    }
}