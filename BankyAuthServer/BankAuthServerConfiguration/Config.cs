using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace BankyAuthServer.BankAuthServerConfiguration
{
    public static class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "oauthClient",
                    ClientName = "Example client application using client credentials",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                    AllowedScopes = new List<string> {"api1.read", "customAPI"}
                }
            };
        }
        
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }, new IdentityResource("ApiRead", new List<string>{"complaintAPI", "complaintService"}), 
            };
        }
        
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "BanklyComplaintAPI",
                    DisplayName = "API #1",
                    Description = "Allow the application to access API #1 on your behalf",
                    Scopes = new List<string> {"complaintAPI", "complaintService"},
                    ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                    UserClaims = new List<string> {"role"}
                }
            };
        }
        
        
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope("complaintService", "Read Access to API #1"),
                new ApiScope("complaintAPI", "Write Access to API #1")
            };
        }
        
        public static List<TestUser> GeTestUsers()
        {
            return new List<TestUser> {
                new TestUser {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "scott",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                }
            };
        }
    }
}