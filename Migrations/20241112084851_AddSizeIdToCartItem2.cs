using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoolCBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeIdToCartItem2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { new Guid("4d46f249-51a3-481e-b572-44fa92d3a09c"), null, "User", "USER" },
                    { new Guid("73bf2ebf-e749-4ebc-b685-ca3c99680ddd"), null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4d46f249-51a3-481e-b572-44fa92d3a09c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("73bf2ebf-e749-4ebc-b685-ca3c99680ddd"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4e3fa905-4c80-4898-92f0-4573daa104ec"), null, "User", "USER" },
                    { new Guid("9b5bc739-a2ef-40a5-aac2-6b788c4a8e93"), null, "Admin", "ADMIN" }
                });
        }
    }
}
