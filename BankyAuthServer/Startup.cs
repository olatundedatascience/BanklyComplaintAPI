using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BankyAuthServer.BankAuthServerConfiguration;
using BankyAuthServer.DataContext;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankyAuthServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var db_con_string = @"Data Source = FCMB-IT-L16582\TUNDE;database=BanklyTestApp;User Id=rib_details;Password=adejORbl@q9000";
            var migrationAssemly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<BanklyDBContext>(opts =>
            {
                opts.UseSqlServer(db_con_string, sqloptions =>
                {
                    sqloptions.MigrationsAssembly(migrationAssemly);
                });
            });

            IIdentityServerBuilder idsBuilder = services.AddIdentityServer().AddDeveloperSigningCredential();

            idsBuilder.AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(db_con_string, sqloptions =>
                        {
                            sqloptions.MigrationsAssembly(migrationAssemly);
                        });
                    };
                })
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(db_con_string, sqloptions =>
                        {
                            sqloptions.MigrationsAssembly(migrationAssemly);
                        });
                    };
                });

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BanklyDBContext>()
                .AddDefaultTokenProviders();
            idsBuilder.AddAspNetIdentity<IdentityUser>();
            
            /*
            services.AddIdentityServer()
                //.AddAspNetIdentity<IdentityUser>()
                .AddTestUsers(Config.GeTestUsers())
                /*
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddTestUsers(Config.GeTestUsers())
                
               .AddOperationalStore(opt =>
               {
                   opt.ConfigureDbContext = builder =>
                   {
                       builder.UseSqlServer(db_con_string, sqloptions =>
                       {
                           sqloptions.MigrationsAssembly(migrationAssemly);
                       });
                   };
               })
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(db_con_string, sqloptions =>
                        {
                            sqloptions.MigrationsAssembly(migrationAssemly);
                        });
                    };
                }).AddDeveloperSigningCredential();
            */
           
           
            services.AddTransient<HandleException>();
           // services.AddTransient<IApplicationBuilder>();

           services.AddHttpContextAccessor();
           services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                app.UseRouting();
                app.UseIdentityServer();
                
                app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
              endpoints.MapDefaultControllerRoute();
               endpoints.MapControllers();
            });
        }
    }
}