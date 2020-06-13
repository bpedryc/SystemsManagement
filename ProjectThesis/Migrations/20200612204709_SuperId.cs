using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectThesis.Migrations
{
    public partial class SuperId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studs_Supers_SuperId",
                table: "Studs");

            migrationBuilder.DropForeignKey(
                name: "FK_Theses_Studs_StudentId",
                table: "Theses");

            migrationBuilder.RenameColumn(
                name: "ThesisId",
                table: "Theses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SuperId",
                table: "Studs",
                newName: "SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_Studs_SuperId",
                table: "Studs",
                newName: "IX_Studs_SupervisorId");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Theses",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Studs_Supers_SupervisorId",
                table: "Studs",
                column: "SupervisorId",
                principalTable: "Supers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Theses_Studs_StudentId",
                table: "Theses",
                column: "StudentId",
                principalTable: "Studs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Studs_Supers_SupervisorId",
                table: "Studs");

            migrationBuilder.DropForeignKey(
                name: "FK_Theses_Studs_StudentId",
                table: "Theses");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Theses",
                newName: "ThesisId");

            migrationBuilder.RenameColumn(
                name: "SupervisorId",
                table: "Studs",
                newName: "SuperId");

            migrationBuilder.RenameIndex(
                name: "IX_Studs_SupervisorId",
                table: "Studs",
                newName: "IX_Studs_SuperId");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Theses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Studs_Supers_SuperId",
                table: "Studs",
                column: "SuperId",
                principalTable: "Supers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Theses_Studs_StudentId",
                table: "Theses",
                column: "StudentId",
                principalTable: "Studs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
