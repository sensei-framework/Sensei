using System.Collections.Generic;
using Sensei.AspNet.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace CatalogApplication.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ArticleCategory> ArticleCategories { get; set; }
    }
}