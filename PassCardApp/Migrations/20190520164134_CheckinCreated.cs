using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PassCardApp.Migrations
{
    public partial class CheckinCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checkin",
                columns: table => new
                {
                    CheckinId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TicketId = table.Column<int>(nullable: false),
                    CheckedInById = table.Column<string>(nullable: true),
                    ChecinDate = table.Column<DateTime>(nullable: false),
                    CheckedOutById = table.Column<string>(nullable: true),
                    CheckOutDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkin", x => x.CheckinId);
                    table.ForeignKey(
                        name: "FK_Checkin_AspNetUsers_CheckedInById",
                        column: x => x.CheckedInById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Checkin_AspNetUsers_CheckedOutById",
                        column: x => x.CheckedOutById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Checkin_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checkin_CheckedInById",
                table: "Checkin",
                column: "CheckedInById");

            migrationBuilder.CreateIndex(
                name: "IX_Checkin_CheckedOutById",
                table: "Checkin",
                column: "CheckedOutById");

            migrationBuilder.CreateIndex(
                name: "IX_Checkin_TicketId",
                table: "Checkin",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkin");
        }
    }
}
