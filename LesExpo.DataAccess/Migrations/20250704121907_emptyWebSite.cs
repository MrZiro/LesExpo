using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LesExpo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class emptyWebSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Tickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "WebSitesi",
                table: "Registrations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CardImageUrl", "CreatedAt", "Slug", "Title" },
                values: new object[] { "/images/ESTAblog1.jpeg", new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4928), "les-expo-fuari-is-birligi-imzalandi-yuk-muhendisligi-istanbul", "İstanbul’da Yük Mühendisliği Sektörünü Buluşturacak LESEXPO Fuarı İçin İş Birliği Protokolü İmzalandı" });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/ESTAblog1.jpeg", new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4933) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/ESTAblog1.jpeg", new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4937) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/ESTAblog1.jpeg", new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4941) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/ESTAblog1.jpeg", new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4944) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/ESTAblog1.jpeg", new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4947) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Website",
                table: "Tickets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WebSitesi",
                table: "Registrations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CardImageUrl", "CreatedAt", "Slug", "Title" },
                values: new object[] { "/images/blog-1.jpg", new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4154), "les-expo-fuari-is-birligi-protokolu-imzalandi", "LES-EXPO Fuarı'nın İş Birliği Protokolü İmzalandı" });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/blog-1.jpg", new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4162) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/blog-2.jpg", new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4168) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/blog-2.jpg", new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4173) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/blog-3.jpg", new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4177) });

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CardImageUrl", "CreatedAt" },
                values: new object[] { "/images/blog-3.jpg", new DateTime(2025, 6, 13, 12, 42, 3, 103, DateTimeKind.Local).AddTicks(4182) });
        }
    }
}
