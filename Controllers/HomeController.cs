using crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace crud.Controllers
{
    public class HomeController : Controller
    {
        TodoContext todoContext = new();

        public IActionResult Index()
        {
            if (haveToRedirect()) return RedirectToAction("Login", "Auth");
            return View();
        }

        public IActionResult GetItemsPartial()
        {            
            var id = HttpContext.Session.GetInt32("id");

            var items = todoContext.Todos
                .Join(todoContext.Users, e => e.UserId, ee => ee.Id, (e, ee) => new { todo = e, user = ee })
                .Where(e => e.user.Id == id)
                .Select(e => e.todo)
                .ToList();
            return PartialView("_ItemsPartial", items);
        }

        private bool haveToRedirect()
        {
            var e = HttpContext.Session.GetString("email");
            var id = HttpContext.Session.GetInt32("id");
            return e == null || id == null;
        }

        public IActionResult Add()
        {
            if (haveToRedirect()) return RedirectToAction("Login", "Auth");

            return View();
        }

        [HttpPost]
        public IActionResult Add(Todo todo)
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.Session.GetInt32("id");

                todo.UserId = id.Value;
                todo.CreatedDate = DateTime.Now;
                todoContext.Todos.Add(todo);
                todoContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("id");
            var todo = todoContext.Todos.Find(id);
            if (todo == null) return RedirectToAction("Index");
            todoContext.Todos.Remove(todo);
            todoContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Done(int id)
        {
            var todo = todoContext.Todos.Find(id);
            if (todo == null) return RedirectToAction("Index");
            todo.Done = !todo.Done;
            todoContext.Todos.Update(todo);
            todoContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}