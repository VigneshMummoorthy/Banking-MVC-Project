using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using iStart.Models;

public class AccountController : Controller
{

    private static readonly byte[] key = Encoding.UTF8.GetBytes("A1B2C3D4E5F6G7H8");
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("1H2G3F4E5D6C7B8A");

    private static Dictionary<string, string> otpStore = new Dictionary<string, string>();

    [HttpGet]
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Register(UserRegistrationViewModel model)
    {
        //if (ModelState.IsValid)
        //{
        if (model.Email == "admin@gmail.com" && model.Password == "admin@123")
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("Name", "Admin");
        }
        else
        {
            HttpContext.Session.SetString("IsAdmin", "false");
            HttpContext.Session.SetString("Name", model.Email);
        }
        model.Password = EncryptString(model.Password);
        model.ConfirmPassword = model.Password;

        UserStorage.RegisteredUsers.Add(model);

        return RedirectToAction("Index", "Home");

        //}

        //return View(model);
    }

    public ActionResult Index()
    {
        var displayUsers = new List<UserRegistrationViewModel>();
        foreach (var user in UserStorage.RegisteredUsers)
        {
            displayUsers.Add(new UserRegistrationViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender,
                Password = DecryptString(user.Password),
                ConfirmPassword = DecryptString(user.ConfirmPassword)
            });
        }

        return View(displayUsers);
    }

    private string EncryptString(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (MemoryStream ms = new MemoryStream())
            {
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private string DecryptString(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream(System.Convert.FromBase64String(cipherText)))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ForgotPassword(ForgotPasswordViewModel model)
    {
        // Generate OTP
        string otp = new Random().Next(100000, 999999).ToString();
        otpStore[model.Email] = otp;

        Console.WriteLine($"OTP for {model.Email}: {otp}");

        TempData["Email"] = model.Email;
        return RedirectToAction("VerifyOtp");
    }

    [HttpGet]
    public IActionResult VerifyOtp()
    {
        var model = new VerifyOtpViewModel
        {
            Email = TempData["Email"]?.ToString()
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult VerifyOtp(VerifyOtpViewModel model)
    {
        if (otpStore.TryGetValue(model.Email, out var correctOtp) && correctOtp == model.Otp)
        {
            otpStore.Remove(model.Email);
            ViewBag.Message = "Password reset successfully.";
            return RedirectToAction("Login");
        }

        ViewBag.Error = "Invalid OTP.";
        return View(model);
    }
}
