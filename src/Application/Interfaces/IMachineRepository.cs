using VidirDashboard.Domain.Entities;

namespace VidirDashboard.Application.Interfaces;

public interface IMachineRepository
{
    Task<Machine?> GetByIdAsync(int id);
    Task<List<Machine>> GetAllAsync();
    Task UpdateBinAsync(int machineId, int position, bool occupied, string? skuLabel);
    Task AddFaultAsync(int machineId, string code, string message);
    Task AddMoveCommandAsync(MoveCommand command);
    Task SaveChangesAsync();
}