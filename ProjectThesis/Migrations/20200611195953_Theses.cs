using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectThesis.Migrations
{
    public partial class Theses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_Specials_SpecId",
                table: "Thesis");

            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_Studs_StudentId",
                table: "Thesis");

            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_Supers_SuperId",
                table: "Thesis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Thesis",
                table: "Thesis");

            migrationBuilder.RenameTable(
                name: "Thesis",
                newName: "Theses");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_SuperId",
                table: "Theses",
                newName: "IX_Theses_SuperId");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_StudentId",
                table: "Theses",
                newName: "IX_Theses_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_SpecId",
                table: "Theses",
                newName: "IX_Theses_SpecId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Theses",
                table: "Theses",
                column: "ThesisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Theses_Specials_SpecId",
                table: "Theses",
                column: "SpecId",
                principalTable: "Specials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Theses_Studs_StudentId",
                table: "Theses",
                column: "StudentId",
                principalTable: "Studs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Theses_Supers_SuperId",
                table: "Theses",
                column: "SuperId",
                principalTable: "Supers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Theses_Specials_SpecId",
                table: "Theses");

            migrationBuilder.DropForeignKey(
                name: "FK_Theses_Studs_StudentId",
                table: "Theses");

            migrationBuilder.DropForeignKey(
                name: "FK_Theses_Supers_SuperId",
                table: "Theses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Theses",
                table: "Theses");

            migrationBuilder.RenameTable(
                name: "Theses",
                newName: "Thesis");

            migrationBuilder.RenameIndex(
                name: "IX_Theses_SuperId",
                table: "Thesis",
                newName: "IX_Thesis_SuperId");

            migrationBuilder.RenameIndex(
                name: "IX_Theses_StudentId",
                table: "Thesis",
                newName: "IX_Thesis_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Theses_SpecId",
                table: "Thesis",
                newName: "IX_Thesis_SpecId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Thesis",
                table: "Thesis",
                column: "ThesisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_Specials_SpecId",
                table: "Thesis",
                column: "SpecId",
                principalTable: "Specials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_Studs_StudentId",
                table: "Thesis",
                column: "StudentId",
                principalTable: "Studs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_Supers_SuperId",
                table: "Thesis",
                column: "SuperId",
                principalTable: "Supers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
