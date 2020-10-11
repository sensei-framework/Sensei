using System;
using System.Collections.Generic;
using IdentityModel;
using Sensei.AspNet.Attributes;
using Sensei.AspNet.DbProcessor;
using Sensei.AspNet.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace CatalogApplication.Models
{
    public class Article : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
        [PopulateWithClaim(DbEntityState.Added, JwtClaimTypes.Subject)]
        public Guid OwnerId { get; set; }
        
        public Category MainCategory { get; set; }
        
        public StatusEnum Status { get; set; }
        public List<ArticleCategory> ArticleCategories { get; set; }
    }
}