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

            ListAuthorForListVM authors = _authorService.GetList(sortBy, pageSize, (int)pageNo, searchString);
            return View(authors);
        }

        [HttpPost]
        public ActionResult AuthorDetails(int id, int booksPageSize = 1, int booksPageNo = 1, int seriesPageSize = SMALL_PAGE_SIZE, int seriesPageNo = 1, int genresPageSize = SMALL_PAGE_SIZE, int genresPageNo = 1)
        {
            AuthorDetailsVM author = _authorService.GetDetails(id, booksPageSize, booksPageNo, seriesPageSize, seriesPageNo, genresPageSize, genresPageNo);
            if (author.Id == default)
            {
                return BadRequest("No author with given id");
            }

            return PartialView(author);
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

            ListBookForListVM list = _bookService.GetList(sortBy, pageSize, (int)pageNo, searchString);
            return View(list);
        }

        [HttpPost]
        public ActionResult BookDetails(int id)
        {
            BookDetailsVM book = _bookService.GetDetails(id);
            if (book.Id == default)
            {
                return BadRequest("No book with given id");
            }

            return PartialView(book);
        }

        [HttpPost]
        public ActionResult BooksPartial(int bookPageSize, int bookPageNo, int? authorId)
        {
            PartialList<Book> books = new();
            if (authorId.HasValue)
            {
                books = _bookService.GetPartialListForAuthor(authorId.Value, bookPageSize, bookPageNo);
            }

            return PartialView("_BooksPartial", books);
        }

        [HttpGet]
        public ActionResult AddGenresForBook(int id)
        {
            ListGenreForBookVM genres = _genreService.GetListForBook(id, SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(genres);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGenresForBook(int bookId, List<int> genresIds)
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

        [HttpGet("/Books/Genres/Details/{id}")]
        public ActionResult GenreDetails(int id)
        {
            GenreDetailsVM genre = _genreService.GetDetails(id, SMALL_PAGE_SIZE, 1, SMALL_PAGE_SIZE, 1, SMALL_PAGE_SIZE, 1);
            return View(genre);
        }

        [HttpGet]
        public ActionResult Genres()
        {
            ListGenreForListVM genres = _genreService.GetList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(genres);
        }

        [HttpPost]
        public ActionResult Genres(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListGenreForListVM genres = _genreService.GetList(sortBy, pageSize, (int)pageNo, searchString);
            return View(genres);
        }

        [HttpGet]
        public ActionResult Series()
        {
            ListSeriesForListVM series = _seriesService.GetList(SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(series);
        }

        [HttpPost]
        public ActionResult Series(SortByEnum sortBy, int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }

            ListSeriesForListVM series = _seriesService.GetList(sortBy, pageSize, (int)pageNo, searchString);
            return View(series);
        }

        [HttpGet("/Books/Series/Details/{id}")]
        public ActionResult SeriesDetails(int id)
        {
            SeriesDetailsVM genre = _seriesService.GetDetails(id, SMALL_PAGE_SIZE, 1);
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

        [HttpGet]
        public ActionResult SelectAuthorsForBook(int id)
        {
            ListAuthorForBookVM authors = _authorService.GetListForBook(id, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, string.Empty);

            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectAuthorsForBook(ListAuthorForBookVM listAuthorForBook, int? pageNo)
        {
            listAuthorForBook.Paging.CurrentPage = pageNo.HasValue ? pageNo.Value : 1;

            //_authorService.SelectForBook(listAuthorForBook);
            ListAuthorForBookVM authors = _authorService.GetListForBook(listAuthorForBook.Item.Id, listAuthorForBook.Filtering.SortBy, listAuthorForBook.Paging.PageSize, listAuthorForBook.Paging.CurrentPage, listAuthorForBook.Filtering.SearchString);

            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinalizeSelectAuthorsForBook(ListAuthorForBookVM listAuthorForBook)
        {
            //_authorService.SelectForBook(listAuthorForBook);

            return RedirectToAction(nameof(GetUpsertBook), new { id = listAuthorForBook.Item.Id });
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