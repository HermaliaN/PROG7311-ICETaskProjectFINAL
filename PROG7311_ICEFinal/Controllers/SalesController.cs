using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_ICEFinal.Data;
using PROG7311_ICEFinal.Filters;
using PROG7311_ICEFinal.Helpers;

// Priyanka Govender

namespace PROG7311_ICEFinal.Controllers
{
    [Authorize("Sales")]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SalesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Sales/Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            return View();
        }

        // GET: Sales/ViewOrders
        public async Task<IActionResult> ViewOrders(string? search, string? statusFilter, string? sortOrder)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentStatus = statusFilter;
            ViewBag.CurrentSort = sortOrder;

            var orders = _db.Orders.Include(o => o.Customer).AsQueryable();

            // Filter by search (product name or customer name)
            if (!string.IsNullOrWhiteSpace(search))
            {
                orders = orders.Where(o =>
                    o.ProductName.Contains(search) ||
                    o.Customer!.Name.Contains(search));
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "All")
            {
                orders = orders.Where(o => o.OrderStatus == statusFilter);
            }

            // Sort by date
            orders = sortOrder == "asc"
                ? orders.OrderBy(o => o.OrderDate)
                : orders.OrderByDescending(o => o.OrderDate);

            return View(await orders.ToListAsync());
        }

        // POST: Sales/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            var validStatuses = new[] { "Received", "Processing", "Dispatched" };
            if (!validStatuses.Contains(newStatus)) return BadRequest();

            order.OrderStatus = newStatus;
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Order #{orderId} status updated to {newStatus}.";
            return RedirectToAction("ViewOrders");
        }

        // GET: Sales/ViewCustomers
        public async Task<IActionResult> ViewCustomers(string? search, string? sortOrder)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sortOrder;

            var customers = _db.Customers
                .Include(c => c.Orders)
                .Where(c => c.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = customers.Where(c =>
                    c.Name.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.PhoneNumber.Contains(search));
            }

            customers = sortOrder == "asc"
                ? customers.OrderBy(c => c.Name)
                : customers.OrderByDescending(c => c.RegisteredDate);

            return View(await customers.ToListAsync());
        }
    }
}