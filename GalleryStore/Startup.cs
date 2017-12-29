using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using GalleryStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GalleryStore.Services;
using Newtonsoft.Json;
using AutoMapper;
using GalleryStore.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace GalleryStore
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<GalleryStoreContext>();

            services.AddDbContext<GalleryStoreContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("GalleryStoreConnectionString"));
            });

            services.AddAutoMapper();
            services.AddTransient<IMailService, NullMailService>();
            services.AddTransient<GalleryStoreSeeder>();
            services.AddScoped<IGalleryStoreRepository, GalleryStoreRepository>();
            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", Action = "Index" });
            });

            if (env.IsDevelopment())
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<GalleryStoreSeeder>();
                    seeder.Seed().Wait();
                }

            }
        }
    }
}
