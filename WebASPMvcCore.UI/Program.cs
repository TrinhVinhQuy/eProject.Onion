using Owl.reCAPTCHA;
using WebASPMvcCore.Insfrastructure.Configuration;
using WebASPMvcCore.UI.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureIdentity(builder.Configuration);

builder.Services.AddDependencyInjection();

builder.Services.AddAutoMapper();

builder.Services.AddSignalR();

builder.Services.AddAuthorizationGlobal(builder.Configuration);

builder.Services.AddreCAPTCHAV2(x =>
{
    x.SiteKey = "6LfijO4pAAAAAAzavbWU2SyofdtzWu6dpu0JFafE";
    x.SiteSecret = "6LfijO4pAAAAAGbQ5kd7ew9pZSlCa8wyiDxggOmG";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

await app.AutoMigration();
await app.SeedData(builder.Configuration);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Response.StatusCode = 404;
        context.Response.Redirect("/error.html");
    }
});
app.UseSession();
app.UseRouting();

app.UseAuthorization();
app.ConfigureRouting();

app.Run();
