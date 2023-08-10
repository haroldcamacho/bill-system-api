using Microsoft.EntityFrameworkCore.Migrations;
using System;
using BasicBilling.API.Models.Enums;

#nullable disable

namespace BasicBilling.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedBills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bills",
                columns: new[] { "Id", "ClientId", "Category", "MonthYear", "State", "Amount" },
                values: new object[,]
                {
                    { 1, 100, "WATER", new DateTime(2023, 6, 1), (int)BillState.Pending, 50.0m },
                    { 2, 100, "ELECTRICITY", new DateTime(2023, 6, 1), (int)BillState.Paid, 75.0m },
                    { 3, 100, "INTERNET", new DateTime(2023, 7, 1), (int)BillState.Pending, 60.0m },
                    { 4, 200, "GAS", new DateTime(2023, 6, 1), (int)BillState.Paid, 45.0m },
                    { 5, 200, "WATER", new DateTime(2023, 7, 1), (int)BillState.Pending, 55.0m },
                    { 6, 300, "ELECTRICITY", new DateTime(2023, 7, 1), (int)BillState.Pending, 70.0m },
                    { 7, 300, "SEWER", new DateTime(2023, 6, 1), (int)BillState.Paid, 35.0m },
                    { 8, 400, "INTERNET", new DateTime(2023, 6, 1), (int)BillState.Pending, 65.0m },
                    { 9, 400, "WATER", new DateTime(2023, 7, 1), (int)BillState.Paid, 60.0m },
                    { 10, 500, "ELECTRICITY", new DateTime(2023, 6, 1), (int)BillState.Pending, 80.0m }, 
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
                  migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 6);
            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 7);
            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 8);
            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 9);
            migrationBuilder.DeleteData(
                table: "Bills",
                keyColumn: "Id",
                keyValue: 10);                                
        }
    }
}
