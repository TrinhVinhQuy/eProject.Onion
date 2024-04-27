
using eProject.Insfrastructure.Services;

namespace eProject.UI.Configuration
{
    public static class ConfigurationService
    {
        public static void ConfigureRouting(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}");
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "/login",
                    defaults: new { controller = "Login", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "user",
                    pattern: "/thong-tin-nguoi-dung",
                    defaults: new { controller = "User", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "error",
                    pattern: "/error.html",
                    defaults: new { controller = "Home", action = "Error" });
                endpoints.MapControllerRoute(
                    name: "Cart",
                    pattern: "/gio-hang",
                    defaults: new { controller = "Cart", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "posts",
                    pattern: "/{posts}.html",
                    defaults: new { controller = "Posts", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "danh-muc-{url}",
                    pattern: "/danh-muc-{url}",
                    defaults: new { controller = "Category", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "product",
                    pattern: "/{product}",
                    defaults: new { controller = "Product", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
