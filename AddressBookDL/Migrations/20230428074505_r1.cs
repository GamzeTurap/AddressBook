using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AddressBookDL.Migrations
{
    /// <inheritdoc />
    public partial class r1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Neighbourhoods_Neighbourhood",
                table: "UserAddresses");

            migrationBuilder.DropIndex(
                name: "IX_UserAddresses_Neighbourhood",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "Neighbourhood",
                table: "UserAddresses");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_NeighbourhoodId",
                table: "UserAddresses",
                column: "NeighbourhoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_Neighbourhoods_NeighbourhoodId",
                table: "UserAddresses",
                column: "NeighbourhoodId",
                principalTable: "Neighbourhoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Neighbourhoods_NeighbourhoodId",
                table: "UserAddresses");

            migrationBuilder.DropIndex(
                name: "IX_UserAddresses_NeighbourhoodId",
                table: "UserAddresses");

            migrationBuilder.AddColumn<int>(
                name: "Neighbourhood",
                table: "UserAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_Neighbourhood",
                table: "UserAddresses",
                column: "Neighbourhood");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_Neighbourhoods_Neighbourhood",
                table: "UserAddresses",
                column: "Neighbourhood",
                principalTable: "Neighbourhoods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
