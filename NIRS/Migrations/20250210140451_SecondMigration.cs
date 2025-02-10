using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIRS.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeviceName",
                table: "Rate",
                newName: "DeviceType");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Device",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Device");

            migrationBuilder.RenameColumn(
                name: "DeviceType",
                table: "Rate",
                newName: "DeviceName");
        }
    }
}
