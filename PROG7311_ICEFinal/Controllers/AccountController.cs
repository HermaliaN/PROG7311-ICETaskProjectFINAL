using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_ICEFinal.Data;
using PROG7311_ICEFinal.Helpers;
using PROG7311_ICEFinal.Models;

//Hermalia Naidoo

namespace PROG7311_ICEFinal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var employee = await _db.Employees
                .FirstOrDefaultAsync(e => e.Username == model.Username && e.Password == model.Password);

            if (employee != null && employee.IsActive)
            {
                AuthHelper.Login(employee.EmployeeId.ToString(), employee.Role, employee.FirstName);

                // Redirect to role-specific Dashboard
                switch (employee.Role)
                {
                    case "Admin":
                        return RedirectToAction("Dashboard", "Admin");
                    case "Technician":
                        return RedirectToAction("Dashboard", "Technician");
                    case "Sales":
                        return RedirectToAction("Dashboard", "Sales");
                    default:
                        return RedirectToAction("Login", "Account");
                }
            }

            ViewBag.Error = "Invalid login";
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            AuthHelper.Logout();
            return RedirectToAction("Login");
        }
    }
}