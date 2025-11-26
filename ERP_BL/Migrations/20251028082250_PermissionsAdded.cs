using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ERP_BL.Migrations
{
    /// <inheritdoc />
    public partial class PermissionsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "Description", "IsActive", "IsVoid", "LastModified", "LastModifiedBy", "Name", "ParentPermissionId" },
                values: new object[,]
                {
                    { 3100, "System", new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leaves Related HRM", true, false, new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "HRM", null },
                    { 3101, "System", new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Access to Employment Centre", true, false, new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Access Employment Centre", 3100 },
                    { 3102, "System", new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Permission to add a new employee", true, false, new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Add New Employee", 3100 },
                    { 3103, "System", new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Permission to view employee register", true, false, new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "View Employee Register", 3100 },
                    { 3104, "System", new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Permission to view employee details", true, false, new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "View Employee", 3100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3101);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3102);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3103);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3104);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3100);
        }
    }
}
