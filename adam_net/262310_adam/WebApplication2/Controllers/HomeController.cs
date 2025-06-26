using WebApplication2.Models;
using WebApplication2.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;


namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly YouTubeApiService _api;
        private readonly ApplicationDbContext _database;

        public HomeController(YouTubeApiService api, ApplicationDbContext database)
        {
            _api = api;
            _database = database;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string query)
        {
            var results = await _api.SearchVideosAsync(query);
            var ids = _database.Videos.Select(v => v.vID).ToList();
            ViewData["Favorites"] = ids;

            return View("Results", results);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(string vID, string title, string cID, DateTime data_m)
        {
            if (string.IsNullOrWhiteSpace(vID) || vID.Length > 100) ModelState.AddModelError("vID", "Nieprawidłowy identyfikator filmu.");
            if (string.IsNullOrWhiteSpace(title) || title.Length > 100) ModelState.AddModelError("title", "Tytuł jest wymagany i może mieć maks. 100 znaków.");
            if (string.IsNullOrWhiteSpace(cID) || cID.Length > 50) ModelState.AddModelError("cID", "Nieprawidłowy identyfikator kanału.");
            if (data_m > DateTime.Now) ModelState.AddModelError("data_m", "Data publikacji nie może być w przyszłości.");

            var alreadyInDb = _database.Videos.Any(v => v.vID == vID);
            if (!alreadyInDb)
            {
                _database.Videos.Add(new Video
                {
                    vID = vID,
                    Title = title,
                    cID = cID,
                    data_m = data_m,
                    favo = true
                });
                await _database.SaveChangesAsync();
            }

            return Json(new { vID = vID, favo = true });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(string vID)
        {
            var found = _database.Videos.FirstOrDefault(v => v.vID == vID);
            if (found != null)
            {
                _database.Videos.Remove(found);
                await _database.SaveChangesAsync();
            }

            return RedirectToAction("Favorites");
        }

        public IActionResult Favorites(string searchQuery)
        {
            var data = _database.Videos.Where(v => v.favo);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                data = data.Where(v => v.Title.Contains(searchQuery));
            }

            ViewData["SearchQuery"] = searchQuery;

            return View(data.ToList());
        }
    }
}
