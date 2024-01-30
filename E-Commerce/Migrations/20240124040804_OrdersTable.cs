using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class OrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCarts_AspNetUsers_UserId",
                table: "UserCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCarts_Products_ProductId",
                table: "UserCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCarts",
                table: "UserCarts");

            migrationBuilder.RenameTable(
                name: "UserCarts",
                newName: "UsersCarts");

            migrationBuilder.RenameIndex(
                name: "IX_UserCarts_UserId",
                table: "UsersCarts",
                newName: "IX_UsersCarts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCarts_ProductId",
                table: "UsersCarts",
                newName: "IX_UsersCarts_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "OrdersId",
                table: "UsersCarts",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersCarts",
                table: "UsersCarts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersCarts_OrdersId",
                table: "UsersCarts",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCarts_AspNetUsers_UserId",
                table: "UsersCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCarts_Orders_OrdersId",
                table: "UsersCarts",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCarts_Products_ProductId",
                table: "UsersCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersCarts_AspNetUsers_UserId",
                table: "UsersCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCarts_Orders_OrdersId",
                table: "UsersCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCarts_Products_ProductId",
                table: "UsersCarts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersCarts",
                table: "UsersCarts");

            migrationBuilder.DropIndex(
                name: "IX_UsersCarts_OrdersId",
                table: "UsersCarts");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "UsersCarts");

            migrationBuilder.RenameTable(
                name: "UsersCarts",
                newName: "UserCarts");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCarts_UserId",
                table: "UserCarts",
                newName: "IX_UserCarts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersCarts_ProductId",
                table: "UserCarts",
                newName: "IX_UserCarts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCarts",
                table: "UserCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCarts_AspNetUsers_UserId",
                table: "UserCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCarts_Products_ProductId",
                table: "UserCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
