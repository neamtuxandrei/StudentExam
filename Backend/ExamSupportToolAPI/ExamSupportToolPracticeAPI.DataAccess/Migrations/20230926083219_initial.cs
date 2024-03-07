using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSupportToolAPI.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommitteeMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentExaminationSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PresentationSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExaminationSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BreakStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BreakDuration = table.Column<int>(type: "int", nullable: false),
                    StudentPresentationDuration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecretaryMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecretaryMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnonymizationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearsAverageGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiplomaProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentExaminationSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentPresentationScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExaminationSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecretaryMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PresentationScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HeadOfCommitteeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExaminationSessions_PresentationSchedules_PresentationScheduleId",
                        column: x => x.PresentationScheduleId,
                        principalTable: "PresentationSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExaminationSessions_SecretaryMembers_SecretaryMemberId",
                        column: x => x.SecretaryMemberId,
                        principalTable: "SecretaryMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PresentationScheduleEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PresentationScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentationScheduleEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentationScheduleEntries_PresentationSchedules_PresentationScheduleId",
                        column: x => x.PresentationScheduleId,
                        principalTable: "PresentationSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentationScheduleEntries_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommitteeExaminationSessions",
                columns: table => new
                {
                    ExaminationSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommitteeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeExaminationSessions", x => new { x.CommitteeMemberId, x.ExaminationSessionId });
                    table.ForeignKey(
                        name: "FK_CommitteeExaminationSessions_CommitteeMembers_CommitteeMemberId",
                        column: x => x.CommitteeMemberId,
                        principalTable: "CommitteeMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommitteeExaminationSessions_ExaminationSessions_ExaminationSessionId",
                        column: x => x.ExaminationSessionId,
                        principalTable: "ExaminationSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExaminationTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExaminationSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Question1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Question2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Question3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer3 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExaminationTickets_ExaminationSessions_ExaminationSessionId",
                        column: x => x.ExaminationSessionId,
                        principalTable: "ExaminationSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentExaminationSessions",
                columns: table => new
                {
                    ExaminationSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExaminationSessions", x => new { x.ExaminationSessionId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_StudentExaminationSessions_ExaminationSessions_ExaminationSessionId",
                        column: x => x.ExaminationSessionId,
                        principalTable: "ExaminationSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentExaminationSessions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentPresentations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExaminationSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExaminationTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAbsent = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartingTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TheoryGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPresentations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentPresentations_ExaminationSessions_ExaminationSessionId",
                        column: x => x.ExaminationSessionId,
                        principalTable: "ExaminationSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentPresentations_ExaminationTickets_ExaminationTicketId",
                        column: x => x.ExaminationTicketId,
                        principalTable: "ExaminationTickets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentPresentations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommitteeMemberGrade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommitteeMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TheoryGrade = table.Column<int>(type: "int", nullable: false),
                    ProjectGrade = table.Column<int>(type: "int", nullable: false),
                    StudentPresentationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitteeMemberGrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommitteeMemberGrade_StudentPresentations_StudentPresentationId",
                        column: x => x.StudentPresentationId,
                        principalTable: "StudentPresentations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeExaminationSessions_ExaminationSessionId",
                table: "CommitteeExaminationSessions",
                column: "ExaminationSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeMemberGrade_StudentPresentationId",
                table: "CommitteeMemberGrade",
                column: "StudentPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_CommitteeMembers_Email",
                table: "CommitteeMembers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationSessions_PresentationScheduleId",
                table: "ExaminationSessions",
                column: "PresentationScheduleId",
                unique: true,
                filter: "[PresentationScheduleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationSessions_SecretaryMemberId",
                table: "ExaminationSessions",
                column: "SecretaryMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationTickets_ExaminationSessionId",
                table: "ExaminationTickets",
                column: "ExaminationSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationScheduleEntries_PresentationScheduleId",
                table: "PresentationScheduleEntries",
                column: "PresentationScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentationScheduleEntries_StudentId",
                table: "PresentationScheduleEntries",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretaryMembers_Email",
                table: "SecretaryMembers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExaminationSessions_StudentId",
                table: "StudentExaminationSessions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPresentations_ExaminationSessionId",
                table: "StudentPresentations",
                column: "ExaminationSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPresentations_ExaminationTicketId",
                table: "StudentPresentations",
                column: "ExaminationTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentPresentations_StudentId",
                table: "StudentPresentations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ExternalId",
                table: "Students",
                column: "ExternalId",
                unique: true,
                filter: "[ExternalId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommitteeExaminationSessions");

            migrationBuilder.DropTable(
                name: "CommitteeMemberGrade");

            migrationBuilder.DropTable(
                name: "PresentationScheduleEntries");

            migrationBuilder.DropTable(
                name: "StudentExaminationSessions");

            migrationBuilder.DropTable(
                name: "CommitteeMembers");

            migrationBuilder.DropTable(
                name: "StudentPresentations");

            migrationBuilder.DropTable(
                name: "ExaminationTickets");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "ExaminationSessions");

            migrationBuilder.DropTable(
                name: "PresentationSchedules");

            migrationBuilder.DropTable(
                name: "SecretaryMembers");
        }
    }
}
