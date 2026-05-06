using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_ICEFinal.Data;
using PROG7311_ICEFinal.Filters;
using PROG7311_ICEFinal.Helpers;
using PROG7311_ICEFinal.Models;

// Devashni Naidoo

namespace PROG7311_ICEFinal.Controllers
{
    [Authorize("Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.TotalEmployees = await _db.Employees.CountAsync();
            ViewBag.TotalCustomers = await _db.Customers.CountAsync(c => c.IsActive);
            ViewBag.TotalBookings = await _db.Bookings.CountAsync();
            ViewBag.TotalOrders = await _db.Orders.CountAsync();
            return View();
        }

        public async Task<IActionResult> Employees(string? search, string? roleFilter, string? sortOrder)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.CurrentSearch = search;
            ViewBag.RoleFilter = roleFilter;
            ViewBag.CurrentSort = sortOrder;

            var employees = _db.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                employees = employees.Where(e =>
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search) ||
                    e.Email.Contains(search) ||
                    e.Username.Contains(search));

            if (!string.IsNullOrWhiteSpace(roleFilter) && roleFilter != "All")
                employees = employees.Where(e => e.Role == roleFilter);

            employees = sortOrder == "asc"
                ? employees.OrderBy(e => e.LastName)
                : employees.OrderByDescending(e => e.HireDate);

            return View(await employees.ToListAsync());
        }

        public IActionResult AddEmployee()
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            ViewBag.UserName = AuthHelper.GetUserName();

            // Check for duplicate username
            if (await _db.Employees.AnyAsync(e => e.Username == employee.Username))
                ModelState.AddModelError("Username", "That username is already taken.");

            if (!ModelState.IsValid)
                return View(employee);

            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Employee {employee.FirstName} {employee.LastName} added successfully.";
            return RedirectToAction(nameof(Employees));
        }

        public async Task<IActionResult> EditEmployee(int id)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            var employee = await _db.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(Employee employee)
        {
            ViewBag.UserName = AuthHelper.GetUserName();

            // Check for duplicate username (exclude self)
            if (await _db.Employees.AnyAsync(e => e.Username == employee.Username && e.EmployeeId != employee.EmployeeId))
                ModelState.AddModelError("Username", "That username is already taken.");

            if (!ModelState.IsValid)
                return View(employee);

            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Employee {employee.FirstName} {employee.LastName} updated.";
            return RedirectToAction(nameof(Employees));
        }

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            var employee = await _db.Employees
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("DeleteEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
        {
            var employee = await _db.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            // Soft-delete: just deactivate so booking history is preserved
            employee.IsActive = false;
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Employee {employee.FirstName} {employee.LastName} deactivated.";
            return RedirectToAction(nameof(Employees));
        }

        public async Task<IActionResult> Customers(string? search, string? sortOrder)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSort = sortOrder;

            var customers = _db.Customers
                .Include(c => c.Bookings)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                customers = customers.Where(c =>
                    c.Name.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.PhoneNumber.Contains(search) ||
                    c.Address.Contains(search));

            customers = sortOrder == "asc"
                ? customers.OrderBy(c => c.Name)
                : customers.OrderByDescending(c => c.RegisteredDate);

            return View(await customers.ToListAsync());
        }

        public IActionResult AddCustomer()
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            ViewBag.UserName = AuthHelper.GetUserName();

            if (await _db.Customers.AnyAsync(c => c.Email == customer.Email))
                ModelState.AddModelError("Email", "A customer with that email already exists.");

            if (!ModelState.IsValid)
                return View(customer);

            customer.RegisteredDate = DateTime.Now;
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Customer {customer.Name} added.";
            return RedirectToAction(nameof(Customers));
        }

        public async Task<IActionResult> EditCustomer(int id)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(Customer customer)
        {
            ViewBag.UserName = AuthHelper.GetUserName();

            if (await _db.Customers.AnyAsync(c => c.Email == customer.Email && c.CustomerId != customer.CustomerId))
                ModelState.AddModelError("Email", "A customer with that email already exists.");

            if (!ModelState.IsValid)
                return View(customer);

            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Customer {customer.Name} updated.";
            return RedirectToAction(nameof(Customers));
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            var customer = await _db.Customers
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerConfirmed(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            customer.IsActive = false;
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Customer {customer.Name} deactivated.";
            return RedirectToAction(nameof(Customers));
        }

        public async Task<IActionResult> Bookings(string? search, string? statusFilter, string? sortOrder)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.CurrentSearch = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.CurrentSort = sortOrder;

            var bookings = _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Technician)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                bookings = bookings.Where(b =>
                    b.Customer!.Name.Contains(search) ||
                    b.ServiceType.Contains(search) ||
                    (b.Technician != null &&
                     (b.Technician.FirstName.Contains(search) ||
                      b.Technician.LastName.Contains(search))));

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "All")
                bookings = bookings.Where(b => b.Status == statusFilter);

            bookings = sortOrder == "asc"
                ? bookings.OrderBy(b => b.BookingDate)
                : bookings.OrderByDescending(b => b.BookingDate);

            return View(await bookings.ToListAsync());
        }

        public async Task<IActionResult> AddBooking()
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.Customers = await _db.Customers.Where(c => c.IsActive).ToListAsync();
            ViewBag.Technicians = await _db.Employees
                .Where(e => e.Role == "Technician" && e.IsActive)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBooking(Booking booking)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.Customers = await _db.Customers.Where(c => c.IsActive).ToListAsync();
            ViewBag.Technicians = await _db.Employees
                .Where(e => e.Role == "Technician" && e.IsActive)
                .ToListAsync();

            if (!ModelState.IsValid)
                return View(booking);

            if (booking.TechnicianId.HasValue)
                booking.AssignedDate = DateTime.Now;

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Booking #{booking.BookingId} created successfully.";
            return RedirectToAction(nameof(Bookings));
        }

        public async Task<IActionResult> EditBooking(int id)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.Customers = await _db.Customers.Where(c => c.IsActive).ToListAsync();
            ViewBag.Technicians = await _db.Employees
                .Where(e => e.Role == "Technician" && e.IsActive)
                .ToListAsync();

            var booking = await _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Technician)
                .FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBooking(Booking booking)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.Customers = await _db.Customers.Where(c => c.IsActive).ToListAsync();
            ViewBag.Technicians = await _db.Employees
                .Where(e => e.Role == "Technician" && e.IsActive)
                .ToListAsync();

            if (!ModelState.IsValid)
                return View(booking);

            // Set AssignedDate when a technician is first assigned
            if (booking.TechnicianId.HasValue && !booking.AssignedDate.HasValue)
                booking.AssignedDate = DateTime.Now;

            // Set CompletionDate when status changes to Completed
            if (booking.Status == "Completed" && !booking.CompletionDate.HasValue)
                booking.CompletionDate = DateTime.Now;

            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Booking #{booking.BookingId} updated.";
            return RedirectToAction(nameof(Bookings));
        }

        public async Task<IActionResult> DeleteBooking(int id)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            var booking = await _db.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Technician)
                .FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost, ActionName("DeleteBooking")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookingConfirmed(int id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Booking #{id} deleted.";
            return RedirectToAction(nameof(Bookings));
        }

        // ─────────────────────────────────────────────
        // ORDERS (read-only)
        // ─────────────────────────────────────────────

        public async Task<IActionResult> Orders(string? search, string? statusFilter, string? sortOrder)
        {
            ViewBag.UserName = AuthHelper.GetUserName();
            ViewBag.CurrentSearch = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.CurrentSort = sortOrder;

            var orders = _db.Orders
                .Include(o => o.Customer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                orders = orders.Where(o =>
                    o.ProductName.Contains(search) ||
                    o.Customer!.Name.Contains(search));

            if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "All")
                orders = orders.Where(o => o.OrderStatus == statusFilter);

            orders = sortOrder == "asc"
                ? orders.OrderBy(o => o.OrderDate)
                : orders.OrderByDescending(o => o.OrderDate);

            return View(await orders.ToListAsync());
        }
    }
}