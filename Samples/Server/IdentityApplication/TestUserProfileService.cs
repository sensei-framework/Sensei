using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Logging;

namespace IdentityApplication
{
    public class ProfileService : TestUserProfileService
    {
        public ProfileService(TestUserStore users, ILogger<TestUserProfileService> logger) 
            : base(users, logger)
        { }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims;
            context.IssuedClaims.AddRange(claims);
            return base.GetProfileDataAsync(context);
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            return base.IsActiveAsync(context);
        }
    }
}