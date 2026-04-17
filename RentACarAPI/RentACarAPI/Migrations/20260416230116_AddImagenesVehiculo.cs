using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenesVehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Vehiculos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagenesExtra",
                table: "Vehiculos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Vehiculos");

            migrationBuilder.DropColumn(
                name: "ImagenesExtra",
                table: "Vehiculos");
        }
    }
}
