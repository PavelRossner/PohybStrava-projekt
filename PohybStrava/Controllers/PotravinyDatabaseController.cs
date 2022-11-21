using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;

namespace PohybStrava.Controllers
{
    public class PotravinyDatabaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PotravinyDatabaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PotravinyDatabase
        public async Task<IActionResult> Index()
        {
              return View(await _context.PotravinyDatabase.ToListAsync());
        }

        // GET: PotravinyDatabase/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PotravinyDatabase == null)
            {
                return NotFound();
            }

            var potravinyDatabase = await _context.PotravinyDatabase
                .FirstOrDefaultAsync(m => m.PotravinaDatabaseId == id);
            if (potravinyDatabase == null)
            {
                return NotFound();
            }

            return View(potravinyDatabase);
        }

        // GET: PotravinyDatabase/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PotravinyDatabase/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PotravinaDatabaseId,Email,PotravinaDatabase,Jednotka,EnergieDatabase")] PotravinyDatabase potravinyDatabase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(potravinyDatabase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(potravinyDatabase);
        }

        // GET: PotravinyDatabase/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PotravinyDatabase == null)
            {
                return NotFound();
            }

            var potravinyDatabase = await _context.PotravinyDatabase.FindAsync(id);
            if (potravinyDatabase == null)
            {
                return NotFound();
            }
            return View(potravinyDatabase);
        }

        // POST: PotravinyDatabase/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PotravinaDatabaseId,Email,PotravinaDatabase,Jednotka,EnergieDatabase")] PotravinyDatabase potravinyDatabase)
        {
            if (id != potravinyDatabase.PotravinaDatabaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(potravinyDatabase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PotravinyDatabaseExists(potravinyDatabase.PotravinaDatabaseId))
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
            return View(potravinyDatabase);
        }

        // GET: PotravinyDatabase/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PotravinyDatabase == null)
            {
                return NotFound();
            }

            var potravinyDatabase = await _context.PotravinyDatabase
                .FirstOrDefaultAsync(m => m.PotravinaDatabaseId == id);
            if (potravinyDatabase == null)
            {
                return NotFound();
            }

            return View(potravinyDatabase);
        }

        // POST: PotravinyDatabase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PotravinyDatabase == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PotravinyDatabase'  is null.");
            }
            var potravinyDatabase = await _context.PotravinyDatabase.FindAsync(id);
            if (potravinyDatabase != null)
            {
                _context.PotravinyDatabase.Remove(potravinyDatabase);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PotravinyDatabaseExists(int id)
        {
          return _context.PotravinyDatabase.Any(e => e.PotravinaDatabaseId == id);
        }
    }
}
