using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace ProjectThesis.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldOfStudy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOfStudy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    FacultyID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Faculty_FacultyID",
                        column: x => x.FacultyID,
                        principalTable: "Faculty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    StudentLimit = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    FacID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supervisors_Faculty_FacID",
                        column: x => x.FacID,
                        principalTable: "Faculty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Supervisors_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    StudentNo = table.Column<int>(nullable: false),
                    DegreeCycle = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    FacultyID = table.Column<int>(nullable: false),
                    SupID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Faculty_FacultyID",
                        column: x => x.FacultyID,
                        principalTable: "Faculty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Supervisors_SupID",
                        column: x => x.SupID,
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Thesis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(nullable: true),
                    DegreeCycle = table.Column<int>(nullable: false),
                    FosID = table.Column<int>(nullable: false),
                    SupID = table.Column<int>(nullable: false),
                    StudentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thesis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Thesis_FieldOfStudy_FosID",
                        column: x => x.FosID,
                        principalTable: "FieldOfStudy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Thesis_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Thesis_Supervisors_SupID",
                        column: x => x.SupID,
                        principalTable: "Supervisors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_FacultyID",
                table: "Students",
                column: "FacultyID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SupID",
                table: "Students",
                column: "SupID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserID",
                table: "Students",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_FacID",
                table: "Supervisors",
                column: "FacID");

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_UserID",
                table: "Supervisors",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Thesis_FosID",
                table: "Thesis",
                column: "FosID");

            migrationBuilder.CreateIndex(
                name: "IX_Thesis_StudentID",
                table: "Thesis",
                column: "StudentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Thesis_SupID",
                table: "Thesis",
                column: "SupID");

            migrationBuilder.CreateIndex(
                name: "IX_User_FacultyID",
                table: "User",
                column: "FacultyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Thesis");

            migrationBuilder.DropTable(
                name: "FieldOfStudy");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Faculty");
        }
    }
}
