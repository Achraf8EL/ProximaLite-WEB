namespace ProximaLite.Domain.Entities
{
    public class Step
    {
        public int Id {get;set;}
        public int ProcessId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int DurationMin { get; set; }

        public decimal Yield { get; set; } // Represented as a percentage (e.g., 95.5 for 95.5%)

        public decimal CostEuro { get; set; } // Cost associated with this step


    }
}