using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Data.Migrations
{
    public partial class UpdateConsultants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultants_Specialities_SpecialityId",
                table: "Consultants");

            migrationBuilder.DropIndex(
                name: "IX_Consultants_SpecialityId",
                table: "Consultants");

            migrationBuilder.DropColumn(
                name: "SpecialityId",
                table: "Consultants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpecialityId",
                table: "Consultants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultants_SpecialityId",
                table: "Consultants",
                column: "SpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultants_Specialities_SpecialityId",
                table: "Consultants",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "SpecialityId");
        }
    }
}
