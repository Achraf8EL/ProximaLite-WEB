using System;

namespace ProximaLite.Domain.Entities;

public class ProcessEvaluation
{
    public int Id { get; set; }

    public int ProcessId { get; set; }
    public Process? Process { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TotalDurationMin { get; set; }
    public decimal TotalCostEuro { get; set; }
    public decimal GlobalYield { get; set; }

    public string? Notes { get; set; }
}
