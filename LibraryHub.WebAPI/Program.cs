using LibraryHub.Common.Configurations;
using LibraryHub.Configurations.DependencyInjection;
using LibraryHub.WebAPI.Logging;
using LibraryHub.WebAPI.Middleware;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.With(new CorrelationIdEnricher(new HttpContextAccessor()))
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLibraryHub(builder.Configuration);
builder.Services.BindOptions<DbOptions>(builder.Configuration, "ConnectionStrings");

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("corto", p => p.Expire(TimeSpan.FromSeconds(60)));
    options.AddPolicy("mediano", p => p.Expire(TimeSpan.FromMinutes(5)));
    options.AddPolicy("largo", p => p.Expire(TimeSpan.FromHours(1)));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

await app.Services.InitializeLibraryHubDatabaseAsync(app.Environment.IsDevelopment());

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar/v1", options =>
    {
        options.Title = "LibraryHub Web API";
        options.Theme = ScalarTheme.Laserwave;
        options.Layout = ScalarLayout.Modern;
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.UseOutputCache();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/scalar/v1"))
   .ExcludeFromDescription();

app.Run();
