// Ignore Spelling: Queryable

using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs.QueryVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.Book.ViewModels.QueryVMs.ForListVMs
{
    public class MappingExtensionsTests
    {
        [Fact]
        public void Map_Author_AuthorForListVM()
        {
            var author = Helpers.GetAuthor();

            var result = author.Map();

            result.Should().NotBeNull().And.BeOfType<AuthorForListVM>();
            result.Id.Should().Be(author.Id);
            result.FullName.Should().NotBeNullOrWhiteSpace().And.Be(author.Name + " " + author.LastName);
        }

        [Fact]
        public void Map_IQueryableAuthor_IQueryableAuthorForListVM()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count).AsQueryable();

            var result = authors.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<AuthorForListVM>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.FullName.Should().Be(author.Name + " " + author.LastName);
            }
        }

        [Fact]
        public void Map_ICollectionAuthor_ListAuthorForListVM()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count) as ICollection<Author>;

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<AuthorForListVM>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.FullName.Should().Be(author.Name + " " + author.LastName);
            }
        }

        [Fact]
        public void Map_IEnumerableAuthor_ListAuthorForListVM()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count).AsEnumerable();

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<AuthorForListVM>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.FullName.Should().Be(author.Name + " " + author.LastName);
            }
        }

        [Fact]
        public void Map_ListAuthor_ListAuthorForListVM()
        {
            int count = 2;
            var authors = Helpers.GetAuthorsList(count);

            var result = authors.Map();

            result.Should().NotBeNull().And.BeOfType<List<AuthorForListVM>>().And.HaveCount(count);
            for (int i = 0; i < count; i++)
            {
                var authorForList = result.ElementAt(i);
                var author = authors.ElementAt(i);
                authorForList.Id.Should().Be(author.Id);
                authorForList.FullName.Should().Be(author.Name + " " + author.LastName);
            }
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableAuthorAndPaging_IQueryableAuthorForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var authors = Helpers.GetAuthorsList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = authors.MapToList(ref paging);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<AuthorForListVM>>().And.HaveCount(pageCount);
            paging.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            paging.Count.Should().Be(count);
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPaging_IQueryableAuthorForListVMWithOrderedElements()
        {
            IQueryable<Author> authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable(); // After ordering: 2, 5, 1, 3, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };

            var result = authors.MapToList(ref paging);

            result.ElementAt(0).Id.Should().Be(2);
            result.ElementAt(1).Id.Should().Be(5);
            result.ElementAt(2).Id.Should().Be(1);
            result.ElementAt(3).Id.Should().Be(3);
            result.ElementAt(4).Id.Should().Be(4);
            result.ElementAt(0).FullName.Should().Be("Amanda Adamska");
            result.ElementAt(1).FullName.Should().Be("Piotr Krasowski");
            result.ElementAt(2).FullName.Should().Be("Amanda Popiołek");
            result.ElementAt(3).FullName.Should().Be("Michał Popiołek");
            result.ElementAt(4).FullName.Should().Be("Jerzy Szczur");
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var authors = Helpers.GetAuthorsList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Name" };

            var result = authors.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForListVM>();
            result.Authors.Should().HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithOrderedElements()
        {
            IQueryable<Author> authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable(); // After ordering: 2, 5, 1, 3, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };

            var result = authors.MapToList(paging, new Filtering());

            result.Authors.ElementAt(0).Id.Should().Be(2);
            result.Authors.ElementAt(1).Id.Should().Be(5);
            result.Authors.ElementAt(2).Id.Should().Be(1);
            result.Authors.ElementAt(3).Id.Should().Be(3);
            result.Authors.ElementAt(4).Id.Should().Be(4);
            result.Authors.ElementAt(0).FullName.Should().Be("Amanda Adamska");
            result.Authors.ElementAt(1).FullName.Should().Be("Piotr Krasowski");
            result.Authors.ElementAt(2).FullName.Should().Be("Amanda Popiołek");
            result.Authors.ElementAt(3).FullName.Should().Be("Michał Popiołek");
            result.Authors.ElementAt(4).FullName.Should().Be("Jerzy Szczur");
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithFilteredElements()
        {
            IQueryable<Author> authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Ascending, SearchString = "Popiołek" };

            var result = authors.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListAuthorForListVM>();
            result.Authors.Should().NotBeNull().And.HaveCount(2);
            result.Authors.ElementAt(0).Id.Should().Be(1);
            result.Authors.ElementAt(1).Id.Should().Be(3);
            result.Authors.ElementAt(0).FullName.Should().Be("Amanda Popiołek");
            result.Authors.ElementAt(1).FullName.Should().Be("Michał Popiołek");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableAuthorAndPagingAndFiltering_ListAuthorForListVMWithElementsOrderedDescending()
        {
            IQueryable<Author> authors = new List<Author>()
            {
                new Author(){Id = 1, Name = "Amanda", LastName = "Popiołek"},
                new Author(){Id = 2, Name = "Amanda", LastName = "Adamska"},
                new Author(){Id = 3, Name = "Michał", LastName = "Popiołek"},
                new Author(){Id = 4, Name = "Jerzy", LastName = "Szczur"},
                new Author(){Id = 5, Name = "Piotr", LastName = "Krasowski"}
            }.AsQueryable(); // After ordering: 2, 5, 1, 3, 4 - dasc: 4, 3, 1, 5, 2
            var paging = new Paging() { CurrentPage = 1, PageSize = authors.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = authors.MapToList(paging, filtering);

            result.Authors.ElementAt(0).Id.Should().Be(4);
            result.Authors.ElementAt(1).Id.Should().Be(3);
            result.Authors.ElementAt(2).Id.Should().Be(1);
            result.Authors.ElementAt(3).Id.Should().Be(5);
            result.Authors.ElementAt(4).Id.Should().Be(2);
            result.Authors.ElementAt(0).FullName.Should().Be("Jerzy Szczur");
            result.Authors.ElementAt(1).FullName.Should().Be("Michał Popiołek");
            result.Authors.ElementAt(2).FullName.Should().Be("Amanda Popiołek");
            result.Authors.ElementAt(3).FullName.Should().Be("Piotr Krasowski");
            result.Authors.ElementAt(4).FullName.Should().Be("Amanda Adamska");
        }

        [Fact]
        public void Map_Book_BookForListVM()
        {
            var book = Helpers.GetBook();

            var result = book.Map();

            result.Should().NotBeNull().And.BeOfType<BookForListVM>();
            result.Id.Should().Be(book.Id);
            result.Title.Should().NotBeNullOrWhiteSpace().And.Be(book.Title);
        }

        [Fact]
        public void Map_IQueryableBook_IQueryableBookForListVM()
        {
            int count = 2;
            var books = Helpers.GetBooksList(count).AsQueryable();

            var result = books.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<BookForListVM>>().And.AllBeOfType<BookForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(books.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(books.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(books.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(books.LastOrDefault()?.Title);
        }

        [Fact]
        public void Map_ICollectionBook_ListBookForListVM()
        {
            int count = 2;
            var books = Helpers.GetBooksList(count) as ICollection<Domain.Models.Book>;

            var result = books.Map();

            result.Should().NotBeNull().And.BeOfType<List<BookForListVM>>().And.AllBeOfType<BookForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(books.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(books.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(books.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(books.LastOrDefault()?.Title);
        }

        [Fact]
        public void Map_IEnumerableBook_ListBookForListVM()
        {
            int count = 2;
            var books = Helpers.GetBooksList(count).AsEnumerable();

            var result = books.Map();

            result.Should().NotBeNull().And.BeOfType<List<BookForListVM>>().And.AllBeOfType<BookForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(books.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(books.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(books.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(books.LastOrDefault()?.Title);
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableBookAndPaging_IQueryableBookForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var books = Helpers.GetBooksList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = books.MapToList(ref paging);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<BookForListVM>>().And.AllBeOfType<BookForListVM>().And.HaveCount(pageCount);
            paging.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            paging.Count.Should().Be(count);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPaging_IQueryableBookForListVMWithOrderedElements()
        {
            IQueryable<Domain.Models.Book> books = new List<Domain.Models.Book>()
            {
                new Domain.Models.Book(){Id = 1, Title = "Title"},
                new Domain.Models.Book(){Id = 2, Title = "Example"},
                new Domain.Models.Book(){Id = 3, Title = "Test"},
                new Domain.Models.Book(){Id = 4, Title = "Another Title"},
                new Domain.Models.Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };

            var result = books.MapToList(ref paging);

            result.ElementAt(0).Id.Should().Be(4);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(5);
            result.ElementAt(3).Id.Should().Be(3);
            result.ElementAt(4).Id.Should().Be(1);
            result.ElementAt(0).Title.Should().Be("Another Title");
            result.ElementAt(1).Title.Should().Be("Example");
            result.ElementAt(2).Title.Should().Be("One more Title");
            result.ElementAt(3).Title.Should().Be("Test");
            result.ElementAt(4).Title.Should().Be("Title");
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var books = Helpers.GetBooksList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Title" };

            var result = books.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForListVM>();
            result.Books.Should().HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithOrderedElements()
        {
            IQueryable<Domain.Models.Book> books = new List<Domain.Models.Book>()
            {
                new Domain.Models.Book(){Id = 1, Title = "Title"},
                new Domain.Models.Book(){Id = 2, Title = "Example"},
                new Domain.Models.Book(){Id = 3, Title = "Test"},
                new Domain.Models.Book(){Id = 4, Title = "Another Title"},
                new Domain.Models.Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };

            var result = books.MapToList(paging, new Filtering());

            result.Books.ElementAt(0).Id.Should().Be(4);
            result.Books.ElementAt(1).Id.Should().Be(2);
            result.Books.ElementAt(2).Id.Should().Be(5);
            result.Books.ElementAt(3).Id.Should().Be(3);
            result.Books.ElementAt(4).Id.Should().Be(1);
            result.Books.ElementAt(0).Title.Should().Be("Another Title");
            result.Books.ElementAt(1).Title.Should().Be("Example");
            result.Books.ElementAt(2).Title.Should().Be("One more Title");
            result.Books.ElementAt(3).Title.Should().Be("Test");
            result.Books.ElementAt(4).Title.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithFilteredElements()
        {
            IQueryable<Domain.Models.Book> books = new List<Domain.Models.Book>()
            {
                new Domain.Models.Book(){Id = 1, Title = "Title"},
                new Domain.Models.Book(){Id = 2, Title = "Example"},
                new Domain.Models.Book(){Id = 3, Title = "Test"},
                new Domain.Models.Book(){Id = 4, Title = "Another Title"},
                new Domain.Models.Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Ascending, SearchString = "Title" };

            var result = books.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListBookForListVM>();
            result.Books.Should().NotBeNull().And.HaveCount(3);
            result.Books.ElementAt(0).Id.Should().Be(4);
            result.Books.ElementAt(1).Id.Should().Be(5);
            result.Books.ElementAt(2).Id.Should().Be(1);
            result.Books.ElementAt(0).Title.Should().Be("Another Title");
            result.Books.ElementAt(1).Title.Should().Be("One more Title");
            result.Books.ElementAt(2).Title.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(3);
        }

        [Fact]
        public void MapToList_IQueryableBookAndPagingAndFiltering_ListBookForListVMWithElementsOrderedDescending()
        {
            IQueryable<Domain.Models.Book> books = new List<Domain.Models.Book>()
            {
                new Domain.Models.Book(){Id = 1, Title = "Title"},
                new Domain.Models.Book(){Id = 2, Title = "Example"},
                new Domain.Models.Book(){Id = 3, Title = "Test"},
                new Domain.Models.Book(){Id = 4, Title = "Another Title"},
                new Domain.Models.Book(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1 - desc: 1, 3, 5, 2, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = books.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = books.MapToList(paging, filtering);

            result.Books.ElementAt(0).Id.Should().Be(1);
            result.Books.ElementAt(1).Id.Should().Be(3);
            result.Books.ElementAt(2).Id.Should().Be(5);
            result.Books.ElementAt(3).Id.Should().Be(2);
            result.Books.ElementAt(4).Id.Should().Be(4);
            result.Books.ElementAt(0).Title.Should().Be("Title");
            result.Books.ElementAt(1).Title.Should().Be("Test");
            result.Books.ElementAt(2).Title.Should().Be("One more Title");
            result.Books.ElementAt(3).Title.Should().Be("Example");
            result.Books.ElementAt(4).Title.Should().Be("Another Title");
        }

        [Fact]
        public void Map_LiteratureGenre_GenreForListVM()
        {
            var genre = Helpers.GetGenre();

            var result = genre.Map();

            result.Should().NotBeNull().And.BeOfType<GenreForListVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().NotBeNullOrWhiteSpace().And.Be(genre.Name);
        }

        [Fact]
        public void Map_IQueryableLiteratureGenre_IQueryableGenreForListVM()
        {
            int count = 2;
            var genres = Helpers.GetGenresList(count).AsQueryable();

            var result = genres.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<GenreForListVM>>().And.AllBeOfType<GenreForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(genres.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Name.Should().Be(genres.FirstOrDefault()?.Name);
            result.LastOrDefault()?.Id.Should().Be(genres.LastOrDefault()?.Id);
            result.LastOrDefault()?.Name.Should().Be(genres.LastOrDefault()?.Name);
        }

        [Fact]
        public void Map_ICollectionLiteratureGenre_ListGenreForListVM()
        {
            int count = 2;
            var genres = Helpers.GetGenresList(count) as ICollection<LiteratureGenre>;

            var result = genres.Map();

            result.Should().NotBeNull().And.BeOfType<List<GenreForListVM>>().And.AllBeOfType<GenreForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(genres.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Name.Should().Be(genres.FirstOrDefault()?.Name);
            result.LastOrDefault()?.Id.Should().Be(genres.LastOrDefault()?.Id);
            result.LastOrDefault()?.Name.Should().Be(genres.LastOrDefault()?.Name);
        }

        [Fact]
        public void Map_IEnumerableLiteratureGenre_ListGenreForListVM()
        {
            int count = 2;
            var genres = Helpers.GetGenresList(count).AsEnumerable();

            var result = genres.Map();

            result.Should().NotBeNull().And.BeOfType<List<GenreForListVM>>().And.AllBeOfType<GenreForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(genres.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Name.Should().Be(genres.FirstOrDefault()?.Name);
            result.LastOrDefault()?.Id.Should().Be(genres.LastOrDefault()?.Id);
            result.LastOrDefault()?.Name.Should().Be(genres.LastOrDefault()?.Name);
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableLiteratureGenreAndPaging_IQueryableGenreForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var genres = Helpers.GetGenresList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = genres.MapToList(ref paging);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<GenreForListVM>>().And.AllBeOfType<GenreForListVM>().And.HaveCount(pageCount);
            paging.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            paging.Count.Should().Be(count);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPaging_IQueryableGenreForListVMWithOrderedElements()
        {
            IQueryable<LiteratureGenre> genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable(); // After ordering: 3, 5, 1, 2, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };

            var result = genres.MapToList(ref paging);

            result.ElementAt(0).Id.Should().Be(3);
            result.ElementAt(1).Id.Should().Be(5);
            result.ElementAt(2).Id.Should().Be(1);
            result.ElementAt(3).Id.Should().Be(2);
            result.ElementAt(4).Id.Should().Be(4);
            result.ElementAt(0).Name.Should().Be("Comedy");
            result.ElementAt(1).Name.Should().Be("Fantasy");
            result.ElementAt(2).Name.Should().Be("Horror");
            result.ElementAt(3).Name.Should().Be("SF");
            result.ElementAt(4).Name.Should().Be("Thriller");
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var genres = Helpers.GetGenresList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Name" };

            var result = genres.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForListVM>();
            result.Genres.Should().HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithOrderedElements()
        {
            IQueryable<LiteratureGenre> genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable(); // After ordering: 3, 5, 1, 2, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };

            var result = genres.MapToList(paging, new Filtering());

            result.Genres.ElementAt(0).Id.Should().Be(3);
            result.Genres.ElementAt(1).Id.Should().Be(5);
            result.Genres.ElementAt(2).Id.Should().Be(1);
            result.Genres.ElementAt(3).Id.Should().Be(2);
            result.Genres.ElementAt(4).Id.Should().Be(4);
            result.Genres.ElementAt(0).Name.Should().Be("Comedy");
            result.Genres.ElementAt(1).Name.Should().Be("Fantasy");
            result.Genres.ElementAt(2).Name.Should().Be("Horror");
            result.Genres.ElementAt(3).Name.Should().Be("SF");
            result.Genres.ElementAt(4).Name.Should().Be("Thriller");
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithFilteredElements()
        {
            IQueryable<LiteratureGenre> genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Ascending, SearchString = "y" };

            var result = genres.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListGenreForListVM>();
            result.Genres.Should().NotBeNull().And.HaveCount(2);
            result.Genres.ElementAt(0).Id.Should().Be(3);
            result.Genres.ElementAt(1).Id.Should().Be(5);
            result.Genres.ElementAt(0).Name.Should().Be("Comedy");
            result.Genres.ElementAt(1).Name.Should().Be("Fantasy");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(2);
        }

        [Fact]
        public void MapToList_IQueryableLiteratureGenreAndPagingAndFiltering_ListGenreForListVMWithElementsOrderedDescending()
        {
            IQueryable<LiteratureGenre> genres = new List<LiteratureGenre>()
            {
                new LiteratureGenre(){Id = 1, Name = "Horror"},
                new LiteratureGenre(){Id = 2, Name = "SF"},
                new LiteratureGenre(){Id = 3, Name = "Comedy"},
                new LiteratureGenre(){Id = 4, Name = "Thriller"},
                new LiteratureGenre(){Id = 5, Name = "Fantasy"}
            }.AsQueryable(); // After ordering: 3, 5, 1, 2, 4 - desc: 4, 2, 1, 5, 3
            var paging = new Paging() { CurrentPage = 1, PageSize = genres.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = genres.MapToList(paging, filtering);

            result.Genres.ElementAt(0).Id.Should().Be(4);
            result.Genres.ElementAt(1).Id.Should().Be(2);
            result.Genres.ElementAt(2).Id.Should().Be(1);
            result.Genres.ElementAt(3).Id.Should().Be(5);
            result.Genres.ElementAt(4).Id.Should().Be(3);
            result.Genres.ElementAt(0).Name.Should().Be("Thriller");
            result.Genres.ElementAt(1).Name.Should().Be("SF");
            result.Genres.ElementAt(2).Name.Should().Be("Horror");
            result.Genres.ElementAt(3).Name.Should().Be("Fantasy");
            result.Genres.ElementAt(4).Name.Should().Be("Comedy");
        }

        [Fact]
        public void Map_BookSeries_SeriesForListVM()
        {
            var series = Helpers.GetSeries();

            var result = series.Map();

            result.Should().NotBeNull().And.BeOfType<SeriesForListVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().NotBeNullOrWhiteSpace().And.Be(series.Title);
        }

        [Fact]
        public void Map_IQueryableBookSeries_IQueryableSeriesForListVM()
        {
            int count = 2;
            var series = Helpers.GetSeriesList(count).AsQueryable();

            var result = series.Map();

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<SeriesForListVM>>().And.AllBeOfType<SeriesForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(series.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(series.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(series.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(series.LastOrDefault()?.Title);
        }

        [Fact]
        public void Map_ICollectionBookSeries_ListSeriesForListVM()
        {
            int count = 2;
            var series = Helpers.GetSeriesList(count) as ICollection<BookSeries>;

            var result = series.Map();

            result.Should().NotBeNull().And.BeOfType<List<SeriesForListVM>>().And.AllBeOfType<SeriesForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(series.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(series.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(series.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(series.LastOrDefault()?.Title);
        }

        [Fact]
        public void Map_IEnumerableBookSeries_ListSeriesForListVM()
        {
            int count = 2;
            var books = Helpers.GetSeriesList(count).AsEnumerable();

            var result = books.Map();

            result.Should().NotBeNull().And.BeOfType<List<SeriesForListVM>>().And.AllBeOfType<SeriesForListVM>().And.HaveCount(count);
            result.FirstOrDefault()?.Id.Should().Be(books.FirstOrDefault()?.Id);
            result.FirstOrDefault()?.Title.Should().Be(books.FirstOrDefault()?.Title);
            result.LastOrDefault()?.Id.Should().Be(books.LastOrDefault()?.Id);
            result.LastOrDefault()?.Title.Should().Be(books.LastOrDefault()?.Title);
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableBookSeriesAndPaging_IQueryableSeriesForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var series = Helpers.GetSeriesList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };

            var result = series.MapToList(ref paging);

            result.Should().NotBeNull().And.BeAssignableTo<IQueryable<SeriesForListVM>>().And.AllBeOfType<SeriesForListVM>().And.HaveCount(pageCount);
            paging.CurrentPage.Should().Be(pageNo);
            paging.PageSize.Should().Be(pageSize);
            paging.Count.Should().Be(count);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPaging_IQueryableSeriesForListVMWithOrderedElements()
        {
            IQueryable<BookSeries> series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };

            var result = series.MapToList(ref paging);

            result.ElementAt(0).Id.Should().Be(4);
            result.ElementAt(1).Id.Should().Be(2);
            result.ElementAt(2).Id.Should().Be(5);
            result.ElementAt(3).Id.Should().Be(3);
            result.ElementAt(4).Id.Should().Be(1);
            result.ElementAt(0).Title.Should().Be("Another Title");
            result.ElementAt(1).Title.Should().Be("Example");
            result.ElementAt(2).Title.Should().Be("One more Title");
            result.ElementAt(3).Title.Should().Be("Test");
            result.ElementAt(4).Title.Should().Be("Title");
        }

        [Theory]
        [InlineData(3, 1, 5, 3)]
        [InlineData(3, 2, 5, 2)]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithCorrectAmountOfElementsAndCorrectPaging(int pageSize, int pageNo, int count, int pageCount)
        {
            var series = Helpers.GetSeriesList(count).AsQueryable();
            var paging = new Paging() { CurrentPage = pageNo, PageSize = pageSize };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending, SearchString = "Title" };

            var result = series.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForListVM>();
            result.Series.Should().HaveCount(pageCount);
            result.Paging.Should().Be(paging);
            result.Paging.CurrentPage.Should().Be(pageNo);
            result.Paging.PageSize.Should().Be(pageSize);
            result.Paging.Count.Should().Be(count);
            result.Filtering.Should().Be(filtering);
            result.Filtering.SearchString.Should().Be(filtering.SearchString);
            result.Filtering.SortBy.Should().Be(filtering.SortBy);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithOrderedElements()
        {
            IQueryable<BookSeries> series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };

            var result = series.MapToList(paging, new Filtering());

            result.Series.ElementAt(0).Id.Should().Be(4);
            result.Series.ElementAt(1).Id.Should().Be(2);
            result.Series.ElementAt(2).Id.Should().Be(5);
            result.Series.ElementAt(3).Id.Should().Be(3);
            result.Series.ElementAt(4).Id.Should().Be(1);
            result.Series.ElementAt(0).Title.Should().Be("Another Title");
            result.Series.ElementAt(1).Title.Should().Be("Example");
            result.Series.ElementAt(2).Title.Should().Be("One more Title");
            result.Series.ElementAt(3).Title.Should().Be("Test");
            result.Series.ElementAt(4).Title.Should().Be("Title");
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithFilteredElements()
        {
            IQueryable<BookSeries> series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable();
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Ascending, SearchString = "Title" };

            var result = series.MapToList(paging, filtering);

            result.Should().NotBeNull().And.BeOfType<ListSeriesForListVM>();
            result.Series.Should().NotBeNull().And.HaveCount(3);
            result.Series.ElementAt(0).Id.Should().Be(4);
            result.Series.ElementAt(1).Id.Should().Be(5);
            result.Series.ElementAt(2).Id.Should().Be(1);
            result.Series.ElementAt(0).Title.Should().Be("Another Title");
            result.Series.ElementAt(1).Title.Should().Be("One more Title");
            result.Series.ElementAt(2).Title.Should().Be("Title");
            result.Paging.Should().NotBeNull();
            result.Paging.CurrentPage.Should().Be(paging.CurrentPage);
            result.Paging.PageSize.Should().Be(paging.PageSize);
            result.Paging.Count.Should().Be(3);
        }

        [Fact]
        public void MapToList_IQueryableBookSeriesAndPagingAndFiltering_ListSeriesForListVMWithElementsOrderedDescending()
        {
            IQueryable<BookSeries> series = new List<BookSeries>()
            {
                new BookSeries(){Id = 1, Title = "Title"},
                new BookSeries(){Id = 2, Title = "Example"},
                new BookSeries(){Id = 3, Title = "Test"},
                new BookSeries(){Id = 4, Title = "Another Title"},
                new BookSeries(){Id = 5, Title = "One more Title"}
            }.AsQueryable(); // After ordering: 4, 2, 5, 3, 1 - desc: 1, 3, 5, 2, 4
            var paging = new Paging() { CurrentPage = 1, PageSize = series.Count() };
            var filtering = new Filtering() { SortBy = SortByEnum.Descending };

            var result = series.MapToList(paging, filtering);

            result.Series.ElementAt(0).Id.Should().Be(1);
            result.Series.ElementAt(1).Id.Should().Be(3);
            result.Series.ElementAt(2).Id.Should().Be(5);
            result.Series.ElementAt(3).Id.Should().Be(2);
            result.Series.ElementAt(4).Id.Should().Be(4);
            result.Series.ElementAt(0).Title.Should().Be("Title");
            result.Series.ElementAt(1).Title.Should().Be("Test");
            result.Series.ElementAt(2).Title.Should().Be("One more Title");
            result.Series.ElementAt(3).Title.Should().Be("Example");
            result.Series.ElementAt(4).Title.Should().Be("Another Title");
        }
    }
}