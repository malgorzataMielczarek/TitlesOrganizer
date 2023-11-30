using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Application.ViewModels.BookVMs
{
    public class BookVMsMappings : BaseMappings
    {
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
            var result = base.Map(entity, item);
            switch (entity)
            {
                case Author author:
                    result.Description = $"{author!.Name} {author.LastName}";
                    if (item is Book)
                    {
                        result.IsForItem = IsForItem(author.Books, item.Id);
                    }

                    break;

                case Book book:
                    result.Description = book.Title;
                    result.IsForItem = (item) switch
                    {
                        Author => IsForItem(book.Authors, item.Id),
                        BookSeries => book.SeriesId == item.Id,
                        LiteratureGenre => IsForItem(book.Genres, item.Id),
                        _ => false
                    };
                    break;

                case BookSeries bookSeries:
                    result.Description = bookSeries.Title;
                    if (item is Book)
                    {
                        result.IsForItem = IsForItem(bookSeries.Books, item.Id);
                    }

                    break;

                case LiteratureGenre genre:
                    result.Description = genre.Name;
                    if (item is Book)
                    {
                        result.IsForItem = IsForItem(genre.Books, item.Id);
                    }

                    break;
            }

            return result;
        }

        public override IOrderedQueryable<T> Sort<T>(IQueryable<T> entities, SortByEnum sortBy)
        {
            return (entities) switch
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