namespace VidirDashboard.Domain.Entities;

public class FaultEvent
{
    public int Id { get; set; }
    public int MachineId { get; set; }
    public Machine? Machine { get; set; }

    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime OccurredUtc { get; set; } = DateTime.UtcNow;
}