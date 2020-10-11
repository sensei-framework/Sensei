using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sensei.AspNet.DbProcessor
{
    /// <summary>
    /// Process the entities attributes when db is saving data
    /// </summary>
    public static class DbProcessor
    {
        /// <summary>
        /// The pre process run just before the DB is saved
        /// </summary>
        /// <param name="dbContext">The parent db context</param>
        /// <param name="serviceProvider">The instance of service provider</param>
        public static void PreProcess(DbContext dbContext, IServiceProvider serviceProvider)
        {
            var entities = dbContext.ChangeTracker.Entries().Where(x =>
                x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

            foreach (var entity in entities)
            foreach (var property in entity.Entity.GetType().GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(IDbProcessorAttribute), true);
                foreach (var attribute in attributes)
                {
                    if (!IsEntityStateSupported(((IDbProcessorAttribute) attribute).State, entity.State))
                        continue;

                    var currentValue = entity.CurrentValues[property.Name];
                    var originalValue = entity.OriginalValues[property.Name];
                    var newValue = ((IDbProcessorAttribute) attribute).PreProcess(dbContext, serviceProvider,
                        entity, currentValue, originalValue, property.PropertyType);
                    if (currentValue != newValue)
                        property.SetValue(entity.Entity, newValue);
                }
            }
        }

        /// <summary>
        /// The post process run just after the DB is saved
        /// </summary>
        /// <param name="dbContext">The parent db context</param>
        /// <param name="serviceProvider">The instance of service provider</param>
        public static void PostProcess(DbContext dbContext, IServiceProvider serviceProvider)
        {
            var entities = dbContext.ChangeTracker.Entries().Where(x =>
                x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

            foreach (var entity in entities)
            foreach (var property in entity.Entity.GetType().GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(IDbProcessorAttribute), true);
                foreach (var attribute in attributes)
                {
                    if (!IsEntityStateSupported(((IDbProcessorAttribute) attribute).State, entity.State))
                        continue;

                    var currentValue = entity.CurrentValues[property.Name];
                    var originalValue = entity.OriginalValues[property.Name];
                    ((IDbProcessorAttribute) attribute).PostProcess(dbContext, serviceProvider, entity,
                        currentValue, originalValue, property.PropertyType);
                }
            }
        }

        private static bool IsEntityStateSupported(DbEntityState dbEntityState, EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Deleted:
                    return dbEntityState.HasFlag(DbEntityState.Deleted);
                case EntityState.Modified:
                    return dbEntityState.HasFlag(DbEntityState.Modified);
                case EntityState.Added:
                    return dbEntityState.HasFlag(DbEntityState.Added);
                default:
                    return false;
            }
        }
    }
}