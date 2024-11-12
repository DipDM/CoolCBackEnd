using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoolCBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d8bee4cf-17b6-451d-aa96-423a0a2d5dd1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ff5d5bb9-4f95-44cf-8897-d36828dbf4cf"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2825242d-c3ed-451d-af79-18bd5c2e67ee"), null, "Admin", "ADMIN" },
                    { new Guid("fca7a59b-37f5-4723-9162-b881edcd9b1c"), null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2825242d-c3ed-451d-af79-18bd5c2e67ee"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("fca7a59b-37f5-4723-9162-b881edcd9b1c"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("d8bee4cf-17b6-451d-aa96-423a0a2d5dd1"), null, "Admin", "ADMIN" },
                    { new Guid("ff5d5bb9-4f95-44cf-8897-d36828dbf4cf"), null, "User", "USER" }
                });
        }
    }
}
