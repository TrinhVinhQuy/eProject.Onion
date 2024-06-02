using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebASPMvcCore.Application.Abstracts;
using WebASPMvcCore.Application.Services;
using WebASPMvcCore.DataAccess.Dapper;
using WebASPMvcCore.DataAccess.Data;
using WebASPMvcCore.DataAccess.Repository;
using WebASPMvcCore.Domain.Abstracts;
using WebASPMvcCore.Domain.Entities;
using WebASPMvcCore.Domain.Setting;
using WebASPMvcCore.Insfrastructure.Abstracts;
using WebASPMvcCore.Insfrastructure.Services;

namespace WebASPMvcCore.Insfrastructure.Configuration
{
    public static class ConfigurationService
    {
        public static async Task AutoMigration(this WebApplication webApplication)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await appContext.Database.MigrateAsync();
            }
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "CoffeeShopCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/login";
                options.SlidingExpiration = true;
                //options.AccessDeniedPath = "/";
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });
        }
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<ISQLQueryHandler, SQLQueryHandler>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddTransient<PasswordHasher<ApplicationUser>>();

            services.AddTransient<IHelperService, HelperService>();
        }
        public static void AddAuthorizationGlobal(this IServiceCollection services, IConfiguration configuration)
        {
            var autherizeAdmin = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser().Build();

            services.AddAuthentication()
                   .AddGoogle(options =>
                   {
                       IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");
                       options.ClientId = googleAuthNSection["ClientId"];
                       options.ClientSecret = googleAuthNSection["ClientSecret"];
                   });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizedAdminPolicy", autherizeAdmin);
            });
        }
        public static async Task SeedData(this WebApplication webApplication, IConfiguration configuration)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var defaultUser = configuration.GetSection("DefaultUsers")?.Get<DefaultUser>() ?? new DefaultUser();
                var defaultRole = configuration.GetValue<string>("DefaultRole") ?? "Admin";

                try
                {
                    if (!await roleManager.RoleExistsAsync(defaultRole))
                    {
                        await roleManager.CreateAsync(new IdentityRole(defaultRole));
                    }

                    var existUser = await userManager.FindByNameAsync(defaultUser.Username);

                    if (existUser == null)
                    {
                        var user = new ApplicationUser
                        {
                            MobilePhone = defaultUser.MobilePhone,
                            UserName = defaultUser.Username,
                            Fullname = defaultUser.Fullname,
                            Email = defaultUser.Email,
                            EmailConfirmed = true,
                            NormalizedEmail = defaultUser.Email.ToUpper(),
                            AccessFailedCount = 0,
                            IsActive = true,
                        };

                        var identityUser = await userManager.CreateAsync(user, defaultUser.Password);

                        if (identityUser.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, defaultRole);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
