using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Data.Migrations
{
    public partial class AddMouthUlcer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 1,
                column: "Name",
                value: "Mouth Ulcer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 1,
                column: "Name",
                value: "Mouth Ulcers");
        }
    }
}
