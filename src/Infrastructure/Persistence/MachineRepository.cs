using Microsoft.EntityFrameworkCore;
using VidirDashboard.Application.Interfaces;
using VidirDashboard.Domain.Entities;

namespace VidirDashboard.Infrastructure.Persistence;

public class MachineRepository : IMachineRepository
{
    private readonly AppDbContext _context;

    public MachineRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Machine?> GetByIdAsync(int id) =>
        _context.Machines.Include(m => m.Bins)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<List<Machine>> GetAllAsync() =>
        await _context.Machines.Include(m => m.Bins).ToListAsync();

    public async Task UpdateBinAsync(int machineId, int position, bool occupied, string? skuLabel)
    {
        var bin = await _context.Bins
            .FirstOrDefaultAsync(b => b.MachineId == machineId && b.Position == position);

        if (bin is null)
        {
            _context.Bins.Add(new Bin
            {
                MachineId = machineId,
                Position = position,
                Occupied = occupied,
                SkuLabel = skuLabel
            });
        }
        else
        {
            bin.Occupied = occupied;
            bin.SkuLabel = skuLabel;
        }

        var machine = await _context.Machines.FindAsync(machineId);
        if (machine is not null)
        {
            machine.LastUpdatedUtc = DateTime.UtcNow;
        }
    }

    public async Task AddFaultAsync(int machineId, string code, string message)
    {
        _context.FaultEvents.Add(new FaultEvent
        {
            MachineId = machineId,
            Code = code,
            Message = message
        });

        var machine = await _context.Machines.FindAsync(machineId);
        if (machine is not null)
        {
            machine.Status = Domain.Enums.MachineStatus.Fault;
            machine.LastUpdatedUtc = DateTime.UtcNow;
        }
    }

    public async Task AddMoveCommandAsync(MoveCommand command)
    {
        _context.MoveCommands.Add(command);
        await Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}