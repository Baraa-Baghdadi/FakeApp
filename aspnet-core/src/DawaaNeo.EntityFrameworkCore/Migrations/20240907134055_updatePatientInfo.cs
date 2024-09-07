using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DawaaNeo.Migrations
{
    /// <inheritdoc />
    public partial class updatePatientInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "AppPatients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppPatients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dob",
                table: "AppPatients");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppPatients");
        }
    }
}
