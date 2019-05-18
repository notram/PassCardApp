using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PassCardApp.Migrations
{
    public partial class clientextnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InserterId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "InserterName",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "InsertedById",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_InsertedById",
                table: "Clients",
                column: "InsertedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_AspNetUsers_InsertedById",
                table: "Clients",
                column: "InsertedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_AspNetUsers_InsertedById",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_InsertedById",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "InsertedById",
                table: "Clients");

            migrationBuilder.AddColumn<Guid>(
                name: "InserterId",
                table: "Clients",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "InserterName",
                table: "Clients",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
