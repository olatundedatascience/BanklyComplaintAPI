using System.Collections.Generic;
using BankyAuthServer.DTO;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace BankyAuthServer.DataContext
{
    public class BanklyDBContext : IdentityDbContext
    {
        
        
        public BanklyDBContext(DbContextOptions<BanklyDBContext> opts):base(opts)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Data Source = FCMB-IT-L16582\TUNDE;database=BanklyTestApp;User Id=rib_details;Password=adejORbl@q9000");
            }
            
            base.OnConfiguring(optionsBuilder);
        }
        
        public DbSet<UserClientKey> UserClientKeys { get; set; }
    }
    
    public class BanklyContextDesignTimeFactory : IDesignTimeDbContextFactory<BanklyDBContext>
    {
        public BanklyDBContext CreateDbContext(string[] args)
        {
            
            var optionsBuilder = new DbContextOptionsBuilder<BanklyDBContext>();
            optionsBuilder.UseSqlServer(@"Data Source = FCMB-IT-L16582\TUNDE;database=BanklyTestApp;User Id=rib_details;Password=adejORbl@q9000");
            return new BanklyDBContext(optionsBuilder.Options);
        }
    }

    public class BanklyServiceScope
    {
        public static ConfigurationDbContext GetServiceScopeContext(IHttpContextAccessor app)
        {
            using (var serviceScope = app.HttpContext.RequestServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                if (serviceScope != null)
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    return context;
                }
                else
                {
                    return null;
                }
                
            }
        }

        public static List<ApiScope> GetScopes()
        {
            var scopes = new List<ApiScope>()
            {
                new ApiScope("complaintAPI", "complaintAPI"),
                new ApiScope("complaintService", "complaintService")
                
            };

            return scopes;
        }

        
        
    }
}