namespace VidirDashboard.Application.Interfaces;

public interface ICsvExportService
{
    Task<string> ExportBinsToCsvAsync(int machineId);
    Task ImportBinsFromCsvAsync(int machineId, Stream csvStream);
}