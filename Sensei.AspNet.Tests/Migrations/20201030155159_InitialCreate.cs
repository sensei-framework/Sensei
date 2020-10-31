using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sensei.AspNet.Tests.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Categories",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Info = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Categories", x => x.Id); });

            migrationBuilder.CreateTable(
                "TimeSlots",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_TimeSlots", x => x.Id); });

            migrationBuilder.CreateTable(
                "Products",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    CategoryAltId = table.Column<Guid>(nullable: false),
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
                        "FK_Products_Categories_CategoryAltId",
                        x => x.CategoryAltId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Products_Categories_CategoryId",
                        x => x.CategoryId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ProductsAlt1",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    CategoryAltId = table.Column<Guid>(nullable: false),
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
                        "FK_ProductsAlt1_Categories_CategoryAltId",
                        x => x.CategoryAltId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_ProductsAlt1_Categories_CategoryId",
                        x => x.CategoryId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ProductsAlt2",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    CategoryAltId = table.Column<Guid>(nullable: false),
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
                        "FK_ProductsAlt2_Categories_CategoryAltId",
                        x => x.CategoryAltId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_ProductsAlt2_Categories_CategoryId",
                        x => x.CategoryId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "CategoryTimeSlots",
                table => new
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
                        "FK_CategoryTimeSlots_Categories_CategoryId",
                        x => x.CategoryId,
                        "Categories",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_CategoryTimeSlots_TimeSlots_TimeSlotId",
                        x => x.TimeSlotId,
                        "TimeSlots",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_CategoryTimeSlots_CategoryId",
                "CategoryTimeSlots",
                "CategoryId");

            migrationBuilder.CreateIndex(
                "IX_CategoryTimeSlots_TimeSlotId",
                "CategoryTimeSlots",
                "TimeSlotId");

            migrationBuilder.CreateIndex(
                "IX_Products_CategoryAltId",
                "Products",
                "CategoryAltId");

            migrationBuilder.CreateIndex(
                "IX_Products_CategoryId",
                "Products",
                "CategoryId");

            migrationBuilder.CreateIndex(
                "IX_ProductsAlt1_CategoryAltId",
                "ProductsAlt1",
                "CategoryAltId");

            migrationBuilder.CreateIndex(
                "IX_ProductsAlt1_CategoryId",
                "ProductsAlt1",
                "CategoryId");

            migrationBuilder.CreateIndex(
                "IX_ProductsAlt2_CategoryAltId",
                "ProductsAlt2",
                "CategoryAltId");

            migrationBuilder.CreateIndex(
                "IX_ProductsAlt2_CategoryId",
                "ProductsAlt2",
                "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "CategoryTimeSlots");

            migrationBuilder.DropTable(
                "Products");

            migrationBuilder.DropTable(
                "ProductsAlt1");

            migrationBuilder.DropTable(
                "ProductsAlt2");

            migrationBuilder.DropTable(
                "TimeSlots");

            migrationBuilder.DropTable(
                "Categories");
        }
    }
}