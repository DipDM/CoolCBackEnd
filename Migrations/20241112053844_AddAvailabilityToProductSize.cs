using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoolCBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddAvailabilityToProductSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2825242d-c3ed-451d-af79-18bd5c2e67ee"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fca7a59b-37f5-4723-9162-b881edcd9b1c"));

            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "ProductSizes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("161bedd3-687d-47d3-a5bf-ea208d120e38"), null, "User", "USER" },
                    { new Guid("eb83553d-442c-4039-8750-7002c0496aa2"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("161bedd3-687d-47d3-a5bf-ea208d120e38"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("eb83553d-442c-4039-8750-7002c0496aa2"));

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "ProductSizes");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2825242d-c3ed-451d-af79-18bd5c2e67ee"), null, "Admin", "ADMIN" },
                    { new Guid("fca7a59b-37f5-4723-9162-b881edcd9b1c"), null, "User", "USER" }
                });
        }
    }
}
