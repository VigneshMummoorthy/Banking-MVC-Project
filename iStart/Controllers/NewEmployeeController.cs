using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using iStart.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace iStart.Controllers
{
    public class NewEmployeeController : Controller
    {
        private static List<Employee> _employees = new List<Employee>();

        public IActionResult NewEmployee()
        {
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.Password = HttpContext.Session.GetString("Password");

            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Employees = _employees;
            return View();
        }

        [HttpPost]
        public IActionResult SaveEmployee(Employee model)
        {
            //if (ModelState.IsValid)
            //{
                _employees.Add(model);
            //}
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.Password = HttpContext.Session.GetString("Password");

            ViewBag.Employees = _employees;
            return View("NewEmployee");
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }

    }
}
