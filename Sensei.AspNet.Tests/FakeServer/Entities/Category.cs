using System.Collections.Generic;
using Sensei.AspNet.Models;

namespace Sensei.AspNet.Tests.FakeServer.Entities
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Info { get; set; }

        public ICollection<CategoryTimeSlot> CategoryTimeSlots { get; set; }
    }
}