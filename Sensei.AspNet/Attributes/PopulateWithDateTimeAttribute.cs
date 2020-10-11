using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sensei.AspNet.DbProcessor;

namespace Sensei.AspNet.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PopulateWithDateTimeAttribute : Attribute, IDbProcessorAttribute
    {
        public PopulateWithDateTimeAttribute(DbEntityState state)
        {
            State = state;
        }

        public DbEntityState State { get; }

        public object PreProcess(DbContext dbContext, IServiceProvider serviceProvider, EntityEntry entityEntry,
            object currentValue, object originalValue, Type propertyType)
        {
            return DateTime.Now;
        }

        public void PostProcess(DbContext dbContext, IServiceProvider serviceProvider, EntityEntry entityEntry,
            object currentValue, object originalValue, Type propertyType)
        {
        }
    }
}