using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TitlesOrganizer.Web.Models;
using TitlesOrganizer.Web.Models.Common;

namespace TitlesOrganizer.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly List<Book> _books;

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
            _books = CreateListOfBooks();
        }

        // GET: BooksController
        public ActionResult Index()
        {
            var list = GetListOfBooks();

            return View(list);
        }

        // GET: BooksController/Details/5
        [HttpGet("/Books/Details/{id?}")]
        public ActionResult Details(int id)
        {
            Book? book = GetBook(id);

            if (book == null)
            {
                return BadRequest($"No book with id {id}");
            }

            return View(book);
        }

        //// GET: BooksController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: BooksController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: BooksController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: BooksController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: BooksController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: BooksController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        private List<Book> CreateListOfBooks()
        {
            var books = new List<Book>();
            books.Add(new Book { Id = 1, Title = "The Diary of a Young Girl", Authors = { "Anne Frank" }, OriginalTitle = "Het Achterhuis", Categories = { "Autobiography", "Coming of Age", "Jewish literature" }, Edition = "The Definitive", Year = 1997, OriginalLanguage = "Dutch", Description = "In 1942, as Nazis occupied Holland, a thirteen-year-old Jewish girl and her family fled their home in Amsterdam and went into hiding. For the next two years, until their whereabouts were betrayed to the Gestapo, they and another family lived cloistered in the secret upstairs rooms of an old office building. Cut off from the outside world, they faced hunger, boredom, the constant cruelties of living in confined quarters, and the ever-present threat of discovery and death.\r\nIn her diary Anne Frank recorded vivid impressions of her experiences during this period. By turns thoughtful, moving, and amusing, Anne’s account offers a fascinating commentary on human courage and frailty and a compelling self-portrait of a sensitive and spirited young woman whose promise was tragically cut short." });
            books.Add(new Book { Id = 2, Title = "Cookbook Everything You Need to Know to Cook Today", Authors = { "Betty Crocker" }, Edition = "13th", OriginalLanguage = "English", OriginalTitle = "Cookbook", Categories = { "Cookbook" }, Description = "The fully updated and revised edition of the cookbook that generations of Americans trust, with more than 375 new recipes—including for air fryers, multi cookers, slow cookers, and more—everything the modern home cook needs to confidently cook today.\r\nFor the past 100 years, Betty Crocker has helped generations of American home cooks, and this is the cookbook that they’ve come to trust. This 13th edition of the Betty Crocker Cookbook is radically refreshed and made with busy families in mind, with more than 375 exclusive, new, and on-trend recipes. Look for 5-ingredient, air fryer, multicooker, and slow cooker recipes throughout, plus ways to use up your on-hand ingredients, dependable cooking guides, and much, much more. For the health-conscious, you’ll find a new veggie-forward chapter, plus gluten-free and vegan recipes, with full nutritional info for all of the 1300+ recipes.\r\nPerfect for makers of any cooking level, this foundational tome offers an introduction to basic kitchen tools and staples plus charts for cooking times and storage, measurement conversions, as well as inspirations to be creative in your cooking. It’s everything a home cook needs for confident cooking and baking at your fingertips, with chapters on appetizers and salads, cookies, cakes, and desserts, and all eating occasions in between. Now in a durable, lay-flat, book format, this comprehensive and indispensable book makes it possible to channel your inner Betty and share great food with those you love.", Year = 2022 });
            books.Add(new Book { Id = 3, Title = "The Sun Also Rises", Authors = { "Ernest Hemingway" }, Year = 1926, Categories = { "Fiction Classics", "Literary Fiction", "Historical adventure", "Novel with a key", "Modernist novel", "Travelogue", "Novel of disillusionment" }, Edition = "First", OriginalLanguage = "English", OriginalTitle = "The Sun Also Rises", Description = "The Sun Also Rises follows a group of young American and British expatriates as they wander through Europe in the mid-1920s. They are all members of the cynical and disillusioned Lost Generation, who came of age during World War I (1914–18). Two of the novel’s main characters, Lady Brett Ashley and Jake Barnes, typify the Lost Generation. Jake, the novel’s narrator, is a journalist and World War I veteran. During the war Jake suffered an injury that rendered him impotent. (The title obliquely references Jake’s injury and what no longer rises because of it.)" });
            books.Add(new Book { Id = 4, Title = "To Kill A Mockingbird", Authors = { "Harper Lee" }, Year = 1988, OriginalTitle = "To Kill A Mockingbird", OriginalLanguage = "English", Edition = "International", Description = "The unforgettable novel of a childhood in a sleepy Southern town and the crisis of conscience that rocked it, To Kill A Mockingbird became both an instant bestseller and a critical success when it was first published in 1960. It went on to win the Pulitzer Prize in 1961 and was later made into an Academy Award-winning film, also a classic.\r\nCompassionate, dramatic, and deeply moving, To Kill A Mockingbird takes readers to the roots of human behavior - to innocence and experience, kindness and cruelty, love and hatred, humor and pathos. Now with over 18 million copies in print and translated into forty languages, this regional story by a young Alabama woman claims universal appeal. Harper Lee always considered her book to be a simple love story. Today it is regarded as a masterpiece of American literature.", Categories = { "Novel", "Southern Gothic", "Coming of Age", "Legal Story", "Domestic Fiction", "Thriller" } });
            books.Add(new Book { Id = 5, Title = "Their Eyes Were Watching God", Authors = { "Zora Neale Hurston" }, Categories = { "Novel", "Psychological Fiction", "Coming of Age" }, Description = "16-year-old Janie Crawford expects her love and life to blossom like the pear tree she sits under while basking in the golden light. Instead, she is married off to an old man who treats her like a working mule, runs away with another man who treats her as an aid for his own ambitions, and finally finds the deep love she desires—just to watch it deteriorate after a hurricane. Their Eyes Were Watching God (1937) is Zora Neale Hurston's lyrical coming-of-age novel about a fair-skinned black woman pushed out into the world to become her own strength.", OriginalTitle = "Their Eyes Were Watching God", Edition = "First", Year = 2006, OriginalLanguage = "English" });

            return books;
        }

        private IEnumerable<BaseItem> GetListOfBooks()
        {
            return _books.Select(book => new BaseItem { Id = book.Id, Title = book.Title });
        }

        private Book? GetBook(int id)
        {
            return _books.FirstOrDefault(book => book.Id == id);
        }
    }
}