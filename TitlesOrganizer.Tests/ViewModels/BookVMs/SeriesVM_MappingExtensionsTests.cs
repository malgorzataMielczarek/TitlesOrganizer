﻿using AutoMapper;
using FluentAssertions;
using TitlesOrganizer.Application.ViewModels.Abstract;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Tests.ViewModels.BookVMs
{
    public class SeriesVM_MappingExtensionsTests
    {
        [Fact]
        public void MapToBase_SeriesVM_BookSeries()
        {
            var books = new List<IForListVM<Book>>()
            {
                new BookForListVM()
                {
                    Id = 1,
                    Description = "Title1"
                },
                new BookForListVM()
                {
                    Id = 2,
                    Description = "Title2"
                }
            };
            var seriesVM = new SeriesVM()
            {
                Id = 1,
                Title = "Series Title",
                OriginalTitle = "Original Title",
                Description = "Description",
                Books = new PartialList<Book>()
                {
                    Values = books,
                    Paging = new Application.ViewModels.Helpers.Paging()
                    {
                        Count = 2,
                        CurrentPage = 1,
                        PageSize = 1
                    }
                }
            };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SeriesMappings>());
            IMapper mapper = config.CreateMapper();

            var result = seriesVM.MapToBase(mapper);

            result.Should().NotBeNull().And.BeOfType<BookSeries>();
            result.Id.Should().Be(seriesVM.Id);
            result.Title.Should().Be(seriesVM.Title);
            result.OriginalTitle.Should().Be(seriesVM.OriginalTitle);
            result.Description.Should().Be(seriesVM.Description);
            result.Books.Should().BeNullOrEmpty();
        }

        [Fact]
        public void MapFromBase_BookSeriesWithBooks_SeriesVM()
        {
            int count = 3, currentPage = 1, pageSize = 2, pageCount = 2;
            var series = Helpers.GetSeries();
            series.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SeriesMappings>());
            IMapper mapper = config.CreateMapper();

            var result = series.MapFromBase(mapper, paging);

            result.Should().NotBeNull().And.BeOfType<SeriesVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
            result.OriginalTitle.Should().Be(series.OriginalTitle);
            result.Description.Should().Be(series.Description);
            result.Books.Paging.Should().Be(paging);
            result.Books.Paging.CurrentPage.Should().Be(currentPage);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.Count.Should().Be(count);
            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Theory]
        [InlineData(1, 1, "Title1", 2, "Title2")]
        [InlineData(3, 5, "Title5", 6, "Title6")]
        public void MapFromBase_BookSeriesWithBooks_SeriesVMWithProperBooks(int currentPage, int firstBookId, string firstBookTitle, int secondBookId, string secondBookTitle)
        {
            int count = 9, pageSize = 2, pageCount = 2;
            var series = Helpers.GetSeries();
            series.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SeriesMappings>());
            IMapper mapper = config.CreateMapper();

            var result = series.MapFromBase(mapper, paging);

            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
            result.Books.Values[0].Id.Should().Be(firstBookId);
            result.Books.Values[0].Description.Should().Be(firstBookTitle);
            result.Books.Values[1].Id.Should().Be(secondBookId);
            result.Books.Values[1].Description.Should().Be(secondBookTitle);
        }

        [Fact]
        public void MapFromBase_BookSeriesWithBooksLastPage_SeriesVMWithProperNumberOfBooks()
        {
            int count = 9, currentPage = 5, pageSize = 2, pageCount = 1;
            var series = Helpers.GetSeries();
            series.Books = Helpers.GetBooksList(count);
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SeriesMappings>());
            IMapper mapper = config.CreateMapper();

            var result = series.MapFromBase(mapper, paging);

            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapFromBase_BookSeriesAndBooks_SeriesVM()
        {
            int count = 3, currentPage = 1, pageSize = 2, pageCount = 2;
            var series = Helpers.GetSeries();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SeriesMappings>());
            IMapper mapper = config.CreateMapper();

            var result = series.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Should().NotBeNull().And.BeOfType<SeriesVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
            result.OriginalTitle.Should().Be(series.OriginalTitle);
            result.Description.Should().Be(series.Description);
            result.Books.Paging.Should().Be(paging);
            result.Books.Paging.CurrentPage.Should().Be(currentPage);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.Count.Should().Be(count);
            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
        }

        [Fact]
        public void MapFromBase_BookSeriesAndNullBooks_SeriesVM()
        {
            int currentPage = 1, pageSize = 2;
            var series = Helpers.GetSeries();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SeriesMappings>());
            IMapper mapper = config.CreateMapper();

            var result = series.MapFromBase(mapper, null, paging);

            result.Should().NotBeNull().And.BeOfType<SeriesVM>();
            result.Id.Should().Be(series.Id);
            result.Title.Should().Be(series.Title);
            result.OriginalTitle.Should().Be(series.OriginalTitle);
            result.Description.Should().Be(series.Description);
            result.Books.Paging.Should().Be(paging);
            result.Books.Paging.CurrentPage.Should().Be(currentPage);
            result.Books.Paging.PageSize.Should().Be(pageSize);
            result.Books.Paging.Count.Should().Be(0);
            result.Books.Values.Should().NotBeNull().And.BeEmpty();
        }

        [Theory]
        [InlineData(1, 1, "Title1", 2, "Title2")]
        [InlineData(3, 5, "Title5", 6, "Title6")]
        public void MapFromBase_BookSeriesAndBooks_SeriesVMWithProperBooks(int currentPage, int firstBookId, string firstBookTitle, int secondBookId, string secondBookTitle)
        {
            int count = 9, pageSize = 2, pageCount = 2;
            var series = Helpers.GetSeries();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SeriesMappings>());
            IMapper mapper = config.CreateMapper();

            var result = series.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
            result.Books.Values[0].Id.Should().Be(firstBookId);
            result.Books.Values[0].Description.Should().Be(firstBookTitle);
            result.Books.Values[1].Id.Should().Be(secondBookId);
            result.Books.Values[1].Description.Should().Be(secondBookTitle);
        }

        [Fact]
        public void MapFromBase_BookSeriesAndBooksLastPage_SeriesVMWithProperNumberOfBooks()
        {
            int count = 9, currentPage = 5, pageSize = 2, pageCount = 1;
            var series = Helpers.GetSeries();
            var paging = new Application.ViewModels.Helpers.Paging() { CurrentPage = currentPage, PageSize = pageSize };
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SeriesMappings>());
            IMapper mapper = config.CreateMapper();

            var result = series.MapFromBase(mapper, Helpers.GetBooksList(count).AsQueryable(), paging);

            result.Books.Values.Should().NotBeNull().And.HaveCount(pageCount);
        }
    }
}