using Microsoft.AspNetCore.Mvc;
using SoccerSite2.Models;

namespace SoccerSite2.Controllers
{
    public class StandingsController : Controller
    {
        public IActionResult Index()
        {
            Standings model = new Standings();
            model.year = DateTime.Now.Year.ToString();
            return View(model);
        }
    }
}
