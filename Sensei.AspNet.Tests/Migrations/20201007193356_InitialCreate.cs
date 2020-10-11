using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sensei.AspNet.Tests.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestChildModel2",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestChildModel2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestChildModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ChildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestChildModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestChildModel_TestChildModel2_ChildId",
                        column: x => x.ChildId,
                        principalTable: "TestChildModel2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TestFlag = table.Column<bool>(nullable: false),
                    TestDateTime = table.Column<DateTime>(nullable: false),
                    TestInt = table.Column<int>(nullable: false),
                    TestInt2 = table.Column<int>(nullable: true),
                    TestDecimal = table.Column<decimal>(nullable: false),
                    TestDecimal2 = table.Column<decimal>(nullable: true),
                    UserIdAdded = table.Column<Guid>(nullable: false),
                    UserIdAddedSkipIfExist = table.Column<Guid>(nullable: false),
                    UserIdAddedSkipIfExist2 = table.Column<Guid>(nullable: false),
                    UserIdModified = table.Column<Guid>(nullable: false),
                    ClaimAdded = table.Column<string>(nullable: true),
                    ClaimAddedSkipIfExist = table.Column<string>(nullable: true),
                    ClaimAddedSkipIfExist2 = table.Column<string>(nullable: true),
                    ClaimModified = table.Column<string>(nullable: true),
                    Child1Id = table.Column<Guid>(nullable: true),
                    Child2Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestModels_TestChildModel_Child1Id",
                        column: x => x.Child1Id,
                        principalTable: "TestChildModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestModels_TestChildModel2_Child2Id",
                        column: x => x.Child2Id,
                        principalTable: "TestChildModel2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestChildModel_ChildId",
                table: "TestChildModel",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_TestModels_Child1Id",
                table: "TestModels",
                column: "Child1Id");

            migrationBuilder.CreateIndex(
                name: "IX_TestModels_Child2Id",
                table: "TestModels",
                column: "Child2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestModels");

            migrationBuilder.DropTable(
                name: "TestChildModel");

            migrationBuilder.DropTable(
                name: "TestChildModel2");
        }
    }
}
