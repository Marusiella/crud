using crud.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace crud.Controllers
{
   
    public class AuthController : Controller
    {
        TodoContext todoContext = new();
        public IActionResult Regiser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Regiser(Dao dao)
        {
            var u = todoContext.Users.Where(e => e.Email == dao.email).ToList();
            if (u.Count != 0) return View();
            byte[] result;
            using (SHA512 sha512 = SHA512.Create())
            {
                result = sha512.ComputeHash(Encoding.ASCII.GetBytes(dao.password));
            }
            var hash = Encoding.ASCII.GetString(result);
            todoContext.Users.Add(new User()
            {
                Email = dao.email,
                PasswordHash = hash
            });
            todoContext.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Dao dao)
        {
            var u = todoContext.Users.Where(e => e.Email == dao.email).ToList();
            if (u.Count == 0) return View();
            byte[] result;
            using (SHA512 sha512 = SHA512.Create())
            {
                result = sha512.ComputeHash(Encoding.ASCII.GetBytes(dao.password));
            }
            var hash = Encoding.ASCII.GetString(result);
            if (hash == u[0].PasswordHash)
            {
                HttpContext.Session.SetString("email", dao.email);
                HttpContext.Session.SetInt32("id", u[0].Id);

                return RedirectToAction("Index","Home");
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("email");
            return RedirectToAction("Login");
        }
    }
}
