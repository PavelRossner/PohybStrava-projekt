using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using PohybStrava.Data;
using PohybStrava.Models;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace PohybStrava.Controllers
{
    [Authorize]
    public class PrijemVydejEnergieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrijemVydejEnergieController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PrijemVydejEnergie
        public IActionResult Index()
        {
            IQueryable<PrijemVydejEnergie> prijemVydejEnergie = _context.PrijemVydejEnergie.Where(u => u.Email == this.User.Identity.Name || User.Identity.Name.Contains("admin"));

            bool sportovec = _context.User.Any(u => u.Email == this.User.Identity.Name);
            if (sportovec == false)
            {
                return RedirectToAction("Error", "PrijemVydejEnergie");
            }

            List<DateTime> dateTimes = new List<DateTime>();

            // Get all activities for current user and from those activities select datumactivities
            List<DateTime> activityDateTimes = _context.Activities.Where(a => a.Email == this.User.Identity.Name).Select(a => a.DatumActivities).ToList();
            
            // Get all diest for current user and from those diets select datumdiets
            List<DateTime> dietDateTimes = _context.Diet.Where(a => a.Email == this.User.Identity.Name).Select(a => a.DatumDiet).ToList();

            // Add resulting datetimes to one collection
            dateTimes.AddRange(activityDateTimes);
            dateTimes.AddRange(dietDateTimes);

            // Trim hours, minutes, seconds from all datetimes and remove repeating ones (excess datetimes)
            dateTimes = dateTimes.Select(d => d.Date).Distinct().ToList();

            // Iterate through resulting datetimes and create list of PrijemVydejEnergie
            List<PrijemVydejEnergie> energetickaBilance = dateTimes.Select(dt => 
            {
                PrijemVydejEnergie output = new PrijemVydejEnergie();

                output.DatumDiet = dt;
                output.DatumActivities = dt;


                // Get all activities for the date and current user and for those activities calculate sum of Energie
                output.SoucetEnergieActivities = (int)_context.Activities.Where(a => a.Email == this.User.Identity.Name && a.DatumActivities.Day == dt.Day && a.DatumActivities.Month == dt.Month && a.DatumActivities.Year == dt.Year)
                                                                         .Sum(a => a.Energie);

                // Get all diet for the date and current user and for those diets calculate sum of celkem
                output.SoucetEnergieDiet = (int)_context.Diet.Where(d => d.Email == this.User.Identity.Name && d.DatumDiet.Day == dt.Day && d.DatumDiet.Month == dt.Month && d.DatumDiet.Year == dt.Year)
                                                             .Sum(d => d.Celkem);

                // Get all athletes for the date and current user and for those subjects calculate sum of basal metabolism
                output.BMR = _context.User.Where(u => u.Email == this.User.Identity.Name && u.DatumUser.Day == dt.Day && u.DatumUser.Month == dt.Month && u.DatumUser.Year == dt.Year)
                                          .Sum(u => u.BMR);

                // Return the resulting PrijemVydej
                return output;
            }).ToList();

            return View(energetickaBilance);
        }

        // GET: PrijemVydejEnergie/Details/5
        public IActionResult Details(int id)
        {

            PrijemVydejEnergie prijemVydejEnergie = _context.PrijemVydejEnergie
                .FirstOrDefault(m => m.PrijemVydejId == id);
            if (prijemVydejEnergie == null)
            {
                return NotFound();
            }

            return View(prijemVydejEnergie);
        }

        // GET: PrijemVydejEnergie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PrijemVydejEnergie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrijemVydejEnergie prijemVydejEnergie, Diet diet, Activities activities, User user)
        {
            if (ModelState.IsValid)
            {
                user = _context.User.FirstOrDefault(u => u.Email == this.User.Identity.Name);
                _context.Add(prijemVydejEnergie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prijemVydejEnergie);
        }

        // GET: PrijemVydejEnergie/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var prijemVydejEnergie = await _context.PrijemVydejEnergie.FindAsync(id);
            if (prijemVydejEnergie == null)
            {
                return NotFound();
            }
            return View(prijemVydejEnergie);
        }

        // POST: PrijemVydejEnergie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DatumDiet,SoucetEnergieDiet,SoucetEnergieActivities,BMR,EnergetickaBilance")] PrijemVydejEnergie prijemVydejEnergie)
        {
            if (id != prijemVydejEnergie.PrijemVydejId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prijemVydejEnergie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrijemVydejEnergieExists(prijemVydejEnergie.PrijemVydejId))
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
            return View(prijemVydejEnergie);
        }

        // GET: PrijemVydejEnergie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PrijemVydejEnergie == null)
            {
                return NotFound();
            }

            var prijemVydejEnergie = await _context.PrijemVydejEnergie
                .FirstOrDefaultAsync(m => m.PrijemVydejId == id);
            if (prijemVydejEnergie == null)
            {
                return NotFound();
            }

            return View(prijemVydejEnergie);
        }

        // POST: PrijemVydejEnergie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PrijemVydejEnergie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PrijemVydejEnergie'  is null.");
            }
            var prijemVydejEnergie = await _context.PrijemVydejEnergie.FindAsync(id);
            if (prijemVydejEnergie != null)
            {
                _context.PrijemVydejEnergie.Remove(prijemVydejEnergie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrijemVydejEnergieExists(int id)
        {
            return _context.PrijemVydejEnergie.Any(e => e.PrijemVydejId == id);
        }

        // GET: PrijemVydejEnergie/Error
        public IActionResult Error()
        {
            if (User.Identity.Name.Contains("admin"))
            { ViewBag.Message = "Admin nemůže data upravovat. Přihlaste se jako uživatel."; }

            else
            { ViewBag.Message = "Nejprve vyplňte informace o uživateli."; }
            return View(); ;
        }
    }
}
