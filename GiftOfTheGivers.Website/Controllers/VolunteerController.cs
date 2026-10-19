using Microsoft.AspNetCore.Mvc;
using GiftOfTheGivers.Website.Data;
using GiftOfTheGivers.Website.Models;
using GiftOfTheGivers.Website.ViewModels;

namespace GiftOfTheGivers.Website.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Volunteer/Create
        public IActionResult Create()
        {
            return View(new VolunteerViewModel());
        }

        // POST: /Volunteer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VolunteerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var volunteer = new Volunteer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Skills = model.Skills,
                Availability = model.Availability,
                Notes = model.Notes,
                JoinedDate = DateTime.Now,
                IsActive = true
            };

            _context.Volunteers.Add(volunteer);
            _context.SaveChanges();

            return RedirectToAction("Confirmation", new { id = volunteer.Id });
        }

        // GET: /Volunteer/Confirmation/5
        public IActionResult Confirmation(int id)
        {
            var volunteer = _context.Volunteers.Find(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }
    }
}
