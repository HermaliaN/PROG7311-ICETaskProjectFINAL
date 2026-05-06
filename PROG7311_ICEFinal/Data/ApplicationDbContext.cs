using Microsoft.EntityFrameworkCore;
using PROG7311_ICEFinal.Models;

// Jarrud Cochrane

namespace PROG7311_ICEFinal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Devashni",
                    LastName = "Naidoo",
                    Email = "admin@pcsociety.com",
                    PhoneNumber = "1111111111",
                    Role = "Admin",
                    Username = "admin",
                    Password = "admin123",
                    IsActive = true,
                    HireDate = DateTime.Now.AddMonths(-12)
                },
                new Employee
                {
                    EmployeeId = 2,
                    FirstName = "Jenna",
                    LastName = "Martin",
                    Email = "john.tech@pcsociety.com",
                    PhoneNumber = "2222222222",
                    Role = "Technician",
                    Username = "technician",
                    Password = "tech123",
                    IsActive = true,
                    HireDate = DateTime.Now.AddMonths(-6)
                },
                new Employee
                {
                    EmployeeId = 3,
                    FirstName = "Priyanka",
                    LastName = "Govender",
                    Email = "sarah.sales@pcsociety.com",
                    PhoneNumber = "3333333333",
                    Role = "Sales",
                    Username = "sales",
                    Password = "sales123",
                    IsActive = true,
                    HireDate = DateTime.Now.AddMonths(-3)
                }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    Name = "Jarrud Cochrane",
                    PhoneNumber = "5551112222",
                    Email = "jarrud@gmail.com",
                    Address = "123 Main St, Johannesburg",
                    RegisteredDate = DateTime.Now.AddMonths(-2),
                    IsActive = true
                },
                new Customer
                {
                    CustomerId = 2,
                    Name = "Hermalia Naidoo",
                    PhoneNumber = "5553334444",
                    Email = "hermalia@gmail.com",
                    Address = "456 Oak Ave, Cape Town",
                    RegisteredDate = DateTime.Now.AddMonths(-1),
                    IsActive = true
                },
                new Customer
                {
                    CustomerId = 3,
                    Name = "Denzyl Govender",
                    PhoneNumber = "5555556666",
                    Email = "denzyl@gmail.com",
                    Address = "789 Pine Rd, Durban",
                    RegisteredDate = DateTime.Now.AddDays(-15),
                    IsActive = true
                }
            );

            // Seed Orders (linked to customers)
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    OrderId = 1,
                    CustomerId = 1,
                    ProductName = "Gaming PC",
                    OrderDate = DateTime.Now.AddDays(-10),
                    Amount = 1500.00m,
                    OrderStatus = "Received",
                    Quantity = 1
                },
                new Order
                {
                    OrderId = 2,
                    CustomerId = 2,
                    ProductName = "Laptop Repair Kit",
                    OrderDate = DateTime.Now.AddDays(-5),
                    Amount = 89.99m,
                    OrderStatus = "Processing",
                    Quantity = 2
                },
                new Order
                {
                    OrderId = 3,
                    CustomerId = 1,
                    ProductName = "Monitor",
                    OrderDate = DateTime.Now.AddDays(-3),
                    Amount = 299.99m,
                    OrderStatus = "Dispatched",
                    Quantity = 1
                }
            );

            // Seed Bookings (linked to customers and technicians)
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,
                    CustomerId = 1,
                    ServiceType = "Virus Removal",
                    BookingDate = DateTime.Now.AddDays(-7),
                    TechnicianId = 2,
                    Status = "Completed",
                    Description = "Computer running slow, multiple popups",
                    AssignedDate = DateTime.Now.AddDays(-7),
                    CompletionDate = DateTime.Now.AddDays(-6)
                },
                new Booking
                {
                    BookingId = 2,
                    CustomerId = 2,
                    ServiceType = "Hardware Repair",
                    BookingDate = DateTime.Now.AddDays(-3),
                    TechnicianId = 2,
                    Status = "In Progress",
                    Description = "Laptop screen cracked",
                    AssignedDate = DateTime.Now.AddDays(-3),
                    CompletionDate = null
                },
                new Booking
                {
                    BookingId = 3,
                    CustomerId = 3,
                    ServiceType = "Data Recovery",
                    BookingDate = DateTime.Now.AddDays(-1),
                    TechnicianId = null,
                    Status = "Pending",
                    Description = "External hard drive not recognized",
                    AssignedDate = null,
                    CompletionDate = null
                }
            );
        }
    }
}