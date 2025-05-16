using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasinmazApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogRelationForNullableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar");

            migrationBuilder.AlterColumn<int>(
                name: "KullaniciId",
                table: "Loglar",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciId");
        }
    }
}
