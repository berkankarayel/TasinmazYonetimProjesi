using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasinmazApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLogNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar");

            migrationBuilder.DropIndex(
                name: "IX_Loglar_KullaniciId",
                table: "Loglar");

            migrationBuilder.AlterColumn<int>(
                name: "KullaniciId",
                table: "Loglar",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "KullaniciId",
                table: "Loglar",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Loglar_KullaniciId",
                table: "Loglar",
                column: "KullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
