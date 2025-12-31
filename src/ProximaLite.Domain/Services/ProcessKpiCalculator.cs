using ProximaLite.Domain.Entities;

namespace ProximaLite.Domain.Services
{
   public static class ProcessKpiCalculator
   {
      public static ProcessKpiResult Calculate(Process process)
        {
            var totalDuration = process.Steps.Sum(step => step.DurationMin);
            var totalCost = process.Steps.Sum(step => step.CostEuro);

            decimal globalYeild = 1m;
            foreach (var s in process.Steps)
            {
                globalYeild *= s.Yield;
            }

                return new ProcessKpiResult(totalDuration, totalCost, globalYeild);
        }
   }
}

public record ProcessKpiResult(int TotalDurationMin, decimal TotalCostEuro, decimal GlobalYeild);