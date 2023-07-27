using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TitlesOrganizer.Web.Models;
using TitlesOrganizer.Web.Models.Common;

namespace TitlesOrganizer.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly List<Movie> _movies;

        public MoviesController(ILogger<MoviesController> logger)
        {
            _logger = logger;
            _movies = CreateListOfMovies();
        }

        // GET: MoviesController
        public ActionResult Index()
        {
            var list = GetListOfMovies();

            return View(list);
        }

        // GET: MoviesController/Details/5
        [HttpGet("/Movies/Details/{id?}")]
        public ActionResult Details(int id)
        {
            Movie? movie = GetMovie(id);

            if (movie == null)
            {
                return BadRequest($"No movie with id {id}");
            }

            return View(movie);
        }

        //// GET: MoviesController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: MoviesController/Create
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

        //// GET: MoviesController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: MoviesController/Edit/5
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

        //// GET: MoviesController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: MoviesController/Delete/5
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

        private List<Movie> CreateListOfMovies()
        {
            List<Movie> list = new List<Movie>();
            list.Add(new Movie { Id = 1, Title = "Avengers: Endgame", Categories = { "Action", "Sci-Fi" }, Countries = { "United States" }, Directors = { "Anthony Russo", "Joe Russo" }, OriginalTitle = "Avengers: Endgame", Description = "After the devastating events of Avengers: Infinity War (2018), the universe is in ruins due to the efforts of the Mad Titan, Thanos. With the help of remaining allies, the Avengers must assemble once more in order to undo Thanos's actions and undo the chaos to the universe, no matter what consequences may be in store, and no matter who they face...", Year = 2019 });
            list.Add(new Movie { Id = 2, Title = "Jurassic World", Categories = { "Action", "Adventure", "Sci-Fi" }, Countries = { "United States" }, Description = "A new theme park, built on the original site of Jurassic Park, creates a genetically modified hybrid dinosaur, the Indominus Rex, which escapes containment and goes on a killing spree.", Directors = { "Colin Trevorrow" }, OriginalTitle = "Jurassic World", Year = 2015 });
            list.Add(new Movie { Id = 3, Title = "The Lion King", OriginalTitle = "The Lion King", Categories = { "Animated", "Musical", "Comedy-drama" }, Countries = { "United States" }, Description = "A young lion prince is cast out of his pride by his cruel uncle, who claims he killed his father. While the uncle rules with an iron paw, the prince grows up beyond the Savannah, living by a philosophy: No worries for the rest of your days. But when his past comes to haunt him, the young prince must decide his fate: Will he remain an outcast or face his demons and become what he needs to be?", Directors = { "Roger Allers", "Rob Minkoff" }, Year = 1994 });
            list.Add(new Movie { Id = 4, Title = "The Disaster Artist", OriginalTitle = "The Disaster Artist", Categories = { "Biography", "Comedy", "Drama" }, Countries = { "United States" }, Description = "When aspiring actor Greg Sestero meets the weird and mysterious Tommy Wiseau in an acting class, they form a unique friendship and travel to Hollywood to make their dreams come true.", Directors = { "James Franco" }, Year = 2017 });
            list.Add(new Movie() { Id = 5 });

            return list;
        }

        private IEnumerable<BaseItem> GetListOfMovies()
        {
            return _movies.ConvertAll(movie => movie as BaseItem);
        }

        private Movie? GetMovie(int id)
        {
            return _movies.FirstOrDefault(movie => movie.Id == id);
        }
    }
}