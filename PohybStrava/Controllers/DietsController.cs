using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;

namespace PohybStrava.Controllers
{
    [Authorize]
    public class DietsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DietsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Diets
        public IActionResult Index(int id)
        {
            IQueryable<Diet> diet = _context.Diet.Where(u => u.Email == this.User.Identity.Name || User.Identity.Name.Contains("admin"));

            bool sportovec = _context.User.Any(u => u.Email == this.User.Identity.Name);
            if (sportovec == false)
            {
                return RedirectToAction("Error", "Diets");
            }
            diet = diet.OrderBy(d => d.DatumDiet);
            return View(diet);
        }

        // Diets - mesicni prehledy
        public IActionResult PrehledMesic()
        {
            var result =
                from s in _context.Diet
                group s by new { date = new DateTime(s.DatumDiet.Year, s.DatumDiet.Month, 1) } into g
                select new Diet
                {
                    DatumDiet = g.Key.date,
                    SoucetEnergieDiet = (int)g.Sum(z => z.Celkem)
                };

            return View(result);
        }

        // Diets - denni prehledy
        public IActionResult SoucetDietsDen()
        {
            var result =
                from s in _context.Diet
                group s by new { date = new DateTime(s.DatumDiet.Year, s.DatumDiet.Month, s.DatumDiet.Day) } into g
                select new Diet
                {
                    DatumDiet = g.Key.date,
                    SoucetEnergieDiet = (int)g.Sum(z => z.Celkem)
                };

            return View(result);
        }


        // GET: Diets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Diet == null)
            {
                return NotFound();
            }

            var diet = await _context.Diet
                .FirstOrDefaultAsync(m => m.DietId == id);
            if (diet == null)
            {
                return NotFound();
            }

            return View(diet);
        }

        // GET: Diets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Diets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Email,DietId,DatumDiet,Potravina,Energie,Mnozstvi,Celkem")] Diet diet, User user)
        {
            if (ModelState.IsValid)
            {
                user = _context.User.FirstOrDefault(u => u.Email == this.User.Identity.Name);
                user.Diets.Add(diet);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", diet.UserId);

            return View(diet);
        }

        // GET: Diets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var diet = await _context.Diet.FindAsync(id);
            if (diet == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", diet.UserId);
            return View(diet);
        }

        // POST: Diets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Email,DietId,DatumDiet,Potravina,Energie,Mnozstvi")] Diet diet)
        {
            if (id != diet.DietId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diet);
                    _context.Entry(diet).Property(x => x.UserId).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DietExists(diet.DietId))
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
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", diet.UserId);
            return View(diet);
        }

        // GET: Diets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Diet == null)
            {
                return NotFound();
            }

            var diet = await _context.Diet
                .FirstOrDefaultAsync(m => m.DietId == id);
            if (diet == null)
            {
                return NotFound();
            }

            return View(diet);
        }

        // POST: Diets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Diet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Diet'  is null.");
            }
            var diet = await _context.Diet.FindAsync(id);
            if (diet != null)
            {
                _context.Diet.Remove(diet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DietExists(int id)
        {
            return _context.Diet.Any(e => e.DietId == id);
        }

        // GET: Diets/Error
        public IActionResult Error()
        {
            if (User.Identity.Name.Contains("admin"))
            { ViewBag.Message = "Tato funkce není pro administrátora dostupná. Přihlaste se jako uživatel."; }

            else
            { ViewBag.Message = "Nejprve vyplňte informace o uživateli."; }
            return View(); ;
        }

    }
}
