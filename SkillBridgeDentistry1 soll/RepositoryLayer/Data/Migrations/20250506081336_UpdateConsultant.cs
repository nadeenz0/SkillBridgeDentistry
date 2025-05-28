using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Data.Migrations
{
    public partial class UpdateConsultant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultants_Specialities_SpecialityId",
                table: "Consultants");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialityId",
                table: "Consultants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Consultants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultants_Specialities_SpecialityId",
                table: "Consultants",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "SpecialityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultants_Specialities_SpecialityId",
                table: "Consultants");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Consultants");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialityId",
                table: "Consultants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultants_Specialities_SpecialityId",
                table: "Consultants",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "SpecialityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
