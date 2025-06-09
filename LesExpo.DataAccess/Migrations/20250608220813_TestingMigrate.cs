using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LesExpo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class TestingMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Author", "CardImageUrl", "Content", "ContentTypeId", "CreatedAt", "IsPublished", "Language", "MetaDescription", "MetaKeywords", "Slug", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "LesExpo Admin", "/images/blog-1.jpg", "Türkiye ekonomisine yaklaşık 12 milyar doların üzerinde katkı sağlayan yük mühendisliği hizmetleri ekosistemindeki hizmet alan ve hizmet verenleri bir araya getirecek ilk ihtisas fuarı LES-EXPO, İstanbul'da düzenleniyor.", 1, new DateTime(2025, 6, 9, 1, 8, 11, 404, DateTimeKind.Local).AddTicks(6989), true, "tr", "LES-EXPO Fuarı'nın iş birliği protokolü imzalandı. Yük mühendisliği sektörünün ilk ihtisas fuarı İstanbul'da düzenleniyor.", "LES-EXPO, fuar, yük mühendisliği, iş birliği protokolü, İstanbul", "les-expo-fuari-is-birligi-protokolu-imzalandi", "LES-EXPO Fuarı'nın İş Birliği Protokolü İmzalandı", null },
                    { 2, "LesExpo Admin", "/images/blog-1.jpg", "LES-EXPO, the first specialized fair that will bring together service providers and recipients in the heavy engineering services ecosystem that contributes more than 12 billion dollars to the Turkish economy, is being held in Istanbul.", 1, new DateTime(2025, 6, 9, 1, 8, 11, 404, DateTimeKind.Local).AddTicks(7034), true, "en", "The cooperation protocol of the LES-EXPO Fair has been signed. The first specialized fair of the heavy engineering sector is being held in Istanbul.", "LES-EXPO, fair, heavy engineering, cooperation protocol, Istanbul", "les-expo-fair-cooperation-protocol-signed", "LES-EXPO Fair's Cooperation Protocol Signed", null },
                    { 3, "LesExpo Admin", "/images/blog-2.jpg", "Yük mühendisliği sektöründe son dönemde yaşanan teknolojik gelişmeler ve yenilikler, sektörün geleceğini şekillendiriyor. Akıllı vinç sistemleri ve otomatik yük taşıma çözümleri öne çıkıyor.", 1, new DateTime(2025, 6, 9, 1, 8, 11, 404, DateTimeKind.Local).AddTicks(7036), true, "tr", "Yük mühendisliği sektöründeki teknolojik gelişmeler ve yenilikler hakkında detaylı bilgi.", "yük mühendisliği, teknoloji, akıllı vinç, otomatik taşıma, yenilikler", "yuk-muhendisligi-sektorunde-yeni-teknolojiler", "Yük Mühendisliği Sektöründe Yeni Teknolojiler", null },
                    { 4, "LesExpo Admin", "/images/blog-2.jpg", "Recent technological developments and innovations in the heavy engineering sector are shaping its future. Smart crane systems and automated load handling solutions are leading the way.", 1, new DateTime(2025, 6, 9, 1, 8, 11, 404, DateTimeKind.Local).AddTicks(7039), true, "en", "Detailed information about technological developments and innovations in the heavy engineering sector.", "heavy engineering, technology, smart crane, automated handling, innovations", "innovations-in-heavy-engineering-sector", "Innovations in Heavy Engineering Sector", null },
                    { 5, "LesExpo Admin", "/images/blog-3.jpg", "Yük mühendisliği sektöründe güvenlik standartları ve yeni düzenlemeler hakkında güncel bilgiler. İş güvenliği ve kalite standartları konusunda yapılan güncellemeler.", 1, new DateTime(2025, 6, 9, 1, 8, 11, 404, DateTimeKind.Local).AddTicks(7041), true, "tr", "Yük mühendisliği sektöründeki güvenlik standartları ve yeni düzenlemeler hakkında bilgi.", "güvenlik standartları, iş güvenliği, kalite standartları, düzenlemeler", "sektorde-guvenlik-standartlari-ve-yeni-duzenlemeler", "Sektörde Güvenlik Standartları ve Yeni Düzenlemeler", null },
                    { 6, "LesExpo Admin", "/images/blog-3.jpg", "Current information about safety standards and new regulations in the heavy engineering sector. Updates regarding occupational safety and quality standards.", 1, new DateTime(2025, 6, 9, 1, 8, 11, 404, DateTimeKind.Local).AddTicks(7043), true, "en", "Information about safety standards and new regulations in the heavy engineering sector.", "safety standards, occupational safety, quality standards, regulations", "safety-standards-and-new-regulations-in-the-industry", "Safety Standards and New Regulations in the Industry", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
