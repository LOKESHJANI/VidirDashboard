using VidirDashboard.Domain.Enums;

namespace VidirDashboard.Domain.Entities;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MachineStatus Status { get; set; } = MachineStatus.Idle;
    public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;

    public List<Bin> Bins { get; set; } = new();
    public List<MoveCommand> MoveCommands { get; set; } = new();
    public List<FaultEvent> FaultEvents { get; set; } = new();
}