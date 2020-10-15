using System;

namespace Sensei.AspNet.Queries.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CanIncludeAttribute : Attribute
    {
        internal bool Enabled { get; }
        
        public CanIncludeAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}