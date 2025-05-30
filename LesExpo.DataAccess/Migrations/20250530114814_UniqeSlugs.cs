using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LesExpo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UniqeSlugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Slug",
                table: "Blogs",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_Slug",
                table: "Blogs");
        }
    }
}
