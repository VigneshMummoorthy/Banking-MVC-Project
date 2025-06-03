using System.Text.RegularExpressions;
using iStart.Data;
using iStart.Helper;
using iStart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace iStart.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserDbContext db = new UserDbContext();

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password, bool rememberMe)
        {
            var specialCharRegex = new Regex(@"^(?=.*[!@#$%^&*(),"".?\:{ }|<>]).+$");

            if (!specialCharRegex.IsMatch(password))
            {
                ModelState.AddModelError("password", "Password must contain at least one special character.");
                return View();
            }
            var user = db.Users.FirstOrDefault(u => u.Email == email);

            if (email == "admin@gmail.com" && password == "admin@123")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                HttpContext.Session.SetString("Name", "Admin");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                HttpContext.Session.SetString("IsAdmin", "false");
                HttpContext.Session.SetString("Name", email);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid credentials";
            return View();
        }
    }
}
