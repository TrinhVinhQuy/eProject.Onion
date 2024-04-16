using eProject.Insfrastructure.Configuration;
using eProject.UI.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureIdentity(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDependencyInjection();

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

app.ConfigureRouting();
app.Use(async (context, next) =>
{
    await next();

    // Nếu không tìm thấy trang
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        // Thiết lập trạng thái response
        context.Response.StatusCode = 404;

        // Gửi thông báo "That page can’t be found."
        context.Response.Redirect("/Home/Error");
    }
});

app.Run();

