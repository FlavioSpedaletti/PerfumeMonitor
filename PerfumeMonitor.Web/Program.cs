using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Web.Services;
using PerfumeMonitor.Web.Middleware;
using PerfumeMonitor.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddLogging();

builder.Services.AddScoped<IConfigManager, ConfigManager>();
builder.Services.AddScoped<IPerfumeChecker, PerfumeChecker>();
builder.Services.AddScoped<IWhatsAppNotifier, WhatsAppNotifier>();
builder.Services.AddSingleton<IMonitoringStatusService, MonitoringStatusService>();

builder.Services.AddHostedService<PerfumeMonitoringService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors();
app.UseCacheControl();
app.UseRouting();

app.MapControllers();

// Versioning para cache busting
var appVersion = DateTime.UtcNow.Ticks.ToString();

app.MapGet("/", () => Results.Redirect($"/status.html?v={appVersion}"));

app.MapGet("/admin", () => Results.Redirect($"/admin.html?v={appVersion}"));

app.MapGet("/version", () => new { version = appVersion, timestamp = DateTime.UtcNow });

app.UseStaticFiles();

app.Run();
