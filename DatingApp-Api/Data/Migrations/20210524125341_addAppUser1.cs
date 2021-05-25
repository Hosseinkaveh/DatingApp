using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp_Api.Data.Migrations
{
    public partial class addAppUser1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_AppUsers_AppUserId",
                table: "Photo");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Photo",
                newName: "AppUserid");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_AppUserId",
                table: "Photo",
                newName: "IX_Photo_AppUserid");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserid",
                table: "Photo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_AppUsers_AppUserid",
                table: "Photo",
                column: "AppUserid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_AppUsers_AppUserid",
                table: "Photo");

            migrationBuilder.RenameColumn(
                name: "AppUserid",
                table: "Photo",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Photo_AppUserid",
                table: "Photo",
                newName: "IX_Photo_AppUserId");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Photo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_AppUsers_AppUserId",
                table: "Photo",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
