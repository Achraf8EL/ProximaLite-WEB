namespace ProximaLite.Domain.Entities;

public class Step
{
    public int Id { get; set; }

    public int ProcessId { get; set; }

    public Process? Process { get; set; }

    public string Name { get; set; } = string.Empty;

    public int DurationMin { get; set; }

    public decimal Yield { get; set; }

    public decimal CostEuro { get; set; }
}
