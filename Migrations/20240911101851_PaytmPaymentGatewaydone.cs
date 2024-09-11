using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoolCBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class PaytmPaymentGatewaydone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1378b6d9-c816-445b-8d32-ebd3a31572e2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("53c1960f-ab65-4d06-8dcb-a8ec184a32a2"));

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId1",
                        column: x => x.OrderId1,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2a65db2d-0da5-424f-97bc-b36155409fbc"), null, "User", "USER" },
                    { new Guid("abc6f0c2-7600-4714-8219-b14887364bc4"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId1",
                table: "Payments",
                column: "OrderId1",
                unique: true,
                filter: "[OrderId1] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2a65db2d-0da5-424f-97bc-b36155409fbc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("abc6f0c2-7600-4714-8219-b14887364bc4"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1378b6d9-c816-445b-8d32-ebd3a31572e2"), null, "User", "USER" },
                    { new Guid("53c1960f-ab65-4d06-8dcb-a8ec184a32a2"), null, "Admin", "ADMIN" }
                });
        }
    }
}
