using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectThesis.Migrations
{
    public partial class ThesisStudentOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Theses\" MODIFY (\"StudentId\" NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"Theses\" MODIFY (\"StudentId\" NOT NULL)");
        }
    }
}
