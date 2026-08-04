using VidirDashboard.Domain.Entities;

namespace VidirDashboard.Infrastructure.Persistence;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Machines.Any()) return;

        var machine = new Machine
        {
            Id = 1,
            Name = "ASRS Lift Unit 1",
            Status = Domain.Enums.MachineStatus.Idle
        };

        for (int i = 1; i <= 8; i++)
        {
            machine.Bins.Add(new Bin { Position = i, Occupied = false });
        }

        context.Machines.Add(machine);
        context.SaveChanges();
    }
}