using System;

namespace Sensei.AspNet.Queries.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CanSortAttribute : Attribute
    {
        internal bool Enabled { get; }
        
        public CanSortAttribute(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}