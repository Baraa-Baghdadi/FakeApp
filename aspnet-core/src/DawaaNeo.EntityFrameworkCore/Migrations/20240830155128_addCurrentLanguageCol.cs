using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DawaaNeo.Migrations
{
    /// <inheritdoc />
    public partial class addCurrentLanguageCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentLanguage",
                table: "AppPatients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLanguage",
                table: "AppPatients");
        }
    }
}
