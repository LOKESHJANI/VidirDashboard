using VidirDashboard.Application.DTOs;

namespace VidirDashboard.Application.Interfaces;

public interface IMachineNotifier
{
    Task NotifyStatusChangedAsync(MachineStatusDto status);
    Task NotifyFaultAsync(int machineId, string code, string message);
}