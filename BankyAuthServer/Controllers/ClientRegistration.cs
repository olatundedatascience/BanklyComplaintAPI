using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankyAuthServer.BankAuthServerConfiguration;
using BankyAuthServer.DataContext;
using BankyAuthServer.DTO;
using BankyAuthServer.Routes;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace BankyAuthServer.Controllers
{
    //[ApiController]
   // [Route("[controller]")]
    public class ClientRegistration : ControllerBase
    {

        private UserManager<IdentityUser> _userManager;
        
        private IHttpContextAccessor _app;
        public ClientRegistration(IHttpContextAccessor _app, UserManager<IdentityUser> userManager)
        {
            this._app = _app;
            this._userManager = userManager;
        }


        [Route(ApiRoutes.registerClient)]
        [HttpPost]
        [ServiceFilter(typeof(HandleException))]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser(userDto.username);
                user.Id = Guid.NewGuid().ToString();
                user.Email = userDto.emailAddress;
                user.PhoneNumber = userDto.phoneNumber;
                var identityResult = await _userManager.CreateAsync(user, userDto.password);

                if (identityResult.Succeeded)
                {
                    // create client id and secret
                    var clientDetails = RegisterClient(userDto, user.Id);
                    if (clientDetails != null)
                    {
                        
                            
                        var data = clientDetails.data;
                       
                        if (data.code != "66")
                        {
                            return Ok(new
                            {
                                responseData = new
                                {
                                    clientId = data.clientId,
                                    clientSecret = data.clientSecret
                                },
                                message ="Success"
                            });
                        }
                        else
                        {
                            return Ok(new
                            {
                                message ="Client details already exist"
                            });
                        }
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            message = "seomthing went wrong",
                            description = "client registration unsuccessfull"
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "seomthing went wrong",
                        description = identityResult.Errors.Select(x=>x.Description)
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    message = "parameter validation failed",
                    description = ModelState.Values.Select(x=>x.Errors.Select(y=>y.ErrorMessage))
                });
            }
            
            
        }
        
       
        public dynamic RegisterClient(UserDto client, string userId)
        {
            
           
                var clientModel = new Client();
                var clientID = Guid.NewGuid().ToString();
                var clientSecret = clientID.ToSha512();
                clientModel.ClientId = clientID;
                clientModel.ClientName = client.clientName;
                clientModel.ClientSecrets = new List<Secret>(){new Secret(clientSecret)};
                clientModel.AllowedGrantTypes = GrantTypes.ClientCredentials;
                clientModel.AllowedScopes = new List<string>(){"complaintService"};
                clientModel.ClientUri = client.clientUri;
                clientModel.Description = client.description;
                //var context = BanklyServiceScope.GetServiceScopeContext(this._app);
                
                using (var serviceScope = this._app.HttpContext.RequestServices.GetService<IServiceScopeFactory>()?.CreateScope())
                {
                    if (serviceScope != null)
                    {
                        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                        var banklyDbContext = serviceScope.ServiceProvider.GetRequiredService<BanklyDBContext>();
                       

                        int countModelFromDB = context.ApiResources.Count();
                        // this is just basic seeding
                        if (countModelFromDB < 1)
                        {
                            //context.ApiResources.AddRange(Config.GetApiResources().Cast<Entities>() .ToArray<ApiResource>());
                            foreach (var apiResouce in Config.GetApiResources())
                            {
                                context.ApiResources.Add(apiResouce.ToEntity());
                            }
                        }

                        context.ApiResources.Add(new ApiResource
                        {
                            Name = "complaintService",
                            DisplayName = "complaintService",
                            Description = "Allow the application to access API #1 on your behalf",
                            Scopes = new List<string> {"complaintService"},
                            ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
                            UserClaims = new List<string> {"role"}
                        }.ToEntity());

                        countModelFromDB = context.IdentityResources.Count();

                        if (countModelFromDB < 1)
                        {
                            foreach (var identityResource in Config.GetIdentityResources())
                            {
                                context.IdentityResources.Add(identityResource.ToEntity());
                            }
                        }
                        countModelFromDB = context.ApiScopes.Count();

                        if (countModelFromDB < 1)
                        {
                            foreach (var apiScope in BanklyServiceScope.GetScopes())
                            {
                                context.ApiScopes.Add(apiScope.ToEntity());
                            }
                        }
                        
                        
                        
                       // context.ApiScopes.Add(new ApiScope("complaintService", "Read Access to API #1").ToEntity());
                       // context.ApiScopes.Add(new ApiScope("complaintAPI", "Write Access to API #1").ToEntity());
                        
                        var isClientExist =
                            context.Clients.FirstOrDefault(x =>
                                x.ClientName.ToLower() == client.clientName.ToLower()) != null;

                        if (!isClientExist)
                        {
                            // check if client already exist
                            context.Clients.Add(clientModel.ToEntity());
                            var result = context.SaveChanges() > 1;
                            
                            var userClientId = new UserClientKey();
                            userClientId.userId = userId;
                            userClientId.claimId = clientID;

                            banklyDbContext.UserClientKeys.Add(userClientId);
                            banklyDbContext.SaveChanges();

                            // to perform is the was actually registered
                           // var obj = context.Clients.FirstOrDefault(x => x.ClientName == client.clientName);

                            if (result)
                            {
                                return new
                                {
                                    data = new
                                    {
                                        code = "00",
                                        clientId = clientID,
                                        clientSecret = clientSecret
                                    }
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return new
                            {
                                data = new
                                {
                                    code = "66" // client already exist
                                    
                                }
                            };
                        }
                       

                    }
                    else
                    {
                        return null;
                    }
                    
                
                }
            
        }

        
    }
}