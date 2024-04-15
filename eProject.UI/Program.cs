using eProject.Application.Abstracts;
using eProject.DataAccess;
using eProject.DataAccess.Data;
using eProject.Insfrastructure.Configuration;
using eProject.UI.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ConfigureIdentity(builder.Configuration);
builder.Services.AddAutoMapper();
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
app.AutoMigration().GetAwaiter().GetResult();
app.UseAuthorization();

// Truy cập ApplicationDbContext từ phạm vi của một scope hợp lệ
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    app.ConfigureRouting(dbContext);
}

app.Run();

