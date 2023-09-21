using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TitlesOrganizer.Application.Interfaces;
using TitlesOrganizer.Application.ViewModels.BookVMs;
using TitlesOrganizer.Application.ViewModels.Common;

namespace TitlesOrganizer.Web.Controllers
{
    public class BooksController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const int SMALL_PAGE_SIZE = 3;
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;
        private readonly ILanguageService _languageService;

        public BooksController(ILogger<BooksController> logger, IBookService bookService, ILanguageService languageService)
        {
            _logger = logger;
            _bookService = bookService;
            _languageService = languageService;
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

        [HttpGet("/Books/Authors/Details/{id}")]
        public ActionResult AuthorDetails(int id)
        {
            AuthorDetailsVM author = _bookService.GetAuthorDetails(id, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, "");
            return View(author);
        }

        [HttpGet("/Books/Genres/Details/{id}")]
        public ActionResult GenreDetails(int id)
        {
            GenreDetailsVM genre = _bookService.GetGenreDetails(id, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, "");
            return View(genre);
        }

        [HttpGet("/Books/Series/Details/{id}")]
        public ActionResult SeriesDetails(int id)
        {
            SeriesDetailsVM genre = _bookService.GetSeriesDetails(id, SortByEnum.Ascending, SMALL_PAGE_SIZE, 1, "");
            return View(genre);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            var languages = _languageService.GetAllLanguagesForList();
            ViewBag.Languages = languages.Languages.Select(lang => new SelectListItem(lang.Name, lang.Code));

            return View(new BookVM() { Title = string.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBook(BookVM book)
        {
            int id = _bookService.AddBook(book);
            return View(id);
        }

        [HttpGet]
        public ActionResult AddAuthorsForBook(int id)
        {
            ListAuthorForBookVM authors = _bookService.GetAllAuthorsForBookList(id, SortByEnum.Ascending, PAGE_SIZE, 1, "");
            return View(authors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAuthorsForBook(int bookId, List<int> authorsIds)
        {
            _bookService.AddAuthorsForBook(bookId, authorsIds);
            return View(bookId);
        }

        [HttpGet]
        public ActionResult AddNewAuthor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewAuthor(NewAuthorVM author)
        {
            int id = _bookService.AddAuthor(author);
            return View();
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

        [HttpGet]
        public ActionResult EditBook(int id)
        {
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook(BookVM book)
        {
            _bookService.UpdateBook(book);
            return View();
        }

        [HttpGet]
        public ActionResult DeleteBook(int id)
        {
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBook(int id, bool confirmation)
        {
            if (confirmation)
            {
                _bookService.DeleteBook(id);
            }

            return View();
        }
    }
}