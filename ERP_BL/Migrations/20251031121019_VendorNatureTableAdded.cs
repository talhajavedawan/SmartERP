using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_BL.Migrations
{
    /// <inheritdoc />
    public partial class VendorNatureTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NTN",
                table: "Vendors");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Vendors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VendorNatureId",
                table: "Vendors",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Manager",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currencies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorNatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ISActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    LastModifiedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorNatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorNatures_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VendorNatures_AspNetUsers_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CurrencyId",
                table: "Vendors",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_VendorNatureId",
                table: "Vendors",
                column: "VendorNatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CountryId",
                table: "Currencies",
                column: "CountryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Name",
                table: "Currencies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorNatures_CreatedById",
                table: "VendorNatures",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_VendorNatures_LastModifiedById",
                table: "VendorNatures",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_VendorNatures_Name",
                table: "VendorNatures",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Currencies_CurrencyId",
                table: "Vendors",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_VendorNatures_VendorNatureId",
                table: "Vendors",
                column: "VendorNatureId",
                principalTable: "VendorNatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Currencies_CurrencyId",
                table: "Vendors");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_VendorNatures_VendorNatureId",
                table: "Vendors");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "VendorNatures");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_CurrencyId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_VendorNatureId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorNatureId",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "NTN",
                table: "Vendors",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Manager",
                table: "Employees",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
