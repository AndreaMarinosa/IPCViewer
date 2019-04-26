using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IPCViewer.Api.Migrations
{
    public partial class ModifyUsersAndCameras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cameras",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Cameras",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_CityId",
                table: "Cameras",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_City_CityId",
                table: "Cameras",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_City_CityId",
                table: "Cameras");

            migrationBuilder.DropIndex(
                name: "IX_Cameras_CityId",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Cameras");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Cameras",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
