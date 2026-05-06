using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PROG7311_ICEFinal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TechnicianId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Employees_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "Email", "IsActive", "Name", "PhoneNumber", "RegisteredDate" },
                values: new object[,]
                {
                    { 1, "123 Main St, Johannesburg", "jarrud@gmail.com", true, "Jarrud Cochrane", "5551112222", new DateTime(2026, 3, 6, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8632) },
                    { 2, "456 Oak Ave, Cape Town", "hermalia@gmail.com", true, "Hermalia Naidoo", "5553334444", new DateTime(2026, 4, 6, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8634) },
                    { 3, "789 Pine Rd, Durban", "denzyl@gmail.com", true, "Denzyl Govender", "5555556666", new DateTime(2026, 4, 21, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8637) }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Email", "FirstName", "HireDate", "IsActive", "LastName", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin@pcsociety.com", "Devashni", new DateTime(2025, 5, 6, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8508), true, "Naidoo", "admin123", "1111111111", "Admin", "admin" },
                    { 2, "john.tech@pcsociety.com", "Jenna", new DateTime(2025, 11, 6, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8522), true, "Martin", "tech123", "2222222222", "Technician", "technician" },
                    { 3, "sarah.sales@pcsociety.com", "Priyanka", new DateTime(2026, 2, 6, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8525), true, "Govender", "sales123", "3333333333", "Sales", "sales" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "AssignedDate", "BookingDate", "CompletionDate", "CustomerId", "Description", "ServiceType", "Status", "TechnicianId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 29, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8684), new DateTime(2026, 4, 29, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8682), new DateTime(2026, 4, 30, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8687), 1, "Computer running slow, multiple popups", "Virus Removal", "Completed", 2 },
                    { 2, new DateTime(2026, 5, 3, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8690), new DateTime(2026, 5, 3, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8689), null, 2, "Laptop screen cracked", "Hardware Repair", "In Progress", 2 },
                    { 3, null, new DateTime(2026, 5, 5, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8691), null, 3, "External hard drive not recognized", "Data Recovery", "Pending", null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "Amount", "CustomerId", "OrderDate", "OrderStatus", "ProductName", "Quantity" },
                values: new object[,]
                {
                    { 1, 1500.00m, 1, new DateTime(2026, 4, 26, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8659), "Received", "Gaming PC", 1 },
                    { 2, 89.99m, 2, new DateTime(2026, 5, 1, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8664), "Processing", "Laptop Repair Kit", 2 },
                    { 3, 299.99m, 1, new DateTime(2026, 5, 3, 8, 6, 4, 470, DateTimeKind.Local).AddTicks(8666), "Dispatched", "Monitor", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TechnicianId",
                table: "Bookings",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
