using AutoMapper;
using TitlesOrganizer.Application.Mappings.Abstract;
using TitlesOrganizer.Application.Mappings.Common;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Common;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.Mappings.Concrete
{
    public class BookVMsMappings(IMapper _mapper) : BaseMappings(_mapper), IBookVMsMappings
    {
        public override IQueryable<T> Filter<T>(IQueryable<T> entities, string searchString)
        {
            return (entities) switch
            {
                IQueryable<Author> authors => (IQueryable<T>)authors.Where(a => (a.Name + " " + a.LastName).Contains(searchString)),
                IQueryable<Book> books => (IQueryable<T>)books.Where(b => b.Title.Contains(searchString)),
                IQueryable<BookSeries> series => (IQueryable<T>)series.Where(s => s.Title.Contains(searchString)),
                IQueryable<LiteratureGenre> genres => (IQueryable<T>)genres.Where(g => g.Name.Contains(searchString)),
                _ => entities
            };
        }

        public override IForListVM Map<T>(T entity)
        {
            var result = base.Map(entity);
            switch (entity)
            {
                case Author author:
                    result.Description = $"{author!.Name} {author.LastName}"; break;
                case Book book:
                    result.Description = book.Title; break;
                case BookSeries bookSeries:
                    result.Description = bookSeries.Title; break;
                case LiteratureGenre genre:
                    result.Description = genre.Name; break;
            }

            return result;
        }

        public override IForItemVM Map<T, ItemT>(T entity, ItemT item)
        {
            return (entity) switch
            {
                Author author => new ForItemVM()
                {
                    Id = author.Id,
                    Description = author.Name + " " + author.LastName,
                    IsForItem = (item) switch
                    {
                        Book => IsForItem(author.Books, item.Id),
                        _ => false
                    }
                },
                Book book => new ForItemVM()
                {
                    Id = book.Id,
                    Description = book.Title,
                    IsForItem = (item) switch
                    {
                        Author => IsForItem(book.Authors, item.Id),
                        BookSeries => book.SeriesId == item.Id,
                        LiteratureGenre => IsForItem(book.Genres, item.Id),
                        _ => false
                    }
                },
                BookSeries bookSeries => new ForItemVM()
                {
                    Id = bookSeries.Id,
                    Description = bookSeries.Title,
                    IsForItem = (item) switch
                    {
                        Book => IsForItem(bookSeries.Books, item.Id),
                        _ => false
                    }
                },
                LiteratureGenre genre => new ForItemVM()
                {
                    Id = genre.Id,
                    Description = genre.Name,
                    IsForItem = (item) switch
                    {
                        Book => IsForItem(genre.Books, item.Id),
                        _ => false
                    }
                },
                _ => new ForItemVM() { Id = entity.Id }
            };
        }

        public override TDestination Map<TSource, TDestination>(TSource entity)
        {
            var isMappingDefined =
                (entity is Author && (typeof(TDestination) == typeof(AuthorVM) || typeof(TDestination) == typeof(AuthorDetailsVM))) ||
                (entity is AuthorVM && typeof(TDestination) == typeof(Author)) ||
                (entity is Book && (typeof(TDestination) == typeof(BookVM) || typeof(TDestination) == typeof(BookDetailsVM))) ||
                (entity is BookVM && typeof(TDestination) == typeof(Book)) ||
                (entity is BookSeries && (typeof(TDestination) == typeof(SeriesVM) || typeof(TDestination) == typeof(SeriesDetailsVM))) ||
                (entity is SeriesVM && typeof(TDestination) == typeof(BookSeries)) ||
                (entity is LiteratureGenre && (typeof(TDestination) == typeof(GenreVM) || typeof(TDestination) == typeof(GenreDetailsVM))) ||
                (entity is GenreVM && typeof(TDestination) == typeof(LiteratureGenre));
            if (isMappingDefined)
            {
                return base.Map<TSource, TDestination>(entity);
            }
            else
            {
                throw new NotImplementedException("Mapping between those types is not defined.");
            }
        }

        public override IOrderedQueryable<T> Sort<T>(IQueryable<T> entities, SortByEnum sortBy)
        {
            return entities switch
            {
                IQueryable<Author> authors => (IOrderedQueryable<T>)authors.Sort(sortBy, a => a.LastName, a => a.Name),
                IQueryable<Book> books => (IOrderedQueryable<T>)books.Sort(sortBy, b => b.Title),
                IQueryable<BookSeries> series => (IOrderedQueryable<T>)series.Sort(sortBy, s => s.Title),
                IQueryable<LiteratureGenre> genre => (IOrderedQueryable<T>)genre.Sort(sortBy, g => g.Name),
                _ => entities.Order()
            };
        }

        private bool IsForItem<T>(ICollection<T>? collection, int id)
            where T : class, IBaseModel
        {
            if (collection == null)
            {
                return false;
            }

            return collection.Any(it => it.Id == id);
        }
    }
}