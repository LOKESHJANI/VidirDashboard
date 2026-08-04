namespace VidirDashboard.Domain.Entities;

public class MoveCommand
{
    public int Id { get; set; }
    public int MachineId { get; set; }
    public Machine? Machine { get; set; }

    public int SourceBinPosition { get; set; }
    public int DestinationBinPosition { get; set; }
    public DateTime RequestedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedUtc { get; set; }
    public bool Success { get; set; }
}