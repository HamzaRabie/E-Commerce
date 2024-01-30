using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class CartTableAlter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCart_AspNetUsers_UserId",
                table: "UserCart");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCart_Products_ProductId",
                table: "UserCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCart",
                table: "UserCart");

            migrationBuilder.RenameTable(
                name: "UserCart",
                newName: "UserCarts");

            migrationBuilder.RenameIndex(
                name: "IX_UserCart_UserId",
                table: "UserCarts",
                newName: "IX_UserCarts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCart_ProductId",
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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCarts_Products_ProductId",
                table: "UserCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "UserCart");

            migrationBuilder.RenameIndex(
                name: "IX_UserCarts_UserId",
                table: "UserCart",
                newName: "IX_UserCart_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCarts_ProductId",
                table: "UserCart",
                newName: "IX_UserCart_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCart",
                table: "UserCart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCart_AspNetUsers_UserId",
                table: "UserCart",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCart_Products_ProductId",
                table: "UserCart",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
