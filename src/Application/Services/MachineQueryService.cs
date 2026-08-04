using VidirDashboard.Application.DTOs;
using VidirDashboard.Application.Interfaces;

namespace VidirDashboard.Application.Services;

public class MachineQueryService
{
    private readonly IMachineRepository _repository;

    public MachineQueryService(IMachineRepository repository)
    {
        _repository = repository;
    }

    public async Task<MachineStatusDto?> GetMachineStatusAsync(int machineId)
    {
        var machine = await _repository.GetByIdAsync(machineId);
        if (machine is null) return null;

        return new MachineStatusDto
        {
            MachineId = machine.Id,
            Name = machine.Name,
            Status = machine.Status,
            LastUpdatedUtc = machine.LastUpdatedUtc,
            Bins = machine.Bins.Select(b => new BinDto
            {
                Position = b.Position,
                Occupied = b.Occupied,
                SkuLabel = b.SkuLabel
            }).ToList()
        };
    }

    public async Task<List<MachineStatusDto>> GetAllMachineStatusesAsync()
    {
        var machines = await _repository.GetAllAsync();

        return machines.Select(machine => new MachineStatusDto
        {
            MachineId = machine.Id,
            Name = machine.Name,
            Status = machine.Status,
            LastUpdatedUtc = machine.LastUpdatedUtc,
            Bins = machine.Bins.Select(b => new BinDto
            {
                Position = b.Position,
                Occupied = b.Occupied,
                SkuLabel = b.SkuLabel
            }).ToList()
        }).ToList();
    }
}