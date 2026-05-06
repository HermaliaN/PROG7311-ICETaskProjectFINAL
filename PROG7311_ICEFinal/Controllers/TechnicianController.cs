using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_ICEFinal.Data;
using PROG7311_ICEFinal.Filters;
using PROG7311_ICEFinal.Helpers;

namespace PROG7311_ICEFinal.Controllers
{
    [Authorize("Technician")]
    public class TechnicianController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TechnicianController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Technician/Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            return View();
        }

        // GET: Technician/ViewBookings
        public async Task<IActionResult> ViewBookings(string? search, string? sortOrder)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sortOrder;

            var bookings = _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Technician)
                .AsQueryable();

            // Search by technician name
            if (!string.IsNullOrWhiteSpace(search))
            {
                bookings = bookings.Where(b =>
                    b.Technician != null &&
                    (b.Technician.FirstName.Contains(search) ||
                     b.Technician.LastName.Contains(search) ||
                     (b.Technician.FirstName + " " + b.Technician.LastName).Contains(search)));
            }

            // Sort by booking date
            bookings = sortOrder == "asc"
                ? bookings.OrderBy(b => b.BookingDate)
                : bookings.OrderByDescending(b => b.BookingDate);

            return View(await bookings.ToListAsync());
        }

        // GET: Technician/ViewCustomers
        public async Task<IActionResult> ViewCustomers(string? search, string? sortOrder)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sortOrder;

            var customers = _db.Customers
                .Include(c => c.Bookings)
                .Where(c => c.IsActive)
                .AsQueryable();

            // Search by customer name, email, or phone
            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = customers.Where(c =>
                    c.Name.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.PhoneNumber.Contains(search));
            }

            // Sort by name or registered date
            customers = sortOrder == "asc"
                ? customers.OrderBy(c => c.Name)
                : customers.OrderByDescending(c => c.RegisteredDate);

            return View(await customers.ToListAsync());
        }
    }
}