// Ignore Spelling: Validator Upsert

using FluentValidation;
using FormHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.CommendVMs.UpsertModelVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.DetailsVMs;
using TitlesOrganizer.Application.ViewModels.BookVMs.ForListVMs;
using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Web.Controllers
{
    public class BooksController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const int SMALL_PAGE_SIZE = 5;
        private readonly IBookService _bookService;
        private readonly IValidator<BookVM> _bookValidator;
        private readonly ILanguageService _languageService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, ILanguageService languageService, IValidator<BookVM> bookValidator)
        {
            _logger = logger;
            _bookService = bookService;
            _languageService = languageService;
            _bookValidator = bookValidator;
        }

        [HttpGet]
        public ActionResult AddGenresForBook(int id)
        {
            ListGenreForBookVM genres = _bookService.GetAllGenresForBookList(id, SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(genres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGenresForBook(int bookId, List<int> genresIds)
        {
            _bookService.AddGenresForBook(bookId, genresIds);
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

                int id = _bookService.AddAuthor(author);

                if (id > 0)
                {
                    string? redirectUri;
                    if (author.BookId == default)
                    {
                        redirectUri = Url.Action(nameof(Authors));
                    }
                    else
                    {
                        redirectUri = Url.Action(nameof(SelectAuthorsForBook), new { id = author.BookId });
                    }

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
            int id = _bookService.AddGenre(genre);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewGenre(int bookId, GenreVM genre)
        {
            int id = _bookService.AddGenre(bookId, genre);
            return View();
        }

        [HttpGet("/Books/Authors/Details/{id}")]
        public ActionResult AuthorDetails(int id)
        {
            AuthorDetailsVM author = _bookService.GetAuthorDetails(id, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, "");
            return View(author);
        }

        [HttpGet]
        public ActionResult Authors()
        {
            ListAuthorForListVM authors = _bookService.GetAllAuthorsForList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(authors);
        }

        [HttpPost]
        public ActionResult Authors(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListAuthorForListVM authors = _bookService.GetAllAuthorsForList(sortBy, pageSize, (int)pageNo, searchString);
            return View(authors);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _bookService.DeleteBook(id);
            return RedirectToAction("Index");
        }

        [HttpGet("/Books/Details/{id?}")]
        public ActionResult Details(int id)
        {
            BookDetailsVM book = _bookService.GetBookDetails(id);
            if (book == null)
            {
                return BadRequest($"No book with id {id}");
            }

            return View(book);
        }

        [HttpGet("/Books/Genres/Details/{id}")]
        public ActionResult GenreDetails(int id)
        {
            GenreDetailsVM genre = _bookService.GetGenreDetails(id, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, "");
            return View(genre);
        }

        [HttpGet]
        public ActionResult Genres()
        {
            ListGenreVM genres = _bookService.GetAllGenres(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(genres);
        }

        [HttpPost]
        public ActionResult Genres(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListGenreVM genres = _bookService.GetAllGenres(sortBy, pageSize, (int)pageNo, searchString);
            return View(genres);
        }

        [HttpGet]
        public ActionResult Index()
        {
            ListBookForListVM list = _bookService.GetAllBooksForList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListBookForListVM list = _bookService.GetAllBooksForList(sortBy, pageSize, (int)pageNo, searchString);
            return View(list);
        }

        [HttpGet]
        public ActionResult Series()
        {
            ListSeriesForListVM series = _bookService.GetAllSeriesForList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(series);
        }

        [HttpPost]
        public ActionResult Series(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListSeriesForListVM series = _bookService.GetAllSeriesForList(sortBy, pageSize, (int)pageNo, searchString);
            return View(series);
        }

        [HttpGet("/Books/Series/Details/{id}")]
        public ActionResult SeriesDetails(int id)
        {
            SeriesDetailsVM genre = _bookService.GetSeriesDetails(id, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, "");
            return View(genre);
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
                book = _bookService.GetBook(id.Value);
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
                int id = _bookService.UpsertBook(book);

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
                int id = _bookService.UpsertBook(book);

                if (id > 0)
                {
                    if (string.IsNullOrEmpty(book.Authors))
                    {
                        ViewData["Title"] = "Update Book";
                        return FormResult.CreateErrorResult("Specify the author of the book.");
                    }

                    if (string.IsNullOrEmpty(book.Genres))
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

        [HttpGet]
        public ActionResult SelectAuthorsForBook(int id)
        {
            ListAuthorForBookVM authors = _bookService.GetAllAuthorsForBookList(id, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, string.Empty);

            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAuthorsForBook(ListAuthorForBookVM listAuthorForBook, int? pageNo)
        {
            listAuthorForBook.CurrentPage = pageNo.HasValue ? pageNo.Value : 1;

            _bookService.SelectAuthorsForBook(listAuthorForBook);
            ListAuthorForBookVM authors = _bookService.GetAllAuthorsForBookList(listAuthorForBook);

            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectAuthorsForBook(ListAuthorForBookVM listAuthorForBook)
        {
            _bookService.SelectAuthorsForBook(listAuthorForBook);

            return RedirectToAction(nameof(GetUpsertBook), new { id = listAuthorForBook.BookId });
        }

        [HttpPost("/Books/Update/AddAuthor")]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewAuthorForBook(ListAuthorForBookVM listAuthorForBook)
        {
            _bookService.SelectAuthorsForBook(listAuthorForBook);
            var author = new AuthorVM() { BookId = listAuthorForBook.BookId, BookTitle = listAuthorForBook.BookTitle };

            return View("AddNewAuthor", author);
        }
    }
}