using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoolCBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeIdToCartItem1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("30be31b0-2e48-43fa-87bb-239581e7b895"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee3cfc7d-cbad-4cb3-8075-e26f790eb70c"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4e3fa905-4c80-4898-92f0-4573daa104ec"), null, "User", "USER" },
                    { new Guid("9b5bc739-a2ef-40a5-aac2-6b788c4a8e93"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4e3fa905-4c80-4898-92f0-4573daa104ec"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9b5bc739-a2ef-40a5-aac2-6b788c4a8e93"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("30be31b0-2e48-43fa-87bb-239581e7b895"), null, "User", "USER" },
                    { new Guid("ee3cfc7d-cbad-4cb3-8075-e26f790eb70c"), null, "Admin", "ADMIN" }
                });
        }
    }
}
