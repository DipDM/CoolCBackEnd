using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoolCBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e097043-7994-42e7-8f31-3138c32ca5cc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("efbc3d6f-53b6-4622-9476-4b9ecd2aa5be"));

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("d8bee4cf-17b6-451d-aa96-423a0a2d5dd1"), null, "Admin", "ADMIN" },
                    { new Guid("ff5d5bb9-4f95-44cf-8897-d36828dbf4cf"), null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d8bee4cf-17b6-451d-aa96-423a0a2d5dd1"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ff5d5bb9-4f95-44cf-8897-d36828dbf4cf"));

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Comments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7e097043-7994-42e7-8f31-3138c32ca5cc"), null, "User", "USER" },
                    { new Guid("efbc3d6f-53b6-4622-9476-4b9ecd2aa5be"), null, "Admin", "ADMIN" }
                });
        }
    }
}
