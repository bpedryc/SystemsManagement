using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectThesis.Migrations
{
    public partial class InitNameFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Faculty_FacultyID",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Supervisors_SupID",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_User_UserID",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Supervisors_Faculty_FacID",
                table: "Supervisors");

            migrationBuilder.DropForeignKey(
                name: "FK_Supervisors_User_UserID",
                table: "Supervisors");

            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_FieldOfStudy_FosID",
                table: "Thesis");

            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_Students_StudentID",
                table: "Thesis");

            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_Supervisors_SupID",
                table: "Thesis");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Faculty_FacultyID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_FacultyID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FacultyID",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "SupID",
                table: "Thesis",
                newName: "SupId");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Thesis",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "FosID",
                table: "Thesis",
                newName: "FosId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Thesis",
                newName: "ThesisId");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_SupID",
                table: "Thesis",
                newName: "IX_Thesis_SupId");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_StudentID",
                table: "Thesis",
                newName: "IX_Thesis_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_FosID",
                table: "Thesis",
                newName: "IX_Thesis_FosId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Supervisors",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "FacID",
                table: "Supervisors",
                newName: "FacId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Supervisors",
                newName: "SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_Supervisors_UserID",
                table: "Supervisors",
                newName: "IX_Supervisors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Supervisors_FacID",
                table: "Supervisors",
                newName: "IX_Supervisors_FacId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Students",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "SupID",
                table: "Students",
                newName: "SupId");

            migrationBuilder.RenameColumn(
                name: "FacultyID",
                table: "Students",
                newName: "FacultyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_UserID",
                table: "Students",
                newName: "IX_Students_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_SupID",
                table: "Students",
                newName: "IX_Students_SupId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_FacultyID",
                table: "Students",
                newName: "IX_Students_FacultyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FieldOfStudy",
                newName: "FieldOfStudyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Faculty",
                newName: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Faculty_FacultyId",
                table: "Students",
                column: "FacultyId",
                principalTable: "Faculty",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Supervisors_SupId",
                table: "Students",
                column: "SupId",
                principalTable: "Supervisors",
                principalColumn: "SupervisorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_User_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Supervisors_Faculty_FacId",
                table: "Supervisors",
                column: "FacId",
                principalTable: "Faculty",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Supervisors_User_UserId",
                table: "Supervisors",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_FieldOfStudy_FosId",
                table: "Thesis",
                column: "FosId",
                principalTable: "FieldOfStudy",
                principalColumn: "FieldOfStudyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_Students_StudentId",
                table: "Thesis",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_Supervisors_SupId",
                table: "Thesis",
                column: "SupId",
                principalTable: "Supervisors",
                principalColumn: "SupervisorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Faculty_FacultyId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Supervisors_SupId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_User_UserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Supervisors_Faculty_FacId",
                table: "Supervisors");

            migrationBuilder.DropForeignKey(
                name: "FK_Supervisors_User_UserId",
                table: "Supervisors");

            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_FieldOfStudy_FosId",
                table: "Thesis");

            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_Students_StudentId",
                table: "Thesis");

            migrationBuilder.DropForeignKey(
                name: "FK_Thesis_Supervisors_SupId",
                table: "Thesis");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SupId",
                table: "Thesis",
                newName: "SupID");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Thesis",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "FosId",
                table: "Thesis",
                newName: "FosID");

            migrationBuilder.RenameColumn(
                name: "ThesisId",
                table: "Thesis",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_SupId",
                table: "Thesis",
                newName: "IX_Thesis_SupID");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_StudentId",
                table: "Thesis",
                newName: "IX_Thesis_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Thesis_FosId",
                table: "Thesis",
                newName: "IX_Thesis_FosID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Supervisors",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "FacId",
                table: "Supervisors",
                newName: "FacID");

            migrationBuilder.RenameColumn(
                name: "SupervisorId",
                table: "Supervisors",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Supervisors_UserId",
                table: "Supervisors",
                newName: "IX_Supervisors_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Supervisors_FacId",
                table: "Supervisors",
                newName: "IX_Supervisors_FacID");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Students",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "SupId",
                table: "Students",
                newName: "SupID");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Students",
                newName: "FacultyID");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Students",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Students_UserId",
                table: "Students",
                newName: "IX_Students_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Students_SupId",
                table: "Students",
                newName: "IX_Students_SupID");

            migrationBuilder.RenameIndex(
                name: "IX_Students_FacultyId",
                table: "Students",
                newName: "IX_Students_FacultyID");

            migrationBuilder.RenameColumn(
                name: "FieldOfStudyId",
                table: "FieldOfStudy",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FacultyId",
                table: "Faculty",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "FacultyID",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_FacultyID",
                table: "User",
                column: "FacultyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Faculty_FacultyID",
                table: "Students",
                column: "FacultyID",
                principalTable: "Faculty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Supervisors_SupID",
                table: "Students",
                column: "SupID",
                principalTable: "Supervisors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_User_UserID",
                table: "Students",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Supervisors_Faculty_FacID",
                table: "Supervisors",
                column: "FacID",
                principalTable: "Faculty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Supervisors_User_UserID",
                table: "Supervisors",
                column: "UserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_FieldOfStudy_FosID",
                table: "Thesis",
                column: "FosID",
                principalTable: "FieldOfStudy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_Students_StudentID",
                table: "Thesis",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thesis_Supervisors_SupID",
                table: "Thesis",
                column: "SupID",
                principalTable: "Supervisors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Faculty_FacultyID",
                table: "User",
                column: "FacultyID",
                principalTable: "Faculty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
