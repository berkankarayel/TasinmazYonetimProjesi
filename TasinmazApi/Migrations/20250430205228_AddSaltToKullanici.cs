using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasinmazApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSaltToKullanici : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Kullanicilar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tasinmazlar_MahalleId",
                table: "Tasinmazlar",
                column: "MahalleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasinmazlar_Mahalleler_MahalleId",
                table: "Tasinmazlar",
                column: "MahalleId",
                principalTable: "Mahalleler",
                principalColumn: "MahalleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasinmazlar_Mahalleler_MahalleId",
                table: "Tasinmazlar");

            migrationBuilder.DropIndex(
                name: "IX_Tasinmazlar_MahalleId",
                table: "Tasinmazlar");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Kullanicilar");
        }
    }
}
