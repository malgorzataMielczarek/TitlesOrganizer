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
            searchString ??= string.Empty;

            return (entities) switch
            {
                IQueryable<Creator> authors => (IQueryable<T>)authors.Where(a => (a.Name + " " + a.LastName).Contains(searchString)),
                IQueryable<Book> books => (IQueryable<T>)books.Where(b => b.Titles.Any(wt => wt.Title.Contains(searchString))),
                IQueryable<Series> series => (IQueryable<T>)series.Where(s => s.Works.Any()).Where(s => s.Titles.Any(wt => wt.Title.Contains(searchString))),
                IQueryable<Genre> genres => (IQueryable<T>)genres.Where(g => g.Name.Contains(searchString)),
                _ => entities
            };
        }

        public override IForListVM Map<T>(T entity)
        {
            var result = base.Map(entity);
            switch (entity)
            {
                case Creator author:
                    result.Description = $"{author!.Name} {author.LastName}"; break;
                case WorkModel work:
                    result.Description = work.Titles.OrderBy(wt => wt.Level).ThenBy(wt => wt.Id).FirstOrDefault()?.Title ?? string.Empty; break;
                case Genre genre:
                    result.Description = genre.Name; break;
            }

            return result;
        }

        public override IForItemVM Map<T, ItemT>(T entity, ItemT item)
        {
            return (entity) switch
            {
                Creator author => new ForItemVM()
                {
                    Id = author.Id,
                    Description = author.Name + " " + author.LastName,
                    IsForItem = (item) switch
                    {
                        Book => IsForItem(author.Works, item.Id),
                        _ => false
                    }
                },
                Book book => new ForItemVM()
                {
                    Id = book.Id,
                    Description = book.Title,
                    IsForItem = (item) switch
                    {
                        Creator => IsForItem(book.Creators, item.Id),
                        Series => book.SeriesId == item.Id,
                        Genre => IsForItem(book.Genres, item.Id),
                        _ => false
                    }
                },
                Series bookSeries => new ForItemVM()
                {
                    Id = bookSeries.Id,
                    Description = bookSeries.Title,
                    IsForItem = (item) switch
                    {
                        Book => IsForItem(bookSeries.Works, item.Id),
                        _ => false
                    }
                },
                Genre genre => new ForItemVM()
                {
                    Id = genre.Id,
                    Description = genre.Name,
                    IsForItem = (item) switch
                    {
                        Book => IsForItem(genre.Works, item.Id),
                        _ => false
                    }
                },
                _ => new ForItemVM() { Id = entity.Id }
            };
        }

        public override TDestination Map<TSource, TDestination>(TSource entity)
        {
            var isMappingDefined =
                (entity is Creator && (typeof(TDestination) == typeof(AuthorVM) || typeof(TDestination) == typeof(AuthorDetailsVM))) ||
                (entity is AuthorVM && typeof(TDestination) == typeof(Creator)) ||
                (entity is Book && (typeof(TDestination) == typeof(BookVM) || typeof(TDestination) == typeof(BookDetailsVM))) ||
                (entity is BookVM && typeof(TDestination) == typeof(Book)) ||
                (entity is Series && (typeof(TDestination) == typeof(SeriesVM) || typeof(TDestination) == typeof(SeriesDetailsVM))) ||
                (entity is SeriesVM && typeof(TDestination) == typeof(Series)) ||
                (entity is Genre && (typeof(TDestination) == typeof(GenreVM) || typeof(TDestination) == typeof(GenreDetailsVM))) ||
                (entity is GenreVM && typeof(TDestination) == typeof(Genre));
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
                IQueryable<Creator> authors => (IOrderedQueryable<T>)authors.Sort(sortBy, a => a.LastName, a => a.Name),
                IQueryable<Book> books => (IOrderedQueryable<T>)books.Sort(sortBy, b => b.Title),
                IQueryable<Series> series => (IOrderedQueryable<T>)series.Sort(sortBy, s => s.Title),
                IQueryable<Genre> genre => (IOrderedQueryable<T>)genre.Sort(sortBy, g => g.Name),
                _ => entities.Order()
            };
        }

        private bool IsForItem<T>(ICollection<T>? collection, int id)
            where T : BaseModel
        {
            if (collection == null)
            {
                return false;
            }

            return collection.Any(it => it.Id == id);
        }
    }
}