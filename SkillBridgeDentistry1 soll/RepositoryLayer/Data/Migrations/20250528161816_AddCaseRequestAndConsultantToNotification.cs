using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Data.Migrations
{
    public partial class AddCaseRequestAndConsultantToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaseConsultantId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaseRequestId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CaseConsultantId",
                table: "Notifications",
                column: "CaseConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CaseRequestId",
                table: "Notifications",
                column: "CaseRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_CaseConsultants_CaseConsultantId",
                table: "Notifications",
                column: "CaseConsultantId",
                principalTable: "CaseConsultants",
                principalColumn: "CaseConsultantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_CaseRequests_CaseRequestId",
                table: "Notifications",
                column: "CaseRequestId",
                principalTable: "CaseRequests",
                principalColumn: "CaseRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_CaseConsultants_CaseConsultantId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_CaseRequests_CaseRequestId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CaseConsultantId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CaseRequestId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CaseConsultantId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CaseRequestId",
                table: "Notifications");
        }
    }
}
