using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sensei.AspNet.Tests.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Info = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Info = table.Column<string>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Availability = table.Column<int>(nullable: true),
                    Discount = table.Column<float>(nullable: true),
                    OnlySunday = table.Column<bool>(nullable: true),
                    AvailableSince = table.Column<long>(nullable: false),
                    DiscountAfter = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsAlt1",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Info = table.Column<string>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Availability = table.Column<int>(nullable: true),
                    Discount = table.Column<float>(nullable: true),
                    OnlySunday = table.Column<bool>(nullable: true),
                    AvailableSince = table.Column<long>(nullable: false),
                    DiscountAfter = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsAlt1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsAlt1_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsAlt2",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Info = table.Column<string>(nullable: true),
                    FileId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Availability = table.Column<int>(nullable: true),
                    Discount = table.Column<float>(nullable: true),
                    OnlySunday = table.Column<bool>(nullable: true),
                    AvailableSince = table.Column<long>(nullable: false),
                    DiscountAfter = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsAlt2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsAlt2_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTimeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    TimeSlotId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTimeSlots_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTimeSlots_TimeSlots_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "TimeSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTimeSlots_CategoryId",
                table: "CategoryTimeSlots",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTimeSlots_TimeSlotId",
                table: "CategoryTimeSlots",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsAlt1_CategoryId",
                table: "ProductsAlt1",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsAlt2_CategoryId",
                table: "ProductsAlt2",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTimeSlots");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductsAlt1");

            migrationBuilder.DropTable(
                name: "ProductsAlt2");

            migrationBuilder.DropTable(
                name: "TimeSlots");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
