using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorService.Migrations
{
    /// <inheritdoc />
    public partial class ChangestatusdataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AvailabilitySlots");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "AvailabilitySlots",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "AvailabilitySlots");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AvailabilitySlots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
