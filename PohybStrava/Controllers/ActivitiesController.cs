using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;

namespace PohybStrava.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        public IActionResult Index(int id)
        {
            IQueryable<Activities> activities = _context.Activities.Where(u => u.Email == this.User.Identity.Name || User.Identity.Name.Contains("admin"));

            bool sportovec = _context.User.Any(u => u.Email == this.User.Identity.Name);
            if (sportovec == false)
            {
                return RedirectToAction("Error", "Activities");
            }

            activities = activities.OrderBy(d => d.DatumActivities);
            return View(activities);
        }

        // Activities - mesicni prehledy
        public IActionResult PrehledMesic()
        {
            var result =
               from s in _context.Activities
               group s by new { date = new DateTime(s.DatumActivities.Year, s.DatumActivities.Month, 1) } into g
               select new Activities
               {
                   DatumActivities = g.Key.date,
                   SoucetVzdalenost = (int)g.Sum(x => x.Vzdalenost),
                   SoucetPrevyseni = (int)g.Sum(y => y.Prevyseni),
                   SoucetEnergieActivities = (int)g.Sum(z => z.Energie)
               };

            return View(result);
        }

        // Activities - denni prehledy
        public ActionResult SoucetActivitiesDen()
        {
            var result =
                from s in _context.Activities
                group s by new { date = new DateTime(s.DatumActivities.Year, s.DatumActivities.Month, s.DatumActivities.Day ) } into g
                select new Activities
                {
                    DatumActivities = g.Key.date,
                    SoucetVzdalenost = (int)g.Sum(x => x.Vzdalenost),
                    SoucetPrevyseni = (int)g.Sum(y => y.Prevyseni),
                    SoucetEnergieActivities = (int)g.Sum(z => z.Energie)
                };

            return View(result);
        }


        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Activities == null)
            {
                return NotFound();
            }

            var activities = await _context.Activities
                .FirstOrDefaultAsync(m => m.ActivitiesId == id);
            if (activities == null)
            {
                return NotFound();
            }

            return View(activities);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Email,ActivitiesId,DatumActivities,Trasa,Vzdalenost,Prevyseni,Cas,Tempo,Energie")] Activities activities, User user)
        {
            if (ModelState.IsValid)
            {
                user = _context.User.FirstOrDefault(u => u.Email == this.User.Identity.Name);
                user.Activities.Add(activities);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", activities.UserId);
            return View(activities);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var activities = await _context.Activities.FindAsync(id);
            if (activities == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", activities.UserId);
            return View(activities);
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Email,ActivitiesId,DatumActivities,Trasa,Vzdalenost,Prevyseni,Cas,Tempo,Energie")] Activities activities)
        {
            if (id != activities.ActivitiesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activities);
                    _context.Entry(activities).Property(x => x.UserId).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivitiesExists(activities.ActivitiesId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", activities.UserId);
            return View(activities);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Activities == null)
            {
                return NotFound();
            }

            var activities = await _context.Activities
                .FirstOrDefaultAsync(m => m.ActivitiesId == id);
            if (activities == null)
            {
                return NotFound();
            }

            return View(activities);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Activities == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Activities'  is null.");
            }
            var activities = await _context.Activities.FindAsync(id);
            if (activities != null)
            {
                _context.Activities.Remove(activities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivitiesExists(int id)
        {
            return _context.Activities.Any(e => e.ActivitiesId == id);
        }

        // GET: Activities/Error
        public IActionResult Error()
        {
            if (User.Identity.Name.Contains("admin"))
            {
                ViewBag.Message = "Tato funkce není pro administrátora dostupná. Přihlaste se jako uživatel.";
            }
            else
            {
                ViewBag.Message = "Nejprve vyplňte informace o uživateli.";
            }
            return View();
        }

    }
}
