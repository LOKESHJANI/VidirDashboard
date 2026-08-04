using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VidirDashboard.Application.Interfaces;
using VidirDashboard.Application.Services;

namespace VidirDashboard.Infrastructure.Simulation;

public class MachineSimulatorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MachineSimulatorService> _logger;
    private readonly Random _random = new();
    private const int MachineId = 1;
    private const int BinCount = 8;

    public MachineSimulatorService(
        IServiceScopeFactory scopeFactory,
        ILogger<MachineSimulatorService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SimulateTickAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Simulator tick failed");
            }

            await Task.Delay(TimeSpan.FromSeconds(4), stoppingToken);
        }
    }

    private async Task SimulateTickAsync()
    {
        using var scope = _scopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IMachineRepository>();
        var notifier = scope.ServiceProvider.GetRequiredService<IMachineNotifier>();
        var queryService = scope.ServiceProvider.GetRequiredService<MachineQueryService>();

        int position = _random.Next(1, BinCount + 1);
        bool occupied = _random.Next(0, 2) == 1;
        string? sku = occupied ? $"SKU-{_random.Next(1000, 9999)}" : null;

        await repository.UpdateBinAsync(MachineId, position, occupied, sku);

        bool raiseFault = _random.Next(0, 20) == 0;
        if (raiseFault)
        {
            var code = $"F{_random.Next(100, 999)}";
            await repository.AddFaultAsync(MachineId, code, "Simulated position sensor timeout");
            await notifier.NotifyFaultAsync(MachineId, code, "Simulated position sensor timeout");
            _logger.LogWarning("Simulated fault {Code} raised on machine {MachineId}", code, MachineId);
        }

        await repository.SaveChangesAsync();

        var status = await queryService.GetMachineStatusAsync(MachineId);
        if (status is not null)
        {
            await notifier.NotifyStatusChangedAsync(status);
        }
    }
}