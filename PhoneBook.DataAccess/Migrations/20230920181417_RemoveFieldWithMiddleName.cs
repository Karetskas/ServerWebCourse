using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academits.Karetskas.PhoneBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldWithMiddleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "Contacts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "Contacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
