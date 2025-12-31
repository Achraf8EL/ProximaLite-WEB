using System;
using System.Collections.Generic;

namespace ProximaLite.Domain.Entities;

public class Process
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Step> Steps { get; set; } = new();

    public List<ProcessEvaluation> Evaluations { get; set; } = new();

}
