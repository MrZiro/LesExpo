using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LesExpo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Addedregtick : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SirketAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AdSoyad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gorev = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SirketAdresi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Ulke = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sehir = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WebSitesi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FaaliyetAlani = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UrunGrubu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Markalar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IstenenMetrekare = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KurulusTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AktiviteTuru = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IhracatCirosu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ToplamCiro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PersonelSayisi = table.Column<int>(type: "int", nullable: false),
                    FuarKatilim = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirmaZiyareti = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UlusalFuarlarJson = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    UluslararasiFuarlarJson = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Terms = table.Column<bool>(type: "bit", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApiResponse = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ApiSuccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4154));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4162));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4168));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4173));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4177));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4182));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 16, 8, 28, 239, DateTimeKind.Local).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 16, 8, 28, 239, DateTimeKind.Local).AddTicks(375));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 16, 8, 28, 239, DateTimeKind.Local).AddTicks(378));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 16, 8, 28, 239, DateTimeKind.Local).AddTicks(381));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 16, 8, 28, 239, DateTimeKind.Local).AddTicks(384));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 12, 16, 8, 28, 239, DateTimeKind.Local).AddTicks(386));
        }
    }
}
