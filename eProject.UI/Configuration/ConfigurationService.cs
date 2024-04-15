using eProject.DataAccess.Data;

namespace eProject.UI.Configuration
{
    public static class ConfigurationService
    {
        public static void ConfigureRouting(this IApplicationBuilder app, ApplicationDbContext dbContext)
        {
            app.UseEndpoints(endpoints =>
            {
                // Lấy danh sách các route từ cơ sở dữ liệu
                var routes = dbContext.Role.ToList();

                foreach (var route in routes)
                {
                    endpoints.MapControllerRoute(
                        name: route.Name,
                        pattern: route.Name + ".html",
                        defaults: new { controller = "Product", action = "Index", id = route.Id, name = route.Name });
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}");
                }
            });
        }
    }
}
