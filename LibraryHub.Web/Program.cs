using LibraryHub.Bussiness.Interfaces;
using LibraryHub.Bussiness.Services;
using LibraryHub.Common.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddOptions();
builder.Services.Configure<ApiClientOptions>(builder.Configuration.GetSection("ApiClient"));
builder.Services.AddHttpClient<IHttpApiService, HttpApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "libraryhub-spa",
    pattern: "LibraryHub/{*path}",
    defaults: new { controller = "Home", action = "LibraryHub" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "libaryhub-spa-legacy",
    pattern: "LibaryHub/{*path}",
    defaults: new { controller = "Home", action = "LibraryHub" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
