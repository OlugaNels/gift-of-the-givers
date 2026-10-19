using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GiftOfTheGivers.Website.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GiftOfTheGivers.Website.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Employee/Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.DonationCount = _context.Donations.Count();
            ViewBag.VolunteerCount = _context.Volunteers.Count();
            ViewBag.ProjectCount = _context.ReliefProjects.Count();
            ViewBag.ActiveProjectCount = _context.ReliefProjects.Count(p => p.Status == "Active");

            ViewBag.Volunteers = _context.Volunteers
                .OrderByDescending(v => v.JoinedDate)
                .ToList();

            ViewBag.Projects = _context.ReliefProjects
                .OrderByDescending(p => p.StartDate)
                .ToList();

            ViewBag.RecentUpdates = _context.ProjectUpdates
                .Include(u => u.ReliefProject)
                .OrderByDescending(u => u.PostedAt)
                .Take(5)
                .ToList();

            return View();
        }
    }
}
