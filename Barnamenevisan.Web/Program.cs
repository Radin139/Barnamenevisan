#region Pipeline

using Barnamenevisan.Data.Context;
using Barnamenevisan.IoC;
using Barnamenevisan.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

#region DbContext

string connectionString = builder.Configuration.GetConnectionString("BarnamenevisanDbContext") ?? String.Empty;

builder.Services.AddDbContext<BarnamenevisanDbContext>(options => options.UseSqlServer(connectionString));

#endregion

builder.Services.AddServices();

#region Authentication

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
    });

#endregion

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


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AdminMiddleware>();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllerRoute(
//         name : "areas",
//         pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//     );
// });

app.Run();

#endregion