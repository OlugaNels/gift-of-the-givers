using Microsoft.AspNetCore.Mvc;
using GiftOfTheGivers.Website.Data;
using GiftOfTheGivers.Website.Models;
using GiftOfTheGivers.Website.ViewModels;

namespace GiftOfTheGivers.Website.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Donation/Create
        public IActionResult Create()
        {
            return View(new DonationViewModel());
        }

        // POST: /Donation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DonationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var donation = new Donation
            {
                Amount = model.Amount,
                Currency = model.Currency,
                IsRecurring = model.IsRecurring,
                IsAnonymous = model.IsAnonymous,
                DonorName = model.IsAnonymous ? null : model.DonorName,
                DonorEmail = model.IsAnonymous ? null : model.DonorEmail,
                DonationDate = DateTime.Now
            };

            _context.Donations.Add(donation);
            _context.SaveChanges();

            return RedirectToAction("Confirmation", new { id = donation.Id });
        }

        // GET: /Donation/Confirmation/5
        public IActionResult Confirmation(int id)
        {
            var donation = _context.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        // GET: /Donation/Certificate/5
        // Renders the placeholder Section 18A tax certificate for a recorded donation.
        // The page chrome is hidden when printing, so the browser's "Save as PDF" produces
        // the certificate on its own.
        public IActionResult Certificate(int id)
        {
            var donation = _context.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }
    }
}
