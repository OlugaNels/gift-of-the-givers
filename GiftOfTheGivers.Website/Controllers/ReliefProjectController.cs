using System;
using System.Linq;
using GiftOfTheGivers.Website.Data;
using GiftOfTheGivers.Website.Models;
using GiftOfTheGivers.Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Website.Controllers
{
    /// <summary>
    /// Public browsing of relief projects, plus employee-only creation, editing and the
    /// posting of progress updates.
    /// </summary>
    public class ReliefProjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReliefProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /ReliefProject
        // Public: anyone may browse the list of relief operations.
        public IActionResult Index()
        {
            var projects = _context.ReliefProjects
                .OrderByDescending(p => p.StartDate)
                .ToList();

            return View(projects);
        }

        // GET: /ReliefProject/Details/5
        // Public: project detail with its progress updates, newest first.
        public IActionResult Details(int id)
        {
            var project = _context.ReliefProjects
                .Include(p => p.Updates)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            project.Updates = project.Updates
                .OrderByDescending(u => u.PostedAt)
                .ToList();

            return View(project);
        }

        // GET: /ReliefProject/Create
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            return View(new ReliefProjectViewModel());
        }

        // POST: /ReliefProject/Create
        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReliefProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = new ReliefProject
            {
                Name = model.Name,
                Description = model.Description,
                Location = model.Location,
                Status = model.Status,
                GoalAmount = model.GoalAmount,
                RaisedAmount = model.RaisedAmount,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            _context.ReliefProjects.Add(project);
            _context.SaveChanges();

            TempData["Message"] = $"Relief project \"{project.Name}\" was created.";
            return RedirectToAction(nameof(Details), new { id = project.Id });
        }

        // GET: /ReliefProject/Edit/5
        [Authorize(Roles = "Employee")]
        public IActionResult Edit(int id)
        {
            var project = _context.ReliefProjects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(new ReliefProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Location = project.Location,
                Status = project.Status,
                GoalAmount = project.GoalAmount,
                RaisedAmount = project.RaisedAmount,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            });
        }

        // POST: /ReliefProject/Edit/5
        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ReliefProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = _context.ReliefProjects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Name = model.Name;
            project.Description = model.Description;
            project.Location = model.Location;
            project.Status = model.Status;
            project.GoalAmount = model.GoalAmount;
            project.RaisedAmount = model.RaisedAmount;
            project.StartDate = model.StartDate;
            project.EndDate = model.EndDate;

            _context.SaveChanges();

            TempData["Message"] = "Project details were updated.";
            return RedirectToAction(nameof(Details), new { id = project.Id });
        }

        // GET: /ReliefProject/PostUpdate/5
        [Authorize(Roles = "Employee")]
        public IActionResult PostUpdate(int id)
        {
            var project = _context.ReliefProjects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(new ProjectUpdateViewModel
            {
                ReliefProjectId = project.Id,
                ProjectName = project.Name
            });
        }

        // POST: /ReliefProject/PostUpdate/5
        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public IActionResult PostUpdate(int id, ProjectUpdateViewModel model)
        {
            var project = _context.ReliefProjects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // Re-populate display-only fields before returning the form.
                model.ReliefProjectId = project.Id;
                model.ProjectName = project.Name;
                return View(model);
            }

            var update = new ProjectUpdate
            {
                ReliefProjectId = project.Id,
                Title = model.Title,
                Body = model.Body,
                // Author and timestamp are set server-side, never accepted from the form.
                PostedBy = User.Identity?.Name ?? "Unknown",
                PostedAt = DateTime.Now
            };

            _context.ProjectUpdates.Add(update);
            _context.SaveChanges();

            TempData["Message"] = "Your progress update was published.";
            return RedirectToAction(nameof(Details), new { id = project.Id });
        }
    }
}
