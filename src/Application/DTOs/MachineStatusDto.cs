using VidirDashboard.Domain.Enums;

namespace VidirDashboard.Application.DTOs;

public class MachineStatusDto
{
    public int MachineId { get; set; }
    public string Name { get; set; } = string.Empty;
    public MachineStatus Status { get; set; }
    public DateTime LastUpdatedUtc { get; set; }
    public List<BinDto> Bins { get; set; } = new();
}

public class BinDto
{
    public int Position { get; set; }
    public bool Occupied { get; set; }
    public string? SkuLabel { get; set; }
}