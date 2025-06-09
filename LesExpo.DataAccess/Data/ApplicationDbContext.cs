using LesExpo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Seed default content types
            modelBuilder.Entity<ContentType>().HasData(
                new ContentType { Id = 1, Name = "Blog", NameEn = "Blog" },
                new ContentType { Id = 2, Name = "Gündem", NameEn = "Agenda" },
                new ContentType { Id = 3, Name = "Duyurular", NameEn = "Announcements" },
                new ContentType { Id = 4, Name = "Haberler", NameEn = "News" }
            );
            modelBuilder.Entity<Blog>().HasData(
                new Blog {
                Id = 1,
                Language = "tr",
                Title = "LES-EXPO Fuarı'nın İş Birliği Protokolü İmzalandı",
                Slug = "les-expo-fuari-is-birligi-protokolu-imzalandi",
                Content = "Türkiye ekonomisine yaklaşık 12 milyar doların üzerinde katkı sağlayan yük mühendisliği hizmetleri ekosistemindeki hizmet alan ve hizmet verenleri bir araya getirecek ilk ihtisas fuarı LES-EXPO, İstanbul'da düzenleniyor.",
                CardImageUrl = "/images/blog-1.jpg",
                CreatedAt = DateTime.Now,
                IsPublished = true,
                Author = "LesExpo Admin",
                MetaDescription = "LES-EXPO Fuarı'nın iş birliği protokolü imzalandı. Yük mühendisliği sektörünün ilk ihtisas fuarı İstanbul'da düzenleniyor.",
                MetaKeywords = "LES-EXPO, fuar, yük mühendisliği, iş birliği protokolü, İstanbul",
                ContentTypeId = 1
                },
                new Blog {
                    Id = 2,
                    Language = "en",
                    Title = "LES-EXPO Fair's Cooperation Protocol Signed",
                    Slug = "les-expo-fair-cooperation-protocol-signed",
                    Content = "LES-EXPO, the first specialized fair that will bring together service providers and recipients in the heavy engineering services ecosystem that contributes more than 12 billion dollars to the Turkish economy, is being held in Istanbul.",
                    CardImageUrl = "/images/blog-1.jpg",
                    CreatedAt = DateTime.Now,
                    IsPublished = true,
                    Author = "LesExpo Admin",
                    MetaDescription = "The cooperation protocol of the LES-EXPO Fair has been signed. The first specialized fair of the heavy engineering sector is being held in Istanbul.",
                    MetaKeywords = "LES-EXPO, fair, heavy engineering, cooperation protocol, Istanbul",
                    ContentTypeId = 1
                },
                new Blog {
                    Id = 3,
                    Language = "tr",
                    Title = "Yük Mühendisliği Sektöründe Yeni Teknolojiler",
                    Slug = "yuk-muhendisligi-sektorunde-yeni-teknolojiler",
                    Content = "Yük mühendisliği sektöründe son dönemde yaşanan teknolojik gelişmeler ve yenilikler, sektörün geleceğini şekillendiriyor. Akıllı vinç sistemleri ve otomatik yük taşıma çözümleri öne çıkıyor.",
                    CardImageUrl = "/images/blog-2.jpg",
                    CreatedAt = DateTime.Now,
                    IsPublished = true,
                    Author = "LesExpo Admin",
                    MetaDescription = "Yük mühendisliği sektöründeki teknolojik gelişmeler ve yenilikler hakkında detaylı bilgi.",
                    MetaKeywords = "yük mühendisliği, teknoloji, akıllı vinç, otomatik taşıma, yenilikler",
                    ContentTypeId = 1
                },
                new Blog {
                    Id = 4,
                    Language = "en",
                    Title = "Innovations in Heavy Engineering Sector",
                    Slug = "innovations-in-heavy-engineering-sector",
                    Content = "Recent technological developments and innovations in the heavy engineering sector are shaping its future. Smart crane systems and automated load handling solutions are leading the way.",
                    CardImageUrl = "/images/blog-2.jpg",
                    CreatedAt = DateTime.Now,
                    IsPublished = true,
                    Author = "LesExpo Admin",
                    MetaDescription = "Detailed information about technological developments and innovations in the heavy engineering sector.",
                    MetaKeywords = "heavy engineering, technology, smart crane, automated handling, innovations",
                    ContentTypeId = 1
                },
                new Blog {
                    Id = 5,
                    Language = "tr",
                    Title = "Sektörde Güvenlik Standartları ve Yeni Düzenlemeler",
                    Slug = "sektorde-guvenlik-standartlari-ve-yeni-duzenlemeler",
                    Content = "Yük mühendisliği sektöründe güvenlik standartları ve yeni düzenlemeler hakkında güncel bilgiler. İş güvenliği ve kalite standartları konusunda yapılan güncellemeler.",
                    CardImageUrl = "/images/blog-3.jpg",
                    CreatedAt = DateTime.Now,
                    IsPublished = true,
                    Author = "LesExpo Admin",
                    MetaDescription = "Yük mühendisliği sektöründeki güvenlik standartları ve yeni düzenlemeler hakkında bilgi.",
                    MetaKeywords = "güvenlik standartları, iş güvenliği, kalite standartları, düzenlemeler",
                    ContentTypeId = 1
                },
                new Blog {
                    Id = 6,
                    Language = "en",
                    Title = "Safety Standards and New Regulations in the Industry",
                    Slug = "safety-standards-and-new-regulations-in-the-industry",
                    Content = "Current information about safety standards and new regulations in the heavy engineering sector. Updates regarding occupational safety and quality standards.",
                    CardImageUrl = "/images/blog-3.jpg",
                    CreatedAt = DateTime.Now,
                    IsPublished = true,
                    Author = "LesExpo Admin",
                    MetaDescription = "Information about safety standards and new regulations in the heavy engineering sector.",
                    MetaKeywords = "safety standards, occupational safety, quality standards, regulations",
                    ContentTypeId = 1
                }
            );
            

        }
    }
}
