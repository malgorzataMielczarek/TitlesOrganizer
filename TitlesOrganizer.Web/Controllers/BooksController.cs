// Ignore Spelling: Validator Upsert

using FluentValidation;
using FormHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.Base;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Helpers;
using TitlesOrganizer.Domain.Models;

namespace TitlesOrganizer.Web.Controllers
{
    public class BooksController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const int SMALL_PAGE_SIZE = 5;
        private readonly IAuthorService _authorService;
        private readonly IValidator<AuthorVM> _authorValidator;
        private readonly IBookService _bookService;
        private readonly IValidator<BookVM> _bookValidator;
        private readonly ILiteratureGenreService _genreService;
        private readonly IValidator<GenreVM> _genreValidator;
        private readonly ILanguageService _languageService;
        private readonly ILogger<BooksController> _logger;
        private readonly IBookSeriesService _seriesService;
        private readonly IValidator<SeriesVM> _seriesValidator;

        public BooksController(ILogger<BooksController> logger, IAuthorService authorService, IBookService bookService, IBookSeriesService bookSeriesService, ILiteratureGenreService literatureGenreService, ILanguageService languageService, IValidator<AuthorVM> authorValidator, IValidator<BookVM> bookValidator, IValidator<GenreVM> genreValidator, IValidator<SeriesVM> seriesValidator)
        {
            _logger = logger;
            _authorService = authorService;
            _bookService = bookService;
            _seriesService = bookSeriesService;
            _genreService = literatureGenreService;
            _languageService = languageService;
            _authorValidator = authorValidator;
            _bookValidator = bookValidator;
            _genreValidator = genreValidator;
            _seriesValidator = seriesValidator;
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewAuthorForBook(int bookId, string newAuthorName, string newAuthorLastName, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            if (string.IsNullOrWhiteSpace(newAuthorName) && string.IsNullOrWhiteSpace(newAuthorLastName))
            {
                return BadRequest("Enter name or/and last name of the author.");
            }

            var author = new AuthorVM { Name = newAuthorName?.Trim(), LastName = newAuthorLastName?.Trim() };
            var validationResult = _authorValidator.Validate(author);
            if (validationResult.IsValid)
            {
                int id = _authorService.Upsert(author);
                if (id > 0)
                {
                    _bookService.SelectAuthors(bookId, ids.Append(id).ToArray());
                    var authors = _authorService.GetListForBook(bookId, sortBy, pageSize, pageNo ?? 1, searchString);
                    return PartialView("_SelectAuthorsForBook", authors);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to add new author. Try to repeat performed operation after some time.");
                }
            }
            else
            {
                var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewBookForAuthor(int authorId, string newBookTitle, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            var book = new BookVM { Title = newBookTitle.Trim() };
            var validationResult = _bookValidator.Validate(book);
            if (validationResult.IsValid)
            {
                int id = _bookService.Upsert(book);
                if (id > 0)
                {
                    _authorService.SelectBooks(authorId, ids.Append(id).ToArray());
                    var books = _bookService.GetListForAuthor(authorId, sortBy, pageSize, pageNo ?? 1, searchString);
                    return PartialView("_SelectBooksForAuthor", books);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to add new book. Try to repeat performed operation after some time.");
                }
            }
            else
            {
                var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewBookForGenre(int genreId, string newBookTitle, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            var book = new BookVM { Title = newBookTitle.Trim() };
            var validationResult = _bookValidator.Validate(book);
            if (validationResult.IsValid)
            {
                int id = _bookService.Upsert(book);
                if (id > 0)
                {
                    _genreService.SelectBooks(genreId, ids.Append(id).ToArray());
                    var books = _bookService.GetListForGenre(genreId, sortBy, pageSize, pageNo ?? 1, searchString);
                    return PartialView("_SelectBooksForGenre", books);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to add new book. Try to repeat performed operation after some time.");
                }
            }
            else
            {
                var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewBookForSeries(int seriesId, string newBookTitle, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            var book = new BookVM { Title = newBookTitle.Trim() };
            var validationResult = _bookValidator.Validate(book);
            if (validationResult.IsValid)
            {
                int id = _bookService.Upsert(book);
                if (id > 0)
                {
                    _seriesService.SelectBooks(seriesId, ids.Append(id).ToArray());
                    var books = _bookService.GetListForSeries(seriesId, sortBy, pageSize, pageNo ?? 1, searchString);
                    return PartialView("_SelectBooksForSeries", books);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to add new book. Try to repeat performed operation after some time.");
                }
            }
            else
            {
                var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewGenreForBook(int bookId, string newGenreName, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            var genre = new GenreVM { Name = newGenreName.Trim() };
            var validationResult = _genreValidator.Validate(genre);
            if (validationResult.IsValid)
            {
                int id = _genreService.Upsert(genre);
                if (id > 0)
                {
                    _bookService.SelectGenres(bookId, ids.Append(id).ToArray());
                    var genres = _genreService.GetListForBook(bookId, sortBy, pageSize, pageNo ?? 1, searchString);
                    return PartialView("_SelectGenresForBook", genres);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to add new genre. Try to repeat performed operation after some time.");
                }
            }
            else
            {
                var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewSeriesForBook(int bookId, string newSeriesTitle, string? newSeriesOriginalTitle, string? newSeriesDescription, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString)
        {
            var series = new SeriesVM()
            {
                Title = newSeriesTitle.Trim(),
                OriginalTitle = newSeriesOriginalTitle?.Trim(),
                Description = newSeriesDescription?.Trim()
            };
            var validationResult = _seriesValidator.Validate(series);
            if (validationResult.IsValid)
            {
                int id = _seriesService.Upsert(series);
                if (id > 0)
                {
                    _bookService.SelectSeries(bookId, id);
                    var seriesList = _seriesService.GetListForBook(bookId, sortBy, pageSize, pageNo ?? 1, searchString);
                    return PartialView("_SelectSeriesForBook", seriesList);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to add new series. Try to repeat performed operation after some time.");
                }
            }
            else
            {
                var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorDelete(int id)
        {
            try
            {
                _authorService.Delete(id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to delete author. Try to repeat performed operation after some time.");
            }

            return Ok("Author deleted");
        }

        [HttpGet]
        public ActionResult AuthorDetails(int id)
        {
            AuthorDetailsVM author = _authorService.GetDetails(id, PAGE_SIZE, 1, SMALL_PAGE_SIZE, 1, SMALL_PAGE_SIZE, 1);
            if (author.Id == default)
            {
                return BadRequest("No author with given id");
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorDetailsPartial(int id, int booksPageSize = 1, int booksPageNo = 1, int seriesPageSize = SMALL_PAGE_SIZE, int seriesPageNo = 1, int genresPageSize = SMALL_PAGE_SIZE, int genresPageNo = 1)
        {
            AuthorDetailsVM author = _authorService.GetDetails(id, booksPageSize, booksPageNo, seriesPageSize, seriesPageNo, genresPageSize, genresPageNo);
            if (author.Id == default)
            {
                return BadRequest("No author with given id");
            }

            return PartialView("AuthorDetails", author);
        }

        [HttpGet]
        public ActionResult Authors()
        {
            ListAuthorForListVM authors = _authorService.GetList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authors(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListAuthorForListVM authors = _authorService.GetList(sortBy, pageSize, pageNo.Value, searchString);
            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorsPartial(int authorsPageSize, int? authorsPageNo, int? genreId)
        {
            PartialList<Author> authors;
            if (!authorsPageNo.HasValue)
            {
                authorsPageNo = 1;
            }

            if (genreId.HasValue)
            {
                authors = _authorService.GetPartialListForGenre(genreId.Value, authorsPageSize, authorsPageNo.Value);
            }
            else
            {
                return BadRequest("No object's id was specified");
            }

            return PartialView("_AuthorsPartial", authors);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult BookDelete(int id)
        {
            try
            {
                _bookService.Delete(id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to delete book. Try to repeat performed operation after some time.");
            }

            return Ok("Book deleted");
        }

        [HttpGet]
        public ActionResult BookDetails(int id)
        {
            BookDetailsVM book = _bookService.GetDetails(id);
            if (book.Id == default)
            {
                return BadRequest("No book with given id");
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookDetailsPartial(int id)
        {
            BookDetailsVM book = _bookService.GetDetails(id);
            if (book.Id == default)
            {
                return BadRequest("No book with given id");
            }

            return PartialView("BookDetails", book);
        }

        [HttpGet]
        public ActionResult Books()
        {
            ListBookForListVM list = _bookService.GetList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Books(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListBookForListVM list = _bookService.GetList(sortBy, pageSize, pageNo.Value, searchString);
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BooksPartial(int booksPageSize, int? booksPageNo, int? authorId, int? genreId, int? seriesId)
        {
            PartialList<Book> books;
            if (!booksPageNo.HasValue)
            {
                booksPageNo = 1;
            }

            if (authorId.HasValue)
            {
                books = _bookService.GetPartialListForAuthor(authorId.Value, booksPageSize, booksPageNo.Value);
            }
            else if (genreId.HasValue)
            {
                books = _bookService.GetPartialListForGenre(genreId.Value, booksPageSize, booksPageNo.Value);
            }
            else if (seriesId.HasValue)
            {
                books = _bookService.GetPartialListForSeries(seriesId.Value, booksPageSize, booksPageNo.Value);
            }
            else
            {
                return BadRequest("No object's id was specified");
            }

            return PartialView("_BooksPartial", books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectAuthorsForBook(bool closeModal, int bookId, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            _bookService.SelectAuthors(bookId, ids);

            if (closeModal)
            {
                var authors = string.Join(", ", _bookService.GetDetails(bookId).Authors.Select(a => a.Description));
                return Ok(authors);
            }
            else
            {
                var authors = _authorService.GetListForBook(bookId, sortBy, pageSize, pageNo ?? 1, searchString);

                return PartialView("_SelectAuthorsForBook", authors);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectBooksForAuthor(bool closeModal, int authorId, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            _authorService.SelectBooks(authorId, ids);

            if (closeModal)
            {
                return Ok("Selecting books finalized");
            }
            else
            {
                var books = _bookService.GetListForAuthor(authorId, sortBy, pageSize, pageNo ?? 1, searchString);

                return PartialView("_SelectBooksForAuthor", books);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectBooksForGenre(bool closeModal, int genreId, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            _genreService.SelectBooks(genreId, ids);

            if (closeModal)
            {
                return Ok("Selecting books finalized");
            }
            else
            {
                var books = _bookService.GetListForGenre(genreId, sortBy, pageSize, pageNo ?? 1, searchString);

                return PartialView("_SelectBooksForGenre", books);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectBooksForSeries(bool closeModal, int seriesId, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            _seriesService.SelectBooks(seriesId, ids);

            if (closeModal)
            {
                return Ok("Selecting books finalized");
            }
            else
            {
                var books = _bookService.GetListForSeries(seriesId, sortBy, pageSize, pageNo ?? 1, searchString);

                return PartialView("_SelectBooksForSeries", books);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectGenresForBook(bool closeModal, int bookId, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            _bookService.SelectGenres(bookId, ids);

            if (closeModal)
            {
                var genres = string.Join(", ", _bookService.GetDetails(bookId).Genres.Select(a => a.Description));
                return Ok(genres);
            }
            else
            {
                var genres = _genreService.GetListForBook(bookId, sortBy, pageSize, pageNo ?? 1, searchString);

                return PartialView("_SelectGenresForBook", genres);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectSeriesForBook(bool closeModal, int bookId, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int? id)
        {
            _bookService.SelectSeries(bookId, id);

            if (closeModal)
            {
                return Ok(_bookService.GetDetails(bookId).Series?.Description ?? string.Empty);
            }
            else
            {
                var series = _seriesService.GetListForBook(bookId, sortBy, pageSize, pageNo ?? 1, searchString);

                return PartialView("_SelectSeriesForBook", series);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult GenreDelete(int id)
        {
            try
            {
                _genreService.Delete(id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to delete genre. Try to repeat performed operation after some time.");
            }

            return Ok("Genre deleted");
        }

        [HttpGet]
        public ActionResult GenreDetails(int id)
        {
            GenreDetailsVM genre = _genreService.GetDetails(id, SMALL_PAGE_SIZE, 1, SMALL_PAGE_SIZE, 1, SMALL_PAGE_SIZE, 1);
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenreDetailsPartial(int id, int booksPageSize = PAGE_SIZE, int booksPageNo = 1, int authorsPageSize = SMALL_PAGE_SIZE, int authorsPageNo = 1, int seriesPageSize = SMALL_PAGE_SIZE, int seriesPageNo = 1)
        {
            GenreDetailsVM genre = _genreService.GetDetails(id, booksPageSize, booksPageNo, authorsPageSize, authorsPageNo, seriesPageSize, seriesPageNo);

            return PartialView("GenreDetails", genre);
        }

        [HttpGet]
        public ActionResult Genres()
        {
            ListGenreForListVM genres = _genreService.GetList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(genres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Genres(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListGenreForListVM genres = _genreService.GetList(sortBy, pageSize, pageNo.Value, searchString);
            return View(genres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenresPartial(int genresPageSize, int? genresPageNo, int? authorId)
        {
            PartialList<LiteratureGenre> genres;
            if (!genresPageNo.HasValue)
            {
                genresPageNo = 1;
            }

            if (authorId.HasValue)
            {
                genres = _genreService.GetPartialListForAuthor(authorId.Value, genresPageSize, genresPageNo.Value);
            }
            else
            {
                return BadRequest("No object's id was specified");
            }

            return PartialView("_GenresPartial", genres);
        }

        [HttpPost]
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAuthorsForBook(int? id, string? title)
        {
            if (!id.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var book = new BookVM() { Title = title.Trim() };
                    var validationResult = _bookValidator.Validate(book);
                    if (validationResult.IsValid)
                    {
                        id = _bookService.Upsert(book);
                    }
                    else
                    {
                        var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                        return BadRequest(errors);
                    }
                }
                else
                {
                    return BadRequest("Enter book title before selecting authors");
                }
            }

            if (id.HasValue)
            {
                var authors = _authorService.GetListForBook(id.Value, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, string.Empty);

                return PartialView("_SelectAuthorsForBook", authors);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to create new book. Try to repeat performed operation after some time.");
            }
        }

        [HttpPost]
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult SelectBooksForAuthor(int? id, string? name, string? lastName)
        {
            if (!id.HasValue)
            {
                if (!(string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(lastName)))
                {
                    var author = new AuthorVM() { Name = name?.Trim(), LastName = lastName?.Trim() };
                    var validationResult = _authorValidator.Validate(author);
                    if (validationResult.IsValid)
                    {
                        id = _authorService.Upsert(author);
                    }
                    else
                    {
                        var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                        return BadRequest(errors);
                    }
                }
                else
                {
                    return BadRequest("Enter author's name and/or last name before selecting books");
                }
            }

            if (id.HasValue)
            {
                var books = _bookService.GetListForAuthor(id.Value, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, string.Empty);

                return PartialView("_SelectBooksForAuthor", books);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to create new author. Try to repeat performed operation after some time.");
            }
        }

        [HttpPost]
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult SelectBooksForGenre(int? id, string? name)
        {
            if (!id.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var genre = new GenreVM() { Name = name.Trim() };
                    var validationResult = _genreValidator.Validate(genre);
                    if (validationResult.IsValid)
                    {
                        id = _genreService.Upsert(genre);
                    }
                    else
                    {
                        var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                        return BadRequest(errors);
                    }
                }
                else
                {
                    return BadRequest("Enter genre's name before selecting books");
                }
            }

            if (id.HasValue)
            {
                var books = _bookService.GetListForGenre(id.Value, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, string.Empty);

                return PartialView("_SelectBooksForGenre", books);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to create new genre. Try to repeat performed operation after some time.");
            }
        }

        [HttpPost]
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult SelectBooksForSeries(int? id, string? title)
        {
            if (!id.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var series = new SeriesVM() { Title = title.Trim() };
                    var validationResult = _seriesValidator.Validate(series);
                    if (validationResult.IsValid)
                    {
                        id = _seriesService.Upsert(series);
                    }
                    else
                    {
                        var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                        return BadRequest(errors);
                    }
                }
                else
                {
                    return BadRequest("Enter book series title before selecting books");
                }
            }

            if (id.HasValue)
            {
                var books = _bookService.GetListForSeries(id.Value, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, string.Empty);

                return PartialView("_SelectBooksForSeries", books);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to create new book series. Try to repeat performed operation after some time.");
            }
        }

        [HttpPost]
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult SelectGenresForBook(int? id, string? title)
        {
            if (!id.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var book = new BookVM() { Title = title.Trim() };
                    var validationResult = _bookValidator.Validate(book);
                    if (validationResult.IsValid)
                    {
                        id = _bookService.Upsert(book);
                    }
                    else
                    {
                        var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                        return BadRequest(errors);
                    }
                }
                else
                {
                    return BadRequest("Enter book title before selecting genres");
                }
            }

            if (id.HasValue)
            {
                var genres = _genreService.GetListForBook(id.Value, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, string.Empty);

                return PartialView("_SelectGenresForBook", genres);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to create new book. Try to repeat performed operation after some time.");
            }
        }

        [HttpPost]
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult SelectSeriesForBook(int? id, string? title)
        {
            if (!id.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    var book = new BookVM() { Title = title.Trim() };
                    var validationResult = _bookValidator.Validate(book);
                    if (validationResult.IsValid)
                    {
                        id = _bookService.Upsert(book);
                    }
                    else
                    {
                        var errors = string.Join("<br />", validationResult.Errors.Select(ve => ve.ErrorMessage));
                        return BadRequest(errors);
                    }
                }
                else
                {
                    return BadRequest("Enter book title before selecting book series");
                }
            }

            if (id.HasValue)
            {
                var series = _seriesService.GetListForBook(id.Value, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, string.Empty);

                return PartialView("_SelectSeriesForBook", series);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to create new book. Try to repeat performed operation after some time.");
            }
        }

        [HttpGet]
        public ActionResult Series()
        {
            ListSeriesForListVM series = _seriesService.GetList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(series);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Series(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListSeriesForListVM series = _seriesService.GetList(sortBy, pageSize, pageNo.Value, searchString);
            return View(series);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult SeriesDelete(int id)
        {
            try
            {
                _seriesService.Delete(id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server encountered unexpected state and failed to delete book series. Try to repeat performed operation after some time.");
            }

            return Ok("Book series deleted");
        }

        [HttpGet]
        public ActionResult SeriesDetails(int id)
        {
            SeriesDetailsVM genre = _seriesService.GetDetails(id, PAGE_SIZE, 1);
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeriesDetailsPartial(int id, int booksPageSize = SMALL_PAGE_SIZE, int booksPageNo = 1)
        {
            SeriesDetailsVM genre = _seriesService.GetDetails(id, booksPageSize, booksPageNo);
            return PartialView("SeriesDetails", genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeriesPartial(int seriesPageSize, int? seriesPageNo, int? authorId, int? genreId)
        {
            PartialList<BookSeries> series;
            if (!seriesPageNo.HasValue)
            {
                seriesPageNo = 1;
            }

            if (authorId.HasValue)
            {
                series = _seriesService.GetPartialListForAuthor(authorId.Value, seriesPageSize, seriesPageNo.Value);
            }
            else if (genreId.HasValue)
            {
                series = _seriesService.GetPartialListForGenre(genreId.Value, seriesPageSize, seriesPageNo.Value);
            }
            else
            {
                return BadRequest("No object's id was specified");
            }

            return PartialView("_SeriesPartial", series);
        }

        [HttpGet("/Books/AuthorCreateNew")]
        [HttpGet("/Books/AuthorUpdate/{id?}")]
        public ActionResult UpsertAuthor(int? id)
        {
            AuthorVM author;
            if (id.HasValue)
            {
                ViewData["Title"] = "Update Author";
                author = _authorService.Get(id.Value, SMALL_PAGE_SIZE, 1);
            }
            else
            {
                ViewData["Title"] = "Create New Author";
                author = new AuthorVM();
            }

            return View("UpsertAuthor", author);
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public ActionResult UpsertAuthorSave(AuthorVM author)
        {
            ViewData["Title"] = "Update Author";
            if (ModelState.IsValid)
            {
                var validationResult = _authorValidator.Validate(author);
                if (validationResult.IsValid)
                {
                    int id = _authorService.Upsert(author);

                    if (id > 0)
                    {
                        var savedAuthor = _authorService.Get(id, 1, 1);
                        if (savedAuthor.Books.Paging.Count <= 0)
                        {
                            return FormResult.CreateErrorResult("Specify the books of the author.");
                        }

                        return FormResult.CreateSuccessResult("Changes saved.", Url.Action("AuthorDetails", new { id = id }));
                    }
                }
                else
                {
                    foreach (var err in validationResult.Errors)
                    {
                        ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                    }
                    var error = "<ul><li>" +
                        string.Join("</li><li>", validationResult.Errors.Select(e => e.ErrorMessage)) +
                        "</li></ul>";
                    return FormResult.CreateErrorResult(error);
                }
            }
            else
            {
                var error = "<ul><li>" +
                    string.Join("</li><li>",
                        ModelState.Values.Where(v => v.ValidationState == ModelValidationState.Invalid)
                        .SelectMany(v => v.Errors)
                        .Select(me => me.ErrorMessage)) +
                    "</li></ul>";
                return FormResult.CreateErrorResult(error);
            }

            return FormResult.CreateErrorResult("Check entered data.");
        }

        [HttpGet("/Books/CreateNew")]
        [HttpGet("/Books/Update/{id?}")]
        public ActionResult UpsertBook(int? id)
        {
            var languages = _languageService.GetAllLanguagesForList();
            ViewBag.Languages = languages.Languages.Select(lang => new SelectListItem(lang.Name, lang.Code));

            BookVM book;
            if (id.HasValue)
            {
                ViewData["Title"] = "Update Book";
                book = _bookService.Get(id.Value);
            }
            else
            {
                ViewData["Title"] = "Create New Book";
                book = new BookVM();
            }

            return View("UpsertBook", book);
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public ActionResult UpsertBookSave(BookVM book)
        {
            ViewData["Title"] = "Update Book";
            if (ModelState.IsValid)
            {
                var validationResult = _bookValidator.Validate(book);
                if (validationResult.IsValid)
                {
                    int id = _bookService.Upsert(book);

                    if (id > 0)
                    {
                        var savedBook = _bookService.Get(id);
                        if (savedBook.Authors.IsNullOrEmpty())
                        {
                            return FormResult.CreateErrorResult("Specify the author of the book.");
                        }

                        if (savedBook.Genres.IsNullOrEmpty())
                        {
                            return FormResult.CreateErrorResult("Specify the genre of the book.");
                        }

                        return FormResult.CreateSuccessResult("Changes saved.", Url.Action("BookDetails", new { id = id }));
                    }
                }
                else
                {
                    foreach (var err in validationResult.Errors)
                    {
                        ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                    }
                    var error = "<ul><li>" +
                        string.Join("</li><li>", validationResult.Errors.Select(e => e.ErrorMessage)) +
                        "</li></ul>";
                    return FormResult.CreateErrorResult(error);
                }
            }
            else
            {
                var error = "<ul><li>" +
                    string.Join("</li><li>",
                        ModelState.Values.Where(v => v.ValidationState == ModelValidationState.Invalid)
                        .SelectMany(v => v.Errors)
                        .Select(me => me.ErrorMessage)) +
                    "</li></ul>";
                return FormResult.CreateErrorResult(error);
            }

            return FormResult.CreateErrorResult("Check entered data.");
        }

        [HttpGet("/Books/GenreCreateNew")]
        [HttpGet("/Books/GenreUpdate/{id?}")]
        public ActionResult UpsertGenre(int? id)
        {
            GenreVM genre;
            if (id.HasValue)
            {
                ViewData["Title"] = "Update Genre";
                genre = _genreService.Get(id.Value, SMALL_PAGE_SIZE, 1);
            }
            else
            {
                ViewData["Title"] = "Create New Genre";
                genre = new GenreVM();
            }

            return View("UpsertGenre", genre);
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public ActionResult UpsertGenreSave(GenreVM genre)
        {
            ViewData["Title"] = "Update Genre";
            if (ModelState.IsValid)
            {
                var validationResult = _genreValidator.Validate(genre);
                if (validationResult.IsValid)
                {
                    int id = _genreService.Upsert(genre);

                    if (id > 0)
                    {
                        return FormResult.CreateSuccessResult("Changes saved.", Url.Action("GenreDetails", new { id = id }));
                    }
                }
                else
                {
                    foreach (var err in validationResult.Errors)
                    {
                        ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                    }
                    var error = "<ul><li>" +
                        string.Join("</li><li>", validationResult.Errors.Select(e => e.ErrorMessage)) +
                        "</li></ul>";
                    return FormResult.CreateErrorResult(error);
                }
            }
            else
            {
                var error = "<ul><li>" +
                    string.Join("</li><li>",
                        ModelState.Values.Where(v => v.ValidationState == ModelValidationState.Invalid)
                        .SelectMany(v => v.Errors)
                        .Select(me => me.ErrorMessage)) +
                    "</li></ul>";
                return FormResult.CreateErrorResult(error);
            }

            return FormResult.CreateErrorResult("Check entered data.");
        }

        [HttpGet("/Books/SeriesCreateNew")]
        [HttpGet("/Books/SeriesUpdate/{id?}")]
        public ActionResult UpsertSeries(int? id)
        {
            SeriesVM series;
            if (id.HasValue)
            {
                ViewData["Title"] = "Update Book Series";
                series = _seriesService.Get(id.Value, SMALL_PAGE_SIZE, 1);
            }
            else
            {
                ViewData["Title"] = "Create New Book Series";
                series = new SeriesVM();
            }

            return View("UpsertSeries", series);
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public ActionResult UpsertSeriesSave(SeriesVM series)
        {
            ViewData["Title"] = "Update Book Series";
            if (ModelState.IsValid)
            {
                var validationResult = _seriesValidator.Validate(series);
                if (validationResult.IsValid)
                {
                    int id = _seriesService.Upsert(series);

                    if (id > 0)
                    {
                        var savedSeries = _seriesService.Get(id, 1, 1);
                        if (savedSeries.Books.Paging.Count == 0)
                        {
                            return FormResult.CreateErrorResult("Specify the books in this series.");
                        }

                        return FormResult.CreateSuccessResult("Changes saved.", Url.Action("SeriesDetails", new { id = id }));
                    }
                }
                else
                {
                    foreach (var err in validationResult.Errors)
                    {
                        ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
                    }
                    var error = "<ul><li>" +
                        string.Join("</li><li>", validationResult.Errors.Select(e => e.ErrorMessage)) +
                        "</li></ul>";
                    return FormResult.CreateErrorResult(error);
                }
            }
            else
            {
                var error = "<ul><li>" +
                    string.Join("</li><li>",
                        ModelState.Values.Where(v => v.ValidationState == ModelValidationState.Invalid)
                        .SelectMany(v => v.Errors)
                        .Select(me => me.ErrorMessage)) +
                    "</li></ul>";
                return FormResult.CreateErrorResult(error);
            }

            return FormResult.CreateErrorResult("Check entered data.");
        }
    }
}