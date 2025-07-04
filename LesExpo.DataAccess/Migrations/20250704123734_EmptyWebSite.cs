using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LesExpo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EmptyWebSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 37, 33, 603, DateTimeKind.Local).AddTicks(3248));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 37, 33, 603, DateTimeKind.Local).AddTicks(3253));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 37, 33, 603, DateTimeKind.Local).AddTicks(3257));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 37, 33, 603, DateTimeKind.Local).AddTicks(3260));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 37, 33, 603, DateTimeKind.Local).AddTicks(3264));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 37, 33, 603, DateTimeKind.Local).AddTicks(3267));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4928));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4933));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4937));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4941));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4944));

            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 15, 19, 4, 931, DateTimeKind.Local).AddTicks(4947));
        }
    }
}
