using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sequel.Demo.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Things",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Things", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherStuffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ThingId = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherStuffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherStuffs_Things_ThingId",
                        column: x => x.ThingId,
                        principalTable: "Things",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtherStuffs_ThingId",
                table: "OtherStuffs",
                column: "ThingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtherStuffs");

            migrationBuilder.DropTable(
                name: "Things");
        }
    }
}
