using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoService.Migrations
{
    /// <inheritdoc />
    public partial class VideoConsultation_BD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoRooms",
                columns: table => new
                {
                    VideoRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalRoomUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsWaitingRoomEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoRooms", x => x.VideoRoomId);
                });

            migrationBuilder.CreateTable(
                name: "WaitingRoomStatuses",
                columns: table => new
                {
                    WaitingRoomStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitingRoomStatuses", x => x.WaitingRoomStatusId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoRooms");

            migrationBuilder.DropTable(
                name: "WaitingRoomStatuses");
        }
    }
}
