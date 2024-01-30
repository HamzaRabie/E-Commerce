using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class orders_ProductsM2M : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersCarts_AspNetUsers_UserId",
                table: "UsersCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersCarts_Orders_OrderId",
                table: "UsersCarts");

            migrationBuilder.DropIndex(
                name: "IX_UsersCarts_OrderId",
                table: "UsersCarts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "UsersCarts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersCarts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => new { x.OrdersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_OrderProduct_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductsId",
                table: "OrderProduct",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCarts_AspNetUsers_UserId",
                table: "UsersCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersCarts_AspNetUsers_UserId",
                table: "UsersCarts");

            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersCarts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "UsersCarts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersCarts_OrderId",
                table: "UsersCarts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCarts_AspNetUsers_UserId",
                table: "UsersCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersCarts_Orders_OrderId",
                table: "UsersCarts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
