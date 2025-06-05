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
            

        }
    }
}
