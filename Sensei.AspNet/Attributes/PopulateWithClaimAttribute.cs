using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Sensei.AspNet.DbProcessor;

namespace Sensei.AspNet.Attributes
{
    public class PopulateWithClaimAttribute : Attribute, IDbProcessorAttribute
    {
        private readonly string _claimType;
        private readonly bool _skipIfExist;

        public PopulateWithClaimAttribute(DbEntityState state, string claimType, bool skipIfExist = false)
        {
            _claimType = claimType;
            _skipIfExist = skipIfExist;

            State = state;
        }

        public DbEntityState State { get; }

        public object PreProcess(DbContext dbContext, IServiceProvider serviceProvider, EntityEntry entityEntry,
            object currentValue, object originalValue, Type propertyType)
        {
            if (_skipIfExist && currentValue != null && !Guid.Empty.Equals(currentValue))
                return currentValue;

            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            var nameIdentifierClaim =
                httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == _claimType);

            if (nameIdentifierClaim?.Value == null)
                return null;

            return TypeDescriptor.GetConverter(propertyType).ConvertFromInvariantString(nameIdentifierClaim.Value);
        }

        public void PostProcess(DbContext dbContext, IServiceProvider serviceProvider, EntityEntry entityEntry,
            object currentValue, object originalValue, Type propertyType)
        {
        }
    }
}