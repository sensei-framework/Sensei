using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Sensei.AspNet.Utils.Database
{
    public static class Seeder
    {
        public static void Seed<TEntity>(this DbContext dbContext, string path)
            where TEntity : class
        {
            var dbSet = dbContext.Set<TEntity>();
            if (dbSet.Any())
                return;
            
            var json = File.ReadAllText(path);
            var items = JsonConvert.DeserializeObject<List<TEntity>>(json);
            dbSet.AddRange(items);
            dbContext.SaveChanges();
        }
    }
}