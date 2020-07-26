using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ComplaintServiceAPI.ComplaintServiceAPIContext;

using ComplaintServiceAPI.Services;
using ComplaintServiceAPI.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NLog;

namespace ComplaintServiceAPI
{
    public class Startup
    {
        private IConfiguration configure;
        public Startup(IConfiguration config)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            configure = config;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", opts =>
                {
                   // opts.ApiName = "complaintService";
                    
                    opts.Authority = "https://localhost:5001";
                    opts.Audience = "complaintService";
                    
                    opts.SaveToken = true;
                    opts.RequireHttpsMetadata = false;
                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience =  false
                    };


                });
            services.AddTransient<ModelStateValidators>();
            services.AddTransient<HandleException>();
            services.AddTransient<IBaseRespository, BaseRepository>();
            services.AddTransient<IComplaintService, ComplaintService>();
          //  services.AddTransient<ComplaintDbContext>();
            services.AddTransient<IApiResponse, ApiResponse>();
            services.AddTransient<ILoggerManager, LoggManager>();
            services.AddHttpClient();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}