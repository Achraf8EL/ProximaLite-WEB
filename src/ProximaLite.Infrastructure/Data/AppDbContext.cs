using Microsoft.EntityFrameworkCore;
using ProximaLite.Domain.Entities;

namespace ProximaLite.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Process> Processes => Set<Process>();
    public DbSet<Step> Steps => Set<Step>();

    public DbSet<ProcessEvaluation> ProcessEvaluations => Set<ProcessEvaluation>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Process>(entity =>
        {
            entity.ToTable("processes");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasMaxLength(2000);

            entity.Property(p => p.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.ToTable("steps");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(s => s.DurationMin).IsRequired();

            entity.Property(s => s.Yield)
                .HasPrecision(5, 4);

            entity.Property(s => s.CostEuro)
                .HasPrecision(12, 2);

            entity.HasOne(s => s.Process)
                .WithMany(p => p.Steps)
                .HasForeignKey(s => s.ProcessId)
                .OnDelete(DeleteBehavior.Cascade);
        });
modelBuilder.Entity<ProcessEvaluation>(entity =>
{
    entity.ToTable("process_evaluations");

    entity.HasKey(e => e.Id);

    entity.Property(e => e.CreatedAt).IsRequired();

    entity.Property(e => e.TotalDurationMin).IsRequired();

    entity.Property(e => e.TotalCostEuro)
        .HasPrecision(12, 2)
        .IsRequired();

    entity.Property(e => e.GlobalYield)
        .HasPrecision(6, 5) 
        .IsRequired();

    entity.Property(e => e.Notes)
        .HasMaxLength(400);

    entity.HasOne(e => e.Process)
        .WithMany(p => p.Evaluations)
        .HasForeignKey(e => e.ProcessId)
        .OnDelete(DeleteBehavior.Cascade);
});


    }
}
