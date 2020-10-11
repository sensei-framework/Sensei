using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Sensei.AspNet.DbProcessor
{
    public interface IDbProcessorAttribute
    {
        DbEntityState State { get; }

        object PreProcess(DbContext dbContext, IServiceProvider serviceProvider, EntityEntry entityEntry,
            object currentValue, object originalValue, Type propertyType);

        void PostProcess(DbContext dbContext, IServiceProvider serviceProvider, EntityEntry entityEntry,
            object currentValue, object originalValue, Type propertyType);
    }
}