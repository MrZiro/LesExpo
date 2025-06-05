using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LesExpo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YoutubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MetaKeywords = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContentTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_ContentTypes_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ContentTypes",
                columns: new[] { "Id", "Name", "NameEn" },
                values: new object[,]
                {
                    { 1, "Blog", "Blog" },
                    { 2, "Gündem", "Agenda" },
                    { 3, "Duyurular", "Announcements" },
                    { 4, "Haberler", "News" }
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Author", "CardImageUrl", "Content", "ContentTypeId", "CreatedAt", "IsPublished", "Language", "MetaDescription", "MetaKeywords", "Slug", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "LesExpo Admin", "/images/blog-1.jpg", "Türkiye ekonomisine yaklaşık 12 milyar doların üzerinde katkı sağlayan yük mühendisliği hizmetleri ekosistemindeki hizmet alan ve hizmet verenleri bir araya getirecek ilk ihtisas fuarı LES-EXPO, İstanbul'da düzenleniyor.", 1, new DateTime(2025, 6, 5, 2, 28, 56, 274, DateTimeKind.Local).AddTicks(9324), true, "tr", "LES-EXPO Fuarı'nın iş birliği protokolü imzalandı. Yük mühendisliği sektörünün ilk ihtisas fuarı İstanbul'da düzenleniyor.", "LES-EXPO, fuar, yük mühendisliği, iş birliği protokolü, İstanbul", "les-expo-fuari-is-birligi-protokolu-imzalandi", "LES-EXPO Fuarı'nın İş Birliği Protokolü İmzalandı", null },
                    { 2, "LesExpo Admin", "/images/blog-1.jpg", "LES-EXPO, the first specialized fair that will bring together service providers and recipients in the heavy engineering services ecosystem that contributes more than 12 billion dollars to the Turkish economy, is being held in Istanbul.", 1, new DateTime(2025, 6, 5, 2, 28, 56, 274, DateTimeKind.Local).AddTicks(9333), true, "en", "The cooperation protocol of the LES-EXPO Fair has been signed. The first specialized fair of the heavy engineering sector is being held in Istanbul.", "LES-EXPO, fair, heavy engineering, cooperation protocol, Istanbul", "les-expo-fair-cooperation-protocol-signed", "LES-EXPO Fair's Cooperation Protocol Signed", null },
                    { 3, "LesExpo Admin", "/images/blog-2.jpg", "Yük mühendisliği sektöründe son dönemde yaşanan teknolojik gelişmeler ve yenilikler, sektörün geleceğini şekillendiriyor. Akıllı vinç sistemleri ve otomatik yük taşıma çözümleri öne çıkıyor.", 1, new DateTime(2025, 6, 5, 2, 28, 56, 274, DateTimeKind.Local).AddTicks(9338), true, "tr", "Yük mühendisliği sektöründeki teknolojik gelişmeler ve yenilikler hakkında detaylı bilgi.", "yük mühendisliği, teknoloji, akıllı vinç, otomatik taşıma, yenilikler", "yuk-muhendisligi-sektorunde-yeni-teknolojiler", "Yük Mühendisliği Sektöründe Yeni Teknolojiler", null },
                    { 4, "LesExpo Admin", "/images/blog-2.jpg", "Recent technological developments and innovations in the heavy engineering sector are shaping its future. Smart crane systems and automated load handling solutions are leading the way.", 1, new DateTime(2025, 6, 5, 2, 28, 56, 274, DateTimeKind.Local).AddTicks(9342), true, "en", "Detailed information about technological developments and innovations in the heavy engineering sector.", "heavy engineering, technology, smart crane, automated handling, innovations", "innovations-in-heavy-engineering-sector", "Innovations in Heavy Engineering Sector", null },
                    { 5, "LesExpo Admin", "/images/blog-3.jpg", "Yük mühendisliği sektöründe güvenlik standartları ve yeni düzenlemeler hakkında güncel bilgiler. İş güvenliği ve kalite standartları konusunda yapılan güncellemeler.", 1, new DateTime(2025, 6, 5, 2, 28, 56, 274, DateTimeKind.Local).AddTicks(9346), true, "tr", "Yük mühendisliği sektöründeki güvenlik standartları ve yeni düzenlemeler hakkında bilgi.", "güvenlik standartları, iş güvenliği, kalite standartları, düzenlemeler", "sektorde-guvenlik-standartlari-ve-yeni-duzenlemeler", "Sektörde Güvenlik Standartları ve Yeni Düzenlemeler", null },
                    { 6, "LesExpo Admin", "/images/blog-3.jpg", "Current information about safety standards and new regulations in the heavy engineering sector. Updates regarding occupational safety and quality standards.", 1, new DateTime(2025, 6, 5, 2, 28, 56, 274, DateTimeKind.Local).AddTicks(9350), true, "en", "Information about safety standards and new regulations in the heavy engineering sector.", "safety standards, occupational safety, quality standards, regulations", "safety-standards-and-new-regulations-in-the-industry", "Safety Standards and New Regulations in the Industry", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ContentTypeId",
                table: "Blogs",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Slug",
                table: "Blogs",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ContentTypes");
        }
    }
}
