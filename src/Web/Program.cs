using Web.Components;
using Microsoft.EntityFrameworkCore;
using VidirDashboard.Application.Interfaces;
using VidirDashboard.Application.Services;
using VidirDashboard.Infrastructure.Csv;
using VidirDashboard.Infrastructure.Persistence;
using VidirDashboard.Infrastructure.Simulation;
using VidirDashboard.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<ICsvExportService, CsvExportService>();
builder.Services.AddScoped<IMachineNotifier, SignalRMachineNotifier>();
builder.Services.AddScoped<MachineQueryService>();

builder.Services.AddHostedService<MachineSimulatorService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
app.MapHub<MachineHub>("/machineHub");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();