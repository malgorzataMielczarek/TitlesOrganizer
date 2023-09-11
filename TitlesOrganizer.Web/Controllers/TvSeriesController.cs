using Microsoft.AspNetCore.Mvc;
using TitlesOrganizer.Application.Interfaces;

namespace TitlesOrganizer.Web.Controllers
{
    public class TvSeriesController : Controller
    {
        private readonly ILogger<TvSeriesController> _logger;
        private readonly ITvSeriesService _tvSeriesService;
        private readonly ICountryService _countryService;

        public TvSeriesController(ILogger<TvSeriesController> logger, ITvSeriesService tvSeriesService, ICountryService countryService)
        {
            _logger = logger;
            _tvSeriesService = tvSeriesService;
            _countryService = countryService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}