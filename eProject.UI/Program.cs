using eProject.Insfrastructure.Configuration;
using eProject.UI.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureIdentity(builder.Configuration);

builder.Services.AddDependencyInjection();

builder.Services.AddAutoMapper();
//builder.Services.AddAutoMapper(typeof(eProject.Application.Configuration.AutomapConfig));

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
//app.AutoMigration().GetAwaiter().GetResult();
app.UseAuthorization();
app.UseSession();
app.Use(async (context, next) =>
{
    await next();

    // Nếu không tìm thấy trang
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        // Thiết lập trạng thái response
        context.Response.StatusCode = 404;

        // Gửi thông báo "That page can’t be found."
        context.Response.Redirect("/error.html");
    }
});
app.ConfigureRouting();


app.Run();

