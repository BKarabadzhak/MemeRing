using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MemeRing.Contexts;
using MemeRing.Services;
using MemeRing.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MemeRing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDataService, DataService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connection = Configuration.GetConnectionString("Default");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));

            services.AddAutoMapper();

            var key = Configuration.GetSection("Secret").Value;
            var keyBytes = Encoding.UTF8.GetBytes(key);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(x =>
              {
                  x.Events = new JwtBearerEvents
                  {
                      OnTokenValidated = context =>
                      {
                          var userService = context.HttpContext.RequestServices.GetRequiredService<IDataService>();
                          if (Guid.TryParse(context.Principal.Identity.Name, out Guid id))
                          {
                              var user = userService.GetUser(id);
                              if (user == null)
                              {
                                  // return unauthorized if user no longer exists
                                  context.Fail("Unauthorized");
                              }
                              return Task.CompletedTask;
                          }

                          context.Fail("Unauthorized");
                          return Task.CompletedTask;
                      }
                  };
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("MVC didn't find anything!");
            });
        }
    }
}
