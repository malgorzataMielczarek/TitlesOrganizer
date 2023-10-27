using AutoMapper;
using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class GenreVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToBase_GenreVM_LiteratureGenre()
        {
            var books = new List<BookForListVM>()
            {
                new BookForListVM()
                {
                    Id = 1,
                    Title = "Title1"
                },
                new BookForListVM()
                {
                    Id = 2,
                    Title = "Title2"
                }
            };
            var genreVM = new GenreVM()
            {
                Id = 1,
                Name = "Genre Name",
                Books = books,
                BooksPaging = new Application.ViewModels.Helpers.Paging() { Count = 2, CurrentPage = 1, PageSize = 1 }
            };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GenreMappings>());
            IMapper mapper = config.CreateMapper();

            var result = genreVM.MapToBase(mapper);

            result.Should().NotBeNull().And.BeOfType<LiteratureGenre>();
            result.Id.Should().Be(genreVM.Id);
            result.Name.Should().Be(genreVM.Name);
            result.Books.Should().BeNullOrEmpty();
        }

        [Fact]
        public void MapFromBase_LiteratureGenreWithBooks_GenreVM()
        {
            int count = 3, currentPage = 1, pageSize = 2, pageCount = 2;
            var genre = Helpers.GetGenre();
            genre.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GenreMappings>());
            IMapper mapper = config.CreateMapper();

            var result = genre.MapFromBase(mapper, paging);

            result.Should().NotBeNull().And.BeOfType<GenreVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().Be(genre.Name);
            result.BooksPaging.Should().Be(paging);
            result.BooksPaging.CurrentPage.Should().Be(currentPage);
            result.BooksPaging.PageSize.Should().Be(pageSize);
            result.BooksPaging.Count.Should().Be(count);
            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Theory]
        [InlineData(1, 1, "Title1", 2, "Title2")]
        [InlineData(3, 5, "Title5", 6, "Title6")]
        public void MapFromBase_LiteratureGenreWithBooks_GenreVMWithProperBooks(int currentPage, int firstBookId, string firstBookTitle, int secondBookId, string secondBookTitle)
        {
            int count = 9, pageSize = 2, pageCount = 2;
            var genre = Helpers.GetGenre();
            genre.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GenreMappings>());
            IMapper mapper = config.CreateMapper();

            var result = genre.MapFromBase(mapper, paging);

            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
            result.Books[0].Id.Should().Be(firstBookId);
            result.Books[0].Title.Should().Be(firstBookTitle);
            result.Books[1].Id.Should().Be(secondBookId);
            result.Books[1].Title.Should().Be(secondBookTitle);
        }

        [Fact]
        public void MapFromBase_LiteratureGenreWithBooksLastPage_GenreVMWithProperNumberOfBooks()
        {
            int count = 9, currentPage = 5, pageSize = 2, pageCount = 1;
            var genre = Helpers.GetGenre();
            genre.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GenreMappings>());
            IMapper mapper = config.CreateMapper();

            var result = genre.MapFromBase(mapper, paging);

            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapFromBase_LiteratureGenreAndBooks_GenreVM()
        {
            int count = 3, currentPage = 1, pageSize = 2, pageCount = 2;
            var genre = Helpers.GetGenre();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GenreMappings>());
            IMapper mapper = config.CreateMapper();

            var result = genre.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Should().NotBeNull().And.BeOfType<GenreVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().Be(genre.Name);
            result.BooksPaging.Should().Be(paging);
            result.BooksPaging.CurrentPage.Should().Be(currentPage);
            result.BooksPaging.PageSize.Should().Be(pageSize);
            result.BooksPaging.Count.Should().Be(count);
            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapFromBase_LiteratureGenreAndNullBooks_GenreVM()
        {
            int currentPage = 1, pageSize = 2;
            var genre = Helpers.GetGenre();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GenreMappings>());
            IMapper mapper = config.CreateMapper();

            var result = genre.MapFromBase(mapper, null, paging);

            result.Should().NotBeNull().And.BeOfType<GenreVM>();
            result.Id.Should().Be(genre.Id);
            result.Name.Should().Be(genre.Name);
            result.BooksPaging.Should().Be(paging);
            result.BooksPaging.CurrentPage.Should().Be(currentPage);
            result.BooksPaging.PageSize.Should().Be(pageSize);
            result.BooksPaging.Count.Should().Be(0);
            result.Books.Should().NotBeNull().And.BeEmpty();
        }

        [Theory]
        [InlineData(1, 1, "Title1", 2, "Title2")]
        [InlineData(3, 5, "Title5", 6, "Title6")]
        public void MapFromBase_LiteratureGenreAndBooks_GenreVMWithProperBooks(int currentPage, int firstBookId, string firstBookTitle, int secondBookId, string secondBookTitle)
        {
            int count = 9, pageSize = 2, pageCount = 2;
            var genre = Helpers.GetGenre();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GenreMappings>());
            IMapper mapper = config.CreateMapper();

            var result = genre.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
            result.Books[0].Id.Should().Be(firstBookId);
            result.Books[0].Title.Should().Be(firstBookTitle);
            result.Books[1].Id.Should().Be(secondBookId);
            result.Books[1].Title.Should().Be(secondBookTitle);
        }

        [Fact]
        public void MapFromBase_LiteratureGenreAndBooksLastPage_GenreVMWithProperNumberOfBooks()
        {
            int count = 9, currentPage = 5, pageSize = 2, pageCount = 1;
            var genre = Helpers.GetGenre();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GenreMappings>());
            IMapper mapper = config.CreateMapper();

            var result = genre.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Books.Should().NotBeNull().And.HaveCount(pageCount);
        }
    }
}