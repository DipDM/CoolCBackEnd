using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoolCBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignKeyConstraintd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("90d3d0cb-7cd7-4f1d-be1e-ce17604de061"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d8ae475b-a6eb-4989-98d9-8c9560bd15d4"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1a92b3d3-aac6-4881-aa61-441027b7a7ba"), null, "Admin", "ADMIN" },
                    { new Guid("d0831051-d895-4e7a-a8c2-ddbb4584c0a5"), null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1a92b3d3-aac6-4881-aa61-441027b7a7ba"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d0831051-d895-4e7a-a8c2-ddbb4584c0a5"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("90d3d0cb-7cd7-4f1d-be1e-ce17604de061"), null, "User", "USER" },
                    { new Guid("d8ae475b-a6eb-4989-98d9-8c9560bd15d4"), null, "Admin", "ADMIN" }
                });
        }
    }
}
