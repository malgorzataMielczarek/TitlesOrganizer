// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ReferencesVMs.ForBookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Book.ViewModels.QueryVMs.ReferencesVMs.ForBookVMs
{
    public class MappingExtensionsTests
    {
        [Fact]
        public void MapForBook_IQueryableAuthorWithBooksAndBookId_IQueryableAuthorForBookVM()
        {
            var authors = GetAuthorsWithBooks();
            int bookId = 1;

            var result = authors.MapForBook(bookId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<AuthorForBookVM>>().And.AllBeOfType<AuthorForBookVM>().And.HaveCount(authors.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).FullName.Should().Be("Amanda Popiołek");
            result.ElementAt(1).FullName.Should().Be("Amanda Adamska");
            result.ElementAt(2).FullName.Should().Be("Michał Popiołek");
            result.ElementAt(3).FullName.Should().Be("Piotr Krasowski");
            result.ElementAt(0).IsForBook.Should().BeTrue();
            result.ElementAt(1).IsForBook.Should().BeFalse();
            result.ElementAt(2).IsForBook.Should().BeTrue();
            result.ElementAt(3).IsForBook.Should().BeTrue();
        }

        [Theory]
        [InlineData(3, 0, 4, 3, 1, 3)]
        [InlineData(3, 0, 4, 3, 2, 1)]
        [InlineData(1, 3, 1, 3, 1, 1)]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithCorrectAmountOfElementsAndCorrectPaging(int bookId, int selectedCount, int notSelectedCount, int pageSize, int pageNo, int pageCount)
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(bookId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = authors.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForBookVM>();
            result.Book.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.SelectedAuthors.Should().NotBeNull().And.BeOfType<List<AuthorForBookVM>>().And.HaveCount(selectedCount);
            result.NotSelectedAuthors.Should().NotBeNull().And.BeOfType<List<AuthorForBookVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(notSelectedCount);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithOrderedSelectedAuthors()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering();

            var result = authors.MapForBookToList(book, paging, filtering);

            result.SelectedAuthors.ElementAt(0).Id.Should().Be(4);
            result.SelectedAuthors.ElementAt(1).Id.Should().Be(1);
            result.SelectedAuthors.ElementAt(2).Id.Should().Be(3);
            result.SelectedAuthors.ElementAt(0).FullName.Should().Be("Piotr Krasowski");
            result.SelectedAuthors.ElementAt(1).FullName.Should().Be("Amanda Popiołek");
            result.SelectedAuthors.ElementAt(2).FullName.Should().Be("Michał Popiołek");
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithOrderedNotSelectedAuthors()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering();

            var result = authors.MapForBookToList(book, paging, filtering);

            result.NotSelectedAuthors.ElementAt(0).Id.Should().Be(2);
            result.NotSelectedAuthors.ElementAt(1).Id.Should().Be(4);
            result.NotSelectedAuthors.ElementAt(2).Id.Should().Be(1);
            result.NotSelectedAuthors.ElementAt(3).Id.Should().Be(3);
            result.NotSelectedAuthors.ElementAt(0).FullName.Should().Be("Amanda Adamska");
            result.NotSelectedAuthors.ElementAt(1).FullName.Should().Be("Piotr Krasowski");
            result.NotSelectedAuthors.ElementAt(2).FullName.Should().Be("Amanda Popiołek");
            result.NotSelectedAuthors.ElementAt(3).FullName.Should().Be("Michał Popiołek");
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithFilteredNotSelectedAuthors()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SearchString = "Popiołek" };

            var result = authors.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForBookVM>();
            result.NotSelectedAuthors.Should().NotBeNull().And.HaveCount(2);
            result.NotSelectedAuthors.ElementAt(0).Id.Should().Be(1);
            result.NotSelectedAuthors.ElementAt(1).Id.Should().Be(3);
            result.NotSelectedAuthors.ElementAt(0).FullName.Should().Be("Amanda Popiołek");
            result.NotSelectedAuthors.ElementAt(1).FullName.Should().Be("Michał Popiołek");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableAuthorWithBooksAndBookAndPagingAndFiltering_ListAuthorForBookVMWithNotSelectedAuthorsOrderedDescending()
        {
            var authors = GetAuthorsWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = authors.MapForBookToList(book, paging, filtering);

            result.NotSelectedAuthors.ElementAt(0).Id.Should().Be(3);
            result.NotSelectedAuthors.ElementAt(1).Id.Should().Be(1);
            result.NotSelectedAuthors.ElementAt(2).Id.Should().Be(4);
            result.NotSelectedAuthors.ElementAt(3).Id.Should().Be(2);
            result.NotSelectedAuthors.ElementAt(0).FullName.Should().Be("Michał Popiołek");
            result.NotSelectedAuthors.ElementAt(1).FullName.Should().Be("Amanda Popiołek");
            result.NotSelectedAuthors.ElementAt(2).FullName.Should().Be("Piotr Krasowski");
            result.NotSelectedAuthors.ElementAt(3).FullName.Should().Be("Amanda Adamska");
        }

        [Fact]
        public void MapForBook_IQueryableLiteratureGenreWithBooksAndBookId_IQueryableGenreForBookVM()
        {
            var genres = GetGenresWithBooks();
            int bookId = 1;

            var result = genres.MapForBook(bookId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<GenreForBookVM>>().And.AllBeOfType<GenreForBookVM>().And.HaveCount(genres.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).Name.Should().Be("Crime Comedy");
            result.ElementAt(1).Name.Should().Be("Romantic Comedy");
            result.ElementAt(2).Name.Should().Be("Comedy");
            result.ElementAt(3).Name.Should().Be("Crime Story");
            result.ElementAt(0).IsForBook.Should().BeTrue();
            result.ElementAt(1).IsForBook.Should().BeFalse();
            result.ElementAt(2).IsForBook.Should().BeTrue();
            result.ElementAt(3).IsForBook.Should().BeTrue();
        }

        [Theory]
        [InlineData(3, 0, 4, 3, 1, 3)]
        [InlineData(3, 0, 4, 3, 2, 1)]
        [InlineData(1, 3, 1, 3, 1, 1)]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithCorrectAmountOfElementsAndCorrectPaging(int bookId, int selectedCount, int notSelectedCount, int pageSize, int pageNo, int pageCount)
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(bookId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = genres.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForBookVM>();
            result.Book.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.SelectedGenres.Should().NotBeNull().And.BeOfType<List<GenreForBookVM>>().And.HaveCount(selectedCount);
            result.NotSelectedGenres.Should().NotBeNull().And.BeOfType<List<GenreForBookVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(notSelectedCount);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithOrderedSelectedGenres()
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering();

            var result = genres.MapForBookToList(book, paging, filtering);

            result.SelectedGenres.ElementAt(0).Id.Should().Be(3);
            result.SelectedGenres.ElementAt(1).Id.Should().Be(1);
            result.SelectedGenres.ElementAt(2).Id.Should().Be(4);
            result.SelectedGenres.ElementAt(0).Name.Should().Be("Comedy");
            result.SelectedGenres.ElementAt(1).Name.Should().Be("Crime Comedy");
            result.SelectedGenres.ElementAt(2).Name.Should().Be("Crime Story");
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithOrderedNotSelectedGenres()
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering();

            var result = genres.MapForBookToList(book, paging, filtering);

            result.NotSelectedGenres.ElementAt(0).Id.Should().Be(3);
            result.NotSelectedGenres.ElementAt(1).Id.Should().Be(1);
            result.NotSelectedGenres.ElementAt(2).Id.Should().Be(4);
            result.NotSelectedGenres.ElementAt(3).Id.Should().Be(2);
            result.NotSelectedGenres.ElementAt(0).Name.Should().Be("Comedy");
            result.NotSelectedGenres.ElementAt(1).Name.Should().Be("Crime Comedy");
            result.NotSelectedGenres.ElementAt(2).Name.Should().Be("Crime Story");
            result.NotSelectedGenres.ElementAt(3).Name.Should().Be("Romantic Comedy");
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithFilteredNotSelectedGenres()
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SearchString = "Crime" };

            var result = genres.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForBookVM>();
            result.NotSelectedGenres.Should().NotBeNull().And.HaveCount(2);
            result.NotSelectedGenres.ElementAt(0).Id.Should().Be(1);
            result.NotSelectedGenres.ElementAt(1).Id.Should().Be(4);
            result.NotSelectedGenres.ElementAt(0).Name.Should().Be("Crime Comedy");
            result.NotSelectedGenres.ElementAt(1).Name.Should().Be("Crime Story");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreWithBooksAndBookAndPagingAndFiltering_ListGenreForBookVMWithNotSelectedGenresOrderedDescending()
        {
            var genres = GetGenresWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = genres.MapForBookToList(book, paging, filtering);

            result.NotSelectedGenres.ElementAt(0).Id.Should().Be(2);
            result.NotSelectedGenres.ElementAt(1).Id.Should().Be(4);
            result.NotSelectedGenres.ElementAt(2).Id.Should().Be(1);
            result.NotSelectedGenres.ElementAt(3).Id.Should().Be(3);
            result.NotSelectedGenres.ElementAt(0).Name.Should().Be("Romantic Comedy");
            result.NotSelectedGenres.ElementAt(1).Name.Should().Be("Crime Story");
            result.NotSelectedGenres.ElementAt(2).Name.Should().Be("Crime Comedy");
            result.NotSelectedGenres.ElementAt(3).Name.Should().Be("Comedy");
        }

        [Fact]
        public void MapForBook_IQueryableBookSeriesWithBooksAndBookId_IQueryableSeriesForBookVM()
        {
            var series = GetSeriesWithBooks();
            int bookId = 1;
            var result = series.MapForBook(bookId);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<SeriesForBookVM>>().And.AllBeOfType<SeriesForBookVM>().And.HaveCount(series.Count());
            result.ElementAt(0).Id.Should().Be(1);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(3);
            result.ElementAt(3).Id.Should().Be(4);
            result.ElementAt(0).Title.Should().Be("Title");
            result.ElementAt(1).Title.Should().Be("Example");
            result.ElementAt(2).Title.Should().Be("Test");
            result.ElementAt(3).Title.Should().Be("Another Title");
            result.ElementAt(0).IsForBook.Should().BeTrue();
            result.ElementAt(1).IsForBook.Should().BeFalse();
            result.ElementAt(2).IsForBook.Should().BeFalse();
            result.ElementAt(3).IsForBook.Should().BeFalse();
        }

        [Theory]
        [InlineData(11, 3, 1, 3)]
        [InlineData(11, 3, 2, 1)]
        [InlineData(1, 3, 1, 3)]
        public void MapToList_IQueryableBookSeriesWithBooksAndBookAndPagingAndFiltering_ListSeriesForBookVMWithCorrectAmountOfElementsAndCorrectPaging(int bookId, int pageSize, int pageNo, int pageCount)
        {
            var series = GetSeriesWithBooks();
            int count = series.Count();
            var book = Helpers.GetBook(bookId);
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering();

            var result = series.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForBookVM>();
            result.Book.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.Series.Should().NotBeNull().And.BeOfType<List<SeriesForBookVM>>().And.HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesWithBooksAndBookAndPagingAndFiltering_ListSeriesForBookVMWithOrderedElements()
        {
            var series = GetSeriesWithBooks();
            var book = Helpers.GetBook(1);
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering();

            var result = series.MapForBookToList(book, paging, filtering);

            result.Series.ElementAt(0).Id.Should().Be(1);
            result.Series.ElementAt(1).Id.Should().Be(4);
            result.Series.ElementAt(2).Id.Should().Be(2);
            result.Series.ElementAt(3).Id.Should().Be(3);
            result.Series.ElementAt(0).Title.Should().Be("Title");
            result.Series.ElementAt(1).Title.Should().Be("Another Title");
            result.Series.ElementAt(2).Title.Should().Be("Example");
            result.Series.ElementAt(3).Title.Should().Be("Test");
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesWithBooksAndBookAndPagingAndFiltering_ListSeriesForBookVMWithFilteredElements()
        {
            var series = GetSeriesWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SearchString = "Title" };

            var result = series.MapForBookToList(book, paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForBookVM>();
            result.Series.Should().NotBeNull().And.HaveCount(3);
            result.Series.ElementAt(0).Id.Should().Be(2);
            result.Series.ElementAt(1).Id.Should().Be(4);
            result.Series.ElementAt(2).Id.Should().Be(1);
            result.Series.ElementAt(0).Title.Should().Be("Example");
            result.Series.ElementAt(1).Title.Should().Be("Another Title");
            result.Series.ElementAt(2).Title.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(3);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesWithBooksAndBookAndPagingAndFiltering_ListSeriesForBookVMWithElementsOrderedDescending()
        {
            var series = GetSeriesWithBooks();
            var book = Helpers.GetBook(3);
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = series.MapForBookToList(book, paging, filtering);

            result.Series.ElementAt(0).Id.Should().Be(2);
            result.Series.ElementAt(1).Id.Should().Be(1);
            result.Series.ElementAt(2).Id.Should().Be(3);
            result.Series.ElementAt(3).Id.Should().Be(4);
            result.Series.ElementAt(0).Title.Should().Be("Example");
            result.Series.ElementAt(1).Title.Should().Be("Title");
            result.Series.ElementAt(2).Title.Should().Be("Test");
            result.Series.ElementAt(3).Title.Should().Be("Another Title");
        }

        private IQueryable<Author> GetAuthorsWithBooks()
        {
            var book1 = Helpers.GetBook(1);
            var book2 = Helpers.GetBook(2);

            return new List<Author>()
            {
                new Author(){ Id = 1, Name = "Amanda", LastName = "Popiołek", Books = new List<Domain.Models.Book>() { book1 } },
                new Author(){ Id = 2, Name = "Amanda", LastName = "Adamska", Books = new List<Domain.Models.Book>() { book2 } },
                new Author(){ Id = 3, Name = "Michał", LastName = "Popiołek", Books = new List<Domain.Models.Book>() { book1, book2 } },
                new Author(){ Id = 4, Name = "Piotr", LastName = "Krasowski", Books = new List<Domain.Models.Book>() { book1, book2 } }
            }.AsQueryable();
        }

        private IQueryable<LiteratureGenre> GetGenresWithBooks()
        {
            var book1 = Helpers.GetBook(1);
            var book2 = Helpers.GetBook(2);

            return new List<LiteratureGenre>()
            {
                new LiteratureGenre(){ Id = 1, Name = "Crime Comedy", Books = new List<Domain.Models.Book>() { book1 } },
                new LiteratureGenre(){ Id = 2, Name = "Romantic Comedy", Books = new List<Domain.Models.Book>() { book2 } },
                new LiteratureGenre(){ Id = 3, Name = "Comedy", Books = new List<Domain.Models.Book>() { book1, book2 } },
                new LiteratureGenre(){ Id = 4, Name = "Crime Story", Books = new List<Domain.Models.Book>() { book1, book2 } }
            }.AsQueryable();
        }

        private IQueryable<BookSeries> GetSeriesWithBooks()
        {
            var books = Helpers.GetBooksList(10);

            return new List<BookSeries>()
            {
                new BookSeries(){ Id = 1, Title = "Title", Books = new List<Domain.Models.Book>() { books[0], books[1] } },
                new BookSeries(){ Id = 2, Title = "Example", Books = new List<Domain.Models.Book>() { books[2], books[3], books[4] } },
                new BookSeries(){ Id = 3, Title = "Test", Books = new List<Domain.Models.Book>() { books[5], books[6], books[7], books[8] } },
                new BookSeries(){ Id = 4, Title = "Another Title", Books = new List<Domain.Models.Book>() { books[9] } }
            }.AsQueryable();
        }
    }
}