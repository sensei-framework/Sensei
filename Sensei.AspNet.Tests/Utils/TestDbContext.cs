//#define UPDATE_MIGRATIONS
using System;
using Microsoft.EntityFrameworkCore;

namespace Sensei.AspNet.Tests.Utils
{
    public class TestDbContext : SenseiDbContext
    {
        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<TestChildModel> TestChildModel { get; set; }
        public DbSet<TestChildModel2> TestChildModel2 { get; set; }

#if UPDATE_MIGRATIONS
        public TestDbContext()
        {
        }
#endif
        
        public TestDbContext(DbContextOptions<TestDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }

#if UPDATE_MIGRATIONS
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=:memory:");
        }
#endif
    }
}