using Microsoft.EntityFrameworkCore.Migrations;
using Oracle.EntityFrameworkCore.Metadata;

namespace ProjectThesis.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    FacId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specials_Faculties_FacId",
                        column: x => x.FacId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    StudentLimit = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    FacultyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supers_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Supers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Studs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    StudentNo = table.Column<int>(nullable: false),
                    DegreeCycle = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    FacultyId = table.Column<int>(nullable: false),
                    SuperId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Studs_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Studs_Supers_SuperId",
                        column: x => x.SuperId,
                        principalTable: "Supers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Studs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Thesis",
                columns: table => new
                {
                    ThesisId = table.Column<int>(nullable: false)
                        .Annotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn),
                    Subject = table.Column<string>(nullable: true),
                    DegreeCycle = table.Column<int>(nullable: false),
                    SpecId = table.Column<int>(nullable: false),
                    SuperId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thesis", x => x.ThesisId);
                    table.ForeignKey(
                        name: "FK_Thesis_Specials_SpecId",
                        column: x => x.SpecId,
                        principalTable: "Specials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Thesis_Studs_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Thesis_Supers_SuperId",
                        column: x => x.SuperId,
                        principalTable: "Supers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specials_FacId",
                table: "Specials",
                column: "FacId");

            migrationBuilder.CreateIndex(
                name: "IX_Studs_FacultyId",
                table: "Studs",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Studs_SuperId",
                table: "Studs",
                column: "SuperId");

            migrationBuilder.CreateIndex(
                name: "IX_Studs_UserId",
                table: "Studs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Supers_FacultyId",
                table: "Supers",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Supers_UserId",
                table: "Supers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Thesis_SpecId",
                table: "Thesis",
                column: "SpecId");

            migrationBuilder.CreateIndex(
                name: "IX_Thesis_StudentId",
                table: "Thesis",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Thesis_SuperId",
                table: "Thesis",
                column: "SuperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Thesis");

            migrationBuilder.DropTable(
                name: "Specials");

            migrationBuilder.DropTable(
                name: "Studs");

            migrationBuilder.DropTable(
                name: "Supers");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
