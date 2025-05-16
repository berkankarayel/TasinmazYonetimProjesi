using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasinmazApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLogNoActionRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar");

            migrationBuilder.AddForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar");

            migrationBuilder.AddForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
