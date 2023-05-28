using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Models;
using MVCMovie.Data;
using System.Configuration;
using WebAppMVC.Models;
using WebAppMVC.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MVCMovie.Data.RestaurantContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantContext") ?? throw new InvalidOperationException("Connection string 'RestaurantContext' not found.")));
builder.Services.AddRepository(builder.Configuration.GetConnectionString("RestaurantContext"));
builder.Services.AddDefaultIdentity<Users>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<RestaurantContext>()
     .AddDefaultTokenProviders();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
