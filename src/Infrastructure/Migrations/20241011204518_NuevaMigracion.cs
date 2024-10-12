using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NuevaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SaleLine_SaleLineId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleLine_Carts_CartId",
                table: "SaleLine");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleLine_Products_ProductId",
                table: "SaleLine");

            migrationBuilder.DropIndex(
                name: "IX_Products_SaleLineId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubtotalPrice",
                table: "SaleLine");

            migrationBuilder.DropColumn(
                name: "SaleLineId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "SaleLine",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLine_Carts_CartId",
                table: "SaleLine",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLine_Products_ProductId",
                table: "SaleLine",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleLine_Carts_CartId",
                table: "SaleLine");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleLine_Products_ProductId",
                table: "SaleLine");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "SaleLine",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<double>(
                name: "SubtotalPrice",
                table: "SaleLine",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "SaleLineId",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SaleLineId",
                table: "Products",
                column: "SaleLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SaleLine_SaleLineId",
                table: "Products",
                column: "SaleLineId",
                principalTable: "SaleLine",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLine_Carts_CartId",
                table: "SaleLine",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLine_Products_ProductId",
                table: "SaleLine",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
