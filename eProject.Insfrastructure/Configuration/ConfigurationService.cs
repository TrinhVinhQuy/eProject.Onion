﻿using eProject.Application.Abstracts;
using eProject.Application.Services;
using eProject.DataAccess.Data;
using eProject.DataAccess.Repository;
using eProject.Domain.Abstracts;
using eProject.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eProject.Insfrastructure.Configuration
{
    public static class ConfigurationService
    {
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            // Add Services
            services.AddTransient<IRoleServices, RoleServices>();

            // Add RepositoryBase
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>)); ;
        }
        public static void AddAuthorizationGlobal(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/login"; // Đường dẫn đến trang bị từ chối truy cập
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Thời gian hết hạn của cookie là 60 phút
                    options.SlidingExpiration = true; // Kích hoạt thời hạn trượt
                })
                .AddGoogle(googleOptions =>
                {
                    IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");
                    googleOptions.ClientId = googleAuthNSection["ClientId"];
                    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                });
        }

    }
}
