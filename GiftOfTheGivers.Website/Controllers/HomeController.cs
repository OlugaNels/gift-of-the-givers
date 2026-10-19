using System.Diagnostics;
using System.Linq;
using GiftOfTheGivers.Website.Data;
using GiftOfTheGivers.Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.ProjectCount = _context.ReliefProjects.Count();
            ViewBag.VolunteerCount = _context.Volunteers.Count();
            ViewBag.DonationCount = _context.Donations.Count();

            // Show the three most recently started active projects on the landing page.
            var active = _context.ReliefProjects
                .Where(p => p.Status == "Active")
                .OrderByDescending(p => p.StartDate)
                .Take(3)
                .ToList();

            return View(active);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
