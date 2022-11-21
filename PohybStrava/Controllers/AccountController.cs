using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PohybStrava.Data;
using PohybStrava.Models;
using System.Security.Claims;
using System.Security.Policy;

namespace PohybStrava.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext db;

        private readonly ApplicationDbContext _context;

        public AccountController
        (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext db,
            ApplicationDbContext context
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
            _context = context;
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Jmeno = model.Jmeno,
                    Prijmeni = model.Prijmeni,
                };

                db.SaveChanges();

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    var modeluser = new User
                    {
                        Email = model.Email,
                        Jmeno = model.Jmeno,
                        Prijmeni = model.Prijmeni,
                    };

                    return RedirectToLocal(returnUrl);

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string navratovaURL = null)
        {
            ViewData["ReturnUrl"] = navratovaURL;
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult vysledekOvereni = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (vysledekOvereni.Succeeded)
                {
                    return RedirectToLocal(navratovaURL);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Neplatné přihlašovací údaje.");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public IActionResult Administration()
        {
            return View();
        }

        public IActionResult Overview()
        {
            var users = _context.User.Where(u => u.Email == this.User.Identity.Name || User.Identity.Name.Contains("admin"));

            return View(users);
        }

        public IActionResult UsersDatabase()
        {
            return View(db.Users.ToList());
        }



        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Overview));
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> DeleteAsp(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("DeleteAsp")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAspConfirmed(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Overview));
        }
    }
}


