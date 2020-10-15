using System;

namespace Sensei.AspNet.Queries.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CanFilterAttribute : Attribute
    {
        internal bool Enabled { get; }
        
        public CanFilterAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}