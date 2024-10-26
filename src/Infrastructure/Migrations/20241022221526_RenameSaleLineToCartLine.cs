using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameSaleLineToCartLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Renombrar la tabla SaleLine a CartLine
            migrationBuilder.RenameTable(
                name: "SaleLine",
                newName: "CartLine"
            );

            // Si es necesario, renombrar las columnas
            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "CartLine",
                newName: "CartId"
            );

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartLine",
                newName: "ProductId"
            );

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "CartLine",
                newName: "Quantity"
            );

            migrationBuilder.RenameColumn(
                name: "SubtotalPrice",
                table: "CartLine",
                newName: "SubtotalPrice"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir el cambio: renombrar la tabla CartLine a SaleLine
            migrationBuilder.RenameTable(
                name: "CartLine",
                newName: "SaleLine"
            );

            // Revertir los cambios de nombre de columna si es necesario
            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "SaleLine",
                newName: "CartId"
            );

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "SaleLine",
                newName: "ProductId"
            );

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "SaleLine",
                newName: "Quantity"
            );

            migrationBuilder.RenameColumn(
                name: "SubtotalPrice",
                table: "SaleLine",
                newName: "SubtotalPrice"
            );
        }
    }
}

