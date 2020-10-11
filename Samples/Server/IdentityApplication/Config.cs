using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityApplication
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("scope", new[] {JwtClaimTypes.Scope})
            };
        
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("catalog.read", "Read your articles"),
                new ApiScope("catalog.create", "Create your articles"),
                new ApiScope("catalog.update", "Update your articles"),
                new ApiScope("catalog.delete", "Delete your articles"),

                new ApiScope("cart.read", "Read your articles"),
                new ApiScope("cart.create", "Create your articles"),
                new ApiScope("cart.update", "Update your articles"),
                new ApiScope("cart.delete", "Delete your articles"),
                
                new ApiScope("manage", "Provides administrative access")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("catalog", "Catalog API")
                {
                    Scopes = { "catalog.read", "catalog.create", "catalog.update", "catalog.delete", "manage" }
                },
                new ApiResource("cart", "Cart API")
                {
                    Scopes = { "cart.read", "cart.create", "cart.update", "cart.delete", "manage" }
                }
            };
        
        public static List<TestUser> Users =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "alice",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Scope, "catalog.create"),
                        new Claim(JwtClaimTypes.Scope, "catalog.update"),
                        new Claim(JwtClaimTypes.Scope, "catalog.delete")
                    }
                },
                new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "bob",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Scope, "manage")
                    }
                }
            };
        
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AlwaysIncludeUserClaimsInIdToken = true,
                    //AlwaysSendClientClaims = true,
                        
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    
                    // scopes that client has access to
                    AllowedScopes = {"catalog.read"}
                }
            };
    }
}