using Microsoft.AspNetCore.SignalR;
using VidirDashboard.Application.DTOs;
using VidirDashboard.Application.Interfaces;

namespace VidirDashboard.Web.Hubs;

public class SignalRMachineNotifier : IMachineNotifier
{
    private readonly IHubContext<MachineHub> _hubContext;

    public SignalRMachineNotifier(IHubContext<MachineHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyStatusChangedAsync(MachineStatusDto status) =>
        _hubContext.Clients.All.SendAsync("MachineStatusChanged", status);

    public Task NotifyFaultAsync(int machineId, string code, string message) =>
        _hubContext.Clients.All.SendAsync("MachineFaultRaised", machineId, code, message);
}