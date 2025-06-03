using System.Diagnostics;
using iStart.Models;
using Microsoft.AspNetCore.Mvc;

namespace iStart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var users = UserStorage.RegisteredUsers.Select(user => new UserRegistrationViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender,
                Password = DecryptString(user.Password),
                ConfirmPassword = DecryptString(user.ConfirmPassword)
            }).ToList();

            return View(users);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRegistrationViewModel model)
        {
            model.Password = EncryptString(model.Password);
            model.ConfirmPassword = model.Password;

            UserStorage.RegisteredUsers.Add(model);

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string EncryptString(string plainText)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }

        private string DecryptString(string encryptedText)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
        }
    }
}
