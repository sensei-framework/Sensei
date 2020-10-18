using System;
using Sensei.AspNet.Models;
using Sensei.AspNet.Queries.Attributes;

namespace Sensei.AspNet.Tests.FakeServer.Entities
{
    public class ProductAlt1 : BaseModel
    {
        public enum StatusEnum
        {
            Normal,
            Promotion
        }

        public enum AvailabilityEnum
        {
            InStock,
            OutOfStock
        }

        [CanFilter]
        public string Name { get; set; }
        
        [CanFilter]
        public bool Enabled { get; set; }
        public float Price { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string Info { get; set; }
        
        public Guid? FileId { get; set; }
        public StatusEnum Status { get; set; }
        public AvailabilityEnum? Availability { get; set; }
        public float? Discount { get; set; }
        public bool? OnlySunday { get; set; }
        public DateTimeOffset AvailableSince { get; set; }
        public TimeSpan DiscountAfter { get; set; }
    }
}