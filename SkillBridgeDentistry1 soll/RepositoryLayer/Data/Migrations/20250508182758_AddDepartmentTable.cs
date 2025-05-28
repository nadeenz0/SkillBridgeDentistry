using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Data.Migrations
{
    public partial class AddDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Consultants");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Specialities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Consultants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 1, "Periodontics" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 2, "Operative Dentistry" });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentId", "Name" },
                values: new object[] { 3, "Pathology Dentistry" });

            migrationBuilder.InsertData(
                table: "Specialities",
                columns: new[] { "SpecialityId", "DepartmentId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Mouth Ulcer" },
                    { 2, 2, "Dental Caries" },
                    { 3, 2, "Hypodontia" },
                    { 4, 2, "Tooth Discoloration" },
                    { 5, 3, "Dental Malignant tumors" },
                    { 6, 3, "Dental benign tumors" },
                    { 7, 1, "Gingivitis" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_DepartmentId",
                table: "Specialities",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultants_DepartmentId",
                table: "Consultants",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultants_Department_DepartmentId",
                table: "Consultants",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialities_Department_DepartmentId",
                table: "Specialities",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultants_Department_DepartmentId",
                table: "Consultants");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialities_Department_DepartmentId",
                table: "Specialities");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Specialities_DepartmentId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Consultants_DepartmentId",
                table: "Consultants");

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Specialities",
                keyColumn: "SpecialityId",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Consultants");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Consultants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
