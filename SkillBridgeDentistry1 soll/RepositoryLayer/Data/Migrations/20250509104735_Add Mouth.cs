using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Data.Migrations
{
    public partial class AddMouth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 1,
                column: "Name",
                value: "Mouth Ulcers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 1,
                column: "Name",
                value: "Mouth Ulcer");
        }
    }
}
