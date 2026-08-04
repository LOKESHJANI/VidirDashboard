using System.Text;
using Microsoft.EntityFrameworkCore;
using VidirDashboard.Application.Interfaces;
using VidirDashboard.Infrastructure.Persistence;

namespace VidirDashboard.Infrastructure.Csv;

public class CsvExportService : ICsvExportService
{
    private readonly AppDbContext _context;

    public CsvExportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> ExportBinsToCsvAsync(int machineId)
    {
        var bins = await _context.Bins
            .Where(b => b.MachineId == machineId)
            .OrderBy(b => b.Position)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Position,Occupied,SkuLabel");

        foreach (var bin in bins)
        {
            sb.AppendLine($"{bin.Position},{bin.Occupied},{bin.SkuLabel}");
        }

        return sb.ToString();
    }

    public async Task ImportBinsFromCsvAsync(int machineId, Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);

        string? line = await reader.ReadLineAsync(); // header
        while ((line = await reader.ReadLineAsync()) is not null)
        {
            var parts = line.Split(',');
            if (parts.Length < 3) continue;

            int position = int.Parse(parts[0]);
            bool occupied = bool.Parse(parts[1]);
            string? sku = string.IsNullOrWhiteSpace(parts[2]) ? null : parts[2];

            var bin = await _context.Bins
                .FirstOrDefaultAsync(b => b.MachineId == machineId && b.Position == position);

            if (bin is null)
            {
                _context.Bins.Add(new Domain.Entities.Bin
                {
                    MachineId = machineId,
                    Position = position,
                    Occupied = occupied,
                    SkuLabel = sku
                });
            }
            else
            {
                bin.Occupied = occupied;
                bin.SkuLabel = sku;
            }
        }

        await _context.SaveChangesAsync();
    }
}