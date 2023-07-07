using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academits.Karetskas.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "product",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_SupplierId",
                table: "product",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_product_Suppliers_SupplierId",
                table: "product",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_Suppliers_SupplierId",
                table: "product");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_product_SupplierId",
                table: "product");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "product");
        }
    }
}
