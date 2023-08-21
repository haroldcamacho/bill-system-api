using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicBilling.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 100, "Joseph Carlton" },
                    { 200, "Maria Juarez" },
                    { 300, "Albert Kenny" },
                    { 400, "Jessica Phillips" },
                    { 500, "Charles Johnson" }
                });
        }
 
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 300);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 400);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 500);
        }
    }
}
