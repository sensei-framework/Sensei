using System;
using System.Collections.Generic;
using System.Linq;
using Sensei.AspNet.Attributes;
using Sensei.AspNet.DbProcessor;
using Swashbuckle.AspNetCore.Annotations;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Sensei.AspNet.Models
{
    /// <summary>
    ///     Provide an abstract base model for data
    /// </summary>
    public abstract class BaseModel
    {
        private static readonly List<string> DenyList = new List<string>
        {
            "Id",
            "CreatedAt",
            "UpdatedAt"
        };

        /// <summary>
        ///     The record id. This is the unique identifier for the record
        /// </summary>
        [SwaggerSchema("The record id", ReadOnly = true)]
        public Guid Id { get; set; }

        /// <summary>
        ///     The record creation time
        /// </summary>
        [SwaggerSchema("The creation date-time", ReadOnly = true)]
        [PopulateWithDateTime(DbEntityState.Added)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     The record last update time
        /// </summary>
        [SwaggerSchema("The last update date-time", ReadOnly = true)]
        [PopulateWithDateTime(DbEntityState.Added | DbEntityState.Modified)]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        ///     Copy the object fields into a destination object. Fields can be filtered if necessary
        /// </summary>
        /// <param name="destination">The destination object where the fields will be copied to</param>
        /// <param name="allowList">A list of fields allowed to the copy. If omitted all the fields will be copied</param>
        public void CopyTo(object destination, List<string> allowList = null)
        {
            var sourceProps = GetType().GetProperties().Where(x => x.CanRead).ToList();
            var destProps = destination.GetType().GetProperties().Where(x => x.CanWrite).ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (allowList != null && !allowList.Contains(sourceProp.Name))
                    continue;

                if (DenyList.Contains(sourceProp.Name))
                    continue;

                var destProp = destProps.FirstOrDefault(x => x.Name == sourceProp.Name);
                if (destProp == null)
                    continue;

                destProp.SetValue(destination, sourceProp.GetValue(this));
            }
        }
    }
}