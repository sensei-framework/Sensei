using System;
using CatalogApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sensei.AspNet;

// ReSharper disable UnusedMember.Global

namespace CatalogApplication
{
    public class CatalogDbContext : SenseiDbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IServiceProvider serviceProvider)
            : base(options, serviceProvider)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleCategory>()
                .HasKey(t => new {t.ArticleId, t.CategoryId});

            modelBuilder.Entity<ArticleCategory>()
                .HasOne(ac => ac.Article)
                .WithMany(a => a.ArticleCategories)
                .HasForeignKey(ac => ac.ArticleId);

            modelBuilder.Entity<ArticleCategory>()
                .HasOne(ac => ac.Category)
                .WithMany(a => a.ArticleCategories)
                .HasForeignKey(ac => ac.CategoryId);
        }
    }
}