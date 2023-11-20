// Ignore Spelling: Validator Upsert

using FluentValidation;
using FormHelper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IBookService _bookService;
        private readonly IBookSeriesService _seriesService;
        private readonly ILiteratureGenreService _genreService;
        private readonly IValidator<BookVM> _bookValidator;
        private readonly ILanguageService _languageService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger, IAuthorService authorService, IBookService bookService, IBookSeriesService bookSeriesService, ILiteratureGenreService literatureGenreService, ILanguageService languageService, IValidator<BookVM> bookValidator)
        {
            _logger = logger;
            _authorService = authorService;
            _bookService = bookService;
            _seriesService = bookSeriesService;
            _genreService = literatureGenreService;
            _languageService = languageService;
            _bookValidator = bookValidator;
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

        [HttpGet]
        public ActionResult AddGenresForBook(int id)
        {
            ListGenreForBookVM genres = _genreService.GetListForBook(id, SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(genres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGenresForBook(int bookId, int[] genresIds)
        {
            _bookService.SelectGenres(bookId, genresIds);
            return View(bookId);
        }

        [HttpGet]
        public ActionResult AddNewAuthor()
        {
            return View(new AuthorVM());
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewAuthor(AuthorVM author)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(author.Name) && string.IsNullOrWhiteSpace(author.LastName))
                {
                    return FormResult.CreateErrorResult("Enter name or/and last name of the author.");
                }

                int id = _authorService.Upsert(author);

                if (id > 0)
                {
                    string? redirectUri;
                    //if (author.BookId == default)
                    //{
                    redirectUri = Url.Action(nameof(Authors));
                    //}
                    //else
                    //{
                    //  redirectUri = Url.Action(nameof(SelectAuthorsForBook), new { id = author.BookId });
                    //}

                    return FormResult.CreateSuccessResult("New author added.", redirectUri);
                }
            }

            return FormResult.CreateErrorResult("Check entered data.");
        }

        [HttpGet]
        public ActionResult AddNewGenre()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewGenre(GenreVM genre)
        {
            int id = _genreService.Upsert(genre);
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddNewGenre(int bookId, GenreVM genre)
        //{
        //    int id = _genreService.Upsert(bookId, genre);
        //    return View();
        //}

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _bookService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet("/Books/CreateNew")]
        [HttpGet("/Books/Update/{id?}")]
        public ActionResult GetUpsertBook(int? id)
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

        //[HttpPost, FormValidator]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddBook(BookVM book)
        //{
        //    var result = await _bookValidator.ValidateAsync(book);
        //    if (result.IsValid)
        //    {
        //        int id = _bookService.AddBook(book);

        // if (id > 0) { return FormResult.CreateSuccessResult("Book added.", Url.Action("Details",
        // new { id = id })); } }

        //    return result.CreateErrorResult("Check entered data.");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddBook(BookVM book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        int id = _bookService.AddBook(book);

        // if (id > 0) { return RedirectToAction("Details", new { id = id }); } }

        //    return View(book);
        //}

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAuthors(BookVM book)
        {
            if (ModelState.IsValid)
            {
                int id = _bookService.Upsert(book);

                if (id > 0)
                {
                    ViewData["BookTitle"] = book.Title;

                    return FormResult.CreateInfoResult("Select authors of this book", Url.Action(nameof(SelectAuthorsForBook), new { id = id }), 1);
                }
            }

            ViewData["Title"] = "Update Book";
            return FormResult.CreateErrorResultWithObject(book, "You must specify book title first.", Url.Action(nameof(UpsertBook)));
        }

        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public ActionResult UpsertBook(BookVM book)
        {
            if (ModelState.IsValid)
            {
                int id = _bookService.Upsert(book);

                if (id > 0)
                {
                    if (book.Authors.IsNullOrEmpty())
                    {
                        ViewData["Title"] = "Update Book";
                        return FormResult.CreateErrorResult("Specify the author of the book.");
                    }

                    if (book.Genres.IsNullOrEmpty())
                    {
                        ViewData["Title"] = "Update Book";
                        return FormResult.CreateErrorResult("Specify the genre of the book.");
                    }

                    return FormResult.CreateSuccessResult("Book added.", Url.Action("Details", new { id = id }));
                }
            }

            ViewData["Title"] = "Update Book";
            return FormResult.CreateErrorResult("Check entered data.", "/Books/Update");
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
                    id = _bookService.Upsert(new BookVM() { Title = title.Trim() });
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SelectAuthorsForBook(ListAuthorForBookVM listAuthorForBook, int? pageNo)
        //{
        //    listAuthorForBook.Paging.CurrentPage = pageNo.HasValue ? pageNo.Value : 1;

        // //_authorService.SelectForBook(listAuthorForBook); ListAuthorForBookVM authors =
        // _authorService.GetListForBook(listAuthorForBook.Item.Id,
        // listAuthorForBook.Filtering.SortBy, listAuthorForBook.Paging.PageSize,
        // listAuthorForBook.Paging.CurrentPage, listAuthorForBook.Filtering.SearchString);

        //    return View(authors);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectAuthorsForBook(bool closeModal, int bookId, SortByEnum sortBy, int pageSize, int? pageNo, string? searchString, int[] ids)
        {
            _bookService.SelectAuthors(bookId, ids);

            if (closeModal)
            {
                //var authors = string.Join(", ", .Select(a => a.Description));
                return Ok(_bookService.GetDetails(bookId).Authors);
            }
            else
            {
                var authors = _authorService.GetListForBook(bookId, sortBy, pageSize, pageNo ?? 1, searchString);

                return PartialView("_SelectAuthorsForBook", authors);
            }
        }

        [HttpPost("/Books/Update/AddAuthor")]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewAuthorForBook(ListAuthorForBookVM listAuthorForBook)
        {
            //_authorService.SelectForBook(listAuthorForBook);
            var author = new AuthorVM(); //{ BookId = listAuthorForBook.BookId, BookTitle = listAuthorForBook.BookTitle };

            return View("AddNewAuthor", author);
        }
    }
}