using ProximaLite.Domain.Entities;
using ProximaLite.Domain.Services;
using Xunit;

namespace ProximaLite.Tests;

public class ProcessKpiCalculatorTests
{
    [Fact]
    public void Calculate_ShouldReturnSumsAndProduct()
    {
        var process = new Process
        {
            Name = "Test",
            Steps =
            {
                new Step { Name = "S1", DurationMin = 5, CostEuro = 100m, Yield = 1.0m },
                new Step { Name = "S2", DurationMin = 2, CostEuro = 200m, Yield = 1.0m },
                new Step { Name = "S3", DurationMin = 10, CostEuro = 500m, Yield = 0.5m },
            }
        };

        var result = ProcessKpiCalculator.Calculate(process);

        Assert.Equal(17, result.TotalDurationMin);
        Assert.Equal(800m, result.TotalCostEuro);
        Assert.Equal(0.5m, result.GlobalYeild);
    }

    [Fact]
    public void Calculate_WithNoSteps_ShouldReturnZerosAndYield1()
    {
        var process = new Process { Name = "Empty" };

        var result = ProcessKpiCalculator.Calculate(process);

        Assert.Equal(0, result.TotalDurationMin);
        Assert.Equal(0m, result.TotalCostEuro);
        Assert.Equal(1m, result.GlobalYeild);
    }

    [Fact]
    public void Calculate_ShouldHandleDecimalsPrecisely()
    {
        var process = new Process
        {
            Name = "Decimal",
            Steps =
            {
                new Step { Name="A", DurationMin = 1, CostEuro = 0.10m, Yield = 0.90m },
                new Step { Name="B", DurationMin = 1, CostEuro = 0.20m, Yield = 0.95m },
            }
        };

        var result = ProcessKpiCalculator.Calculate(process);

        Assert.Equal(2, result.TotalDurationMin);
        Assert.Equal(0.30m, result.TotalCostEuro);
        Assert.Equal(0.855m, result.GlobalYeild); 
    }
}
