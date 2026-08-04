namespace VidirDashboard.Domain.Entities;

public class Bin
{
    public int Id { get; set; }
    public int MachineId { get; set; }
    public Machine? Machine { get; set; }

    public int Position { get; set; }
    public bool Occupied { get; set; }
    public string? SkuLabel { get; set; }
}