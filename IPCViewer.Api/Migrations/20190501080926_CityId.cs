using Microsoft.EntityFrameworkCore.Migrations;

namespace IPCViewer.Api.Migrations
{
    public partial class CityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Cameras",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Cameras",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
