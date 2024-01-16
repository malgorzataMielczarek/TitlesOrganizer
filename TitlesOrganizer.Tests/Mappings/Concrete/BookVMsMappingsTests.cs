using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Mappings.Concrete;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Domain.Models.Abstract;
using TitlesOrganizer.Tests.Mappings.Common;

namespace TitlesOrganizer.Tests.Mappings.Concrete
{
    public class BookVMsMappingsTests
    {
        [Fact]
        public void Filter_IQueryableAuthorAndSearchString_FilteredIQuerable()
        {
            var authors = Helpers.BookModuleHelpers.GetAuthorsList(11).AsQueryable();
            var searchString = "1 Last Name1";
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(authors, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<Creator>>().And
                .HaveCount(2).And
                .NotContainNulls().And
                .AllBeOfType<Creator>().And
                .SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be(1);
                    first.Name.Should().Be("Name1");
                    first.LastName.Should().Be("Last Name1");
                },
                second =>
                {
                    second.Id.Should().Be(11);
                    second.Name.Should().Be("Name11");
                    second.LastName.Should().Be("Last Name11");
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Filter_IQueryableAuthorAndNullOrEmpty_GivenIQuerable(string searchString)
        {
            var authors = Helpers.BookModuleHelpers.GetAuthorsList(5).AsQueryable();
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(authors, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<Creator>>().And
                .HaveCount(5).And
                .NotContainNulls().And
                .AllBeOfType<Creator>().And
                .Equal(authors);
        }

        [Fact]
        public void Filter_IQueryableBookAndSearchString_FilteredIQuerable()
        {
            var books = Helpers.BookModuleHelpers.GetBooksList(11).AsQueryable();
            var searchString = "Title1";
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(books, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<Book>>().And
                .HaveCount(3).And
                .NotContainNulls().And
                .AllBeOfType<Book>().And
                .SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be(1);
                    first.Title.Should().Be("Title1");
                },
                second =>
                {
                    second.Id.Should().Be(10);
                    second.Title.Should().Be("Title10");
                },
                third =>
                {
                    third.Id.Should().Be(11);
                    third.Title.Should().Be("Title11");
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Filter_IQueryableBookAndNullOrEmpty_GivenIQuerable(string searchString)
        {
            var books = Helpers.BookModuleHelpers.GetBooksList(5).AsQueryable();
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(books, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<Book>>().And
                .HaveCount(5).And
                .NotContainNulls().And
                .AllBeOfType<Book>().And
                .Equal(books);
        }

        [Fact]
        public void Filter_IQueryableBookSeriesAndSearchString_FilteredIQuerable()
        {
            var series = Helpers.BookModuleHelpers.GetSeriesList(11).AsQueryable();
            var searchString = "Title1";
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(series, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<BookSeries>>().And
                .HaveCount(3).And
                .NotContainNulls().And
                .AllBeOfType<BookSeries>().And
                .SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be(1);
                    first.Title.Should().Be("Title1");
                },
                second =>
                {
                    second.Id.Should().Be(10);
                    second.Title.Should().Be("Title10");
                },
                third =>
                {
                    third.Id.Should().Be(11);
                    third.Title.Should().Be("Title11");
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Filter_IQueryableBookSeriesAndNullOrEmpty_GivenIQuerable(string searchString)
        {
            var series = Helpers.BookModuleHelpers.GetSeriesList(5).AsQueryable();
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(series, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<BookSeries>>().And
                .HaveCount(5).And
                .NotContainNulls().And
                .AllBeOfType<BookSeries>().And
                .Equal(series);
        }

        [Fact]
        public void Filter_IQueryableLiteratureGenreAndSearchString_FilteredIQuerable()
        {
            var genres = Helpers.BookModuleHelpers.GetGenresList(11).AsQueryable();
            var searchString = "Name1";
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(genres, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<LiteratureGenre>>().And
                .HaveCount(3).And
                .NotContainNulls().And
                .AllBeOfType<LiteratureGenre>().And
                .SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be(1);
                    first.Name.Should().Be("Name1");
                },
                second =>
                {
                    second.Id.Should().Be(10);
                    second.Name.Should().Be("Name10");
                },
                third =>
                {
                    third.Id.Should().Be(11);
                    third.Name.Should().Be("Name11");
                });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Filter_IQueryableLiteratureGenreAndNullOrEmpty_GivenIQuerable(string searchString)
        {
            var genres = Helpers.BookModuleHelpers.GetGenresList(5).AsQueryable();
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(genres, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<LiteratureGenre>>().And
                .HaveCount(5).And
                .NotContainNulls().And
                .AllBeOfType<LiteratureGenre>().And
                .Equal(genres);
        }

        [Fact]
        public void Filter_IQueryableIBaseModelAndSearchString_GivenIQuerable()
        {
            var baseModels = Helpers.BookModuleHelpers.GetGenresList(5).AsQueryable<BaseModel>();
            var searchString = "Name1";
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Filter(baseModels, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<BaseModel>>().And
                .HaveCount(5).And
                .NotContainNulls().And
                .AllBeAssignableTo<BaseModel>().And
                .Equal(baseModels);
        }

        [Fact]
        public void Map_Author_IForListVM()
        {
            var author = Helpers.BookModuleHelpers.GetAuthor(5);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(author);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM>();
            result.Id.Should().Be(author.Id);
            result.Description.Should().Be($"{author.Name} {author.LastName}");
        }

        [Fact]
        public void Map_Book_IForListVM()
        {
            var book = Helpers.BookModuleHelpers.GetBook(5);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(book);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM>();
            result.Id.Should().Be(book.Id);
            result.Description.Should().Be(book.Title);
        }

        [Fact]
        public void Map_BookSeries_IForListVM()
        {
            var series = Helpers.BookModuleHelpers.GetSeries(5);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(series);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM>();
            result.Id.Should().Be(series.Id);
            result.Description.Should().Be(series.Title);
        }

        [Fact]
        public void Map_LiteratureGenre_IForListVM()
        {
            var genre = Helpers.BookModuleHelpers.GetGenre(5);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(genre);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForListVM>();
            result.Id.Should().Be(genre.Id);
            result.Description.Should().Be(genre.Name);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(5, false)]
        public void Map_AuthorAndBook_IForItemVM(int bookId, bool isForBook)
        {
            var author = Helpers.BookModuleHelpers.GetAuthor(3);
            author.Books = Helpers.BookModuleHelpers.GetBooksList(3);
            var book = Helpers.BookModuleHelpers.GetBook(bookId);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(author, book);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(author.Id);
            result.Description.Should().Be($"{author.Name} {author.LastName}");
            result.IsForItem.Should().Be(isForBook);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(5, false)]
        public void Map_BookAndAuthor_IForItemVM(int authorId, bool isForAuthor)
        {
            var book = Helpers.BookModuleHelpers.GetBook(3);
            book.Creators = Helpers.BookModuleHelpers.GetAuthorsList(3);
            var author = Helpers.BookModuleHelpers.GetAuthor(authorId);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(book, author);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(book.Id);
            result.Description.Should().Be(book.Title);
            result.IsForItem.Should().Be(isForAuthor);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(5, false)]
        public void Map_BookAndBookSeries_IForItemVM(int seriesId, bool isForSeries)
        {
            var book = Helpers.BookModuleHelpers.GetBook(3);
            book.Series = Helpers.BookModuleHelpers.GetSeries(1);
            book.SeriesId = 1;
            var series = Helpers.BookModuleHelpers.GetSeries(seriesId);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(book, series);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(book.Id);
            result.Description.Should().Be(book.Title);
            result.IsForItem.Should().Be(isForSeries);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(5, false)]
        public void Map_BookAndLiteratureGenre_IForItemVM(int genreId, bool isForGenre)
        {
            var book = Helpers.BookModuleHelpers.GetBook(3);
            book.Genres = Helpers.BookModuleHelpers.GetGenresList(3);
            var genre = Helpers.BookModuleHelpers.GetGenre(genreId);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(book, genre);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(book.Id);
            result.Description.Should().Be(book.Title);
            result.IsForItem.Should().Be(isForGenre);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(5, false)]
        public void Map_BookSeriesAndBook_IForItemVM(int bookId, bool isForBook)
        {
            var series = Helpers.BookModuleHelpers.GetSeries(3);
            series.Books = Helpers.BookModuleHelpers.GetBooksList(3);
            var book = Helpers.BookModuleHelpers.GetBook(bookId);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(series, book);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(series.Id);
            result.Description.Should().Be(series.Title);
            result.IsForItem.Should().Be(isForBook);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(5, false)]
        public void Map_LiteratureGenreAndBook_IForItemVM(int bookId, bool isForBook)
        {
            var genre = Helpers.BookModuleHelpers.GetGenre(3);
            genre.Books = Helpers.BookModuleHelpers.GetBooksList(3);
            var book = Helpers.BookModuleHelpers.GetBook(bookId);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(genre, book);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(genre.Id);
            result.Description.Should().Be(genre.Name);
            result.IsForItem.Should().Be(isForBook);
        }

        [Fact]
        public void Map_AuthorAndItem_IForItemVM()
        {
            var author = Helpers.BookModuleHelpers.GetAuthor(3);
            var item = new Item(1);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(author, item);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(author.Id);
            result.Description.Should().Be($"{author.Name} {author.LastName}");
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void Map_BookAndItem_IForItemVM()
        {
            var book = Helpers.BookModuleHelpers.GetBook(3);
            var item = new Item(1);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(book, item);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(book.Id);
            result.Description.Should().Be(book.Title);
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void Map_BookSeriesAndItem_IForItemVM()
        {
            var series = Helpers.BookModuleHelpers.GetSeries(3);
            var item = new Item(1);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(series, item);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(series.Id);
            result.Description.Should().Be(series.Title);
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void Map_LiteratureGenreAndItem_IForItemVM()
        {
            var genre = Helpers.BookModuleHelpers.GetGenre(3);
            var item = new Item(1);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(genre, item);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(genre.Id);
            result.Description.Should().Be(genre.Name);
            result.IsForItem.Should().BeFalse();
        }

        [Fact]
        public void Map_ItemAndItem_IForItemVM()
        {
            var item = new Item(3);
            var forItem = new Item(5);
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Map(item, forItem);

            result.Should()
                .NotBeNull().And
                .BeAssignableTo<IForItemVM>();
            result.Id.Should().Be(item.Id);
            result.Description.Should().NotBeNull().And.BeEmpty();
            result.IsForItem.Should().BeFalse();
        }
    }
}