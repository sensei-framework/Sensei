using System;
using Sensei.AspNet.Models;

namespace Sensei.AspNet.Tests.FakeServer.Entities
{
    public class CategoryTimeSlot : BaseModel
    {
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        
        public Guid TimeSlotId { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}