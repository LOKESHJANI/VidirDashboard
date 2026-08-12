namespace VidirDashboard.Application.DTOs;

public class FaultEventDto
{
    public int MachineId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime OccurredUtc { get; set; }
}