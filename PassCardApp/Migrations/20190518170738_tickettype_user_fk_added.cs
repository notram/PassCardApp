using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PassCardApp.Migrations
{
    public partial class tickettype_user_fk_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InsertedBy",
                table: "Clients",
                newName: "InserterName");

            migrationBuilder.AlterColumn<string>(
                name: "InsertedBy",
                table: "TicketTypes",
                nullable: true,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InserterId",
                table: "Clients",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_InsertedBy",
                table: "TicketTypes",
                column: "InsertedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketTypes_AspNetUsers_InsertedBy",
                table: "TicketTypes",
                column: "InsertedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketTypes_AspNetUsers_InsertedBy",
                table: "TicketTypes");

            migrationBuilder.DropIndex(
                name: "IX_TicketTypes_InsertedBy",
                table: "TicketTypes");

            migrationBuilder.DropColumn(
                name: "InserterId",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "InserterName",
                table: "Clients",
                newName: "InsertedBy");

            migrationBuilder.AlterColumn<Guid>(
                name: "InsertedBy",
                table: "TicketTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
