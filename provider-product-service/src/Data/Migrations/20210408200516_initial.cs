using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Provider.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "product");

            migrationBuilder.CreateTable(
                name: "products",
                schema: "product",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    normalized_name = table.Column<string>(nullable: true),
                    stock_count = table.Column<int>(nullable: false),
                    is_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "product",
                table: "products",
                columns: new[] { "id", "is_active", "name", "normalized_name", "stock_count" },
                values: new object[,]
                {
                    { 1, true, "Keyboard", "KEYBOARD", 40 },
                    { 2, true, "Mouse", "MOUSE", 20 },
                    { 3, true, "Monitor", "MONITOR", 50 },
                    { 4, true, "Printer", "PRINTER", 50 },
                    { 5, true, "Earphone", "EARPHONE", 20 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products",
                schema: "product");
        }
    }
}
