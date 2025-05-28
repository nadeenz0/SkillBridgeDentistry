using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Data.Migrations
{
    public partial class Disease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "CaseRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Case",
                columns: table => new
                {
                    CaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Disease = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Case", x => x.CaseId);
                    table.ForeignKey(
                        name: "FK_Case_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalTable: "Specialities",
                        principalColumn: "SpecialityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseRequests_CaseId",
                table: "CaseRequests",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Case_SpecialityId",
                table: "Case",
                column: "SpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseRequests_Case_CaseId",
                table: "CaseRequests",
                column: "CaseId",
                principalTable: "Case",
                principalColumn: "CaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseRequests_Case_CaseId",
                table: "CaseRequests");

            migrationBuilder.DropTable(
                name: "Case");

            migrationBuilder.DropIndex(
                name: "IX_CaseRequests_CaseId",
                table: "CaseRequests");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "CaseRequests");
        }
    }
}
