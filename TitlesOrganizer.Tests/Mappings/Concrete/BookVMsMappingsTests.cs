using AutoMapper;
using FluentAssertions;
using Moq;
using TitlesOrganizer.Application.Mappings.Concrete;
using TitlesOrganizer.Application.ViewModels.Concrete.BookVMs;
using TitlesOrganizer.Domain.Models;
using TitlesOrganizer.Domain.Models.Abstract;

namespace TitlesOrganizer.Tests.Mappings.Concrete
{
    public class BookVMsMappingsTests
    {
        [Fact]
        public void Filter_IQueryableAuthorAndSearchString_FilteredIQuerable()
        {
            var authors = Helpers.BookModuleHelpers.GetAuthorsList(11).AsQueryable();
            var searchString = "1 Name1";
            var mapper = new Mock<IMapper>().Object;
            var mappings = new BookVMsMappings(mapper);

            var result = mappings.Filter(authors, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<Author>>().And
                .HaveCount(2).And
                .NotContainNulls().And
                .AllBeOfType<Author>().And
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
                .BeAssignableTo<IQueryable<Author>>().And
                .HaveCount(5).And
                .NotContainNulls().And
                .AllBeOfType<Author>().And
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
            var baseModels = Helpers.BookModuleHelpers.GetGenresList(5).AsQueryable<IBaseModel>();
            var searchString = "Name1";
            var mapper = new Mock<IMapper>().Object;
            var mapping = new BookVMsMappings(mapper);

            var result = mapping.Filter(baseModels, searchString);

            result.Should()
                .NotBeNullOrEmpty().And
                .BeAssignableTo<IQueryable<IBaseModel>>().And
                .HaveCount(5).And
                .NotContainNulls().And
                .AllBeAssignableTo<IBaseModel>().And
                .Equal(baseModels);
        }
    }
}