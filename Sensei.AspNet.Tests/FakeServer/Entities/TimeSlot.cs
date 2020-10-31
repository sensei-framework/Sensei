using System;
using System.Collections.Generic;
using Sensei.AspNet.Models;

namespace Sensei.AspNet.Tests.FakeServer.Entities
{
    public class TimeSlot : BaseModel
    {
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public ICollection<CategoryTimeSlot> CategoryTimeSlots { get; set; }
    }
}