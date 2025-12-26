using Microsoft.EntityFrameworkCore;
using ProximaLite.Domain.Entities;

namespace ProximaLite.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Process> Processes { get; set; }
        public DbSet<Step> Steps => Step<Step>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Process>(entity =>
            {
               entity.ToTable("processes");
                entity.HasKey(p=>p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Description).HasMaxLength(2000);
                entity.Property(p => p.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<Step>(entity =>
            {
                entity.ToTable("steps");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
                entity.Property(s => s.DurationMin).IsRequired();
                entity.Property(s => s.Yield).HasPrecision(5, 4);
                entity.Property(s => s.CostEuro).HasPrecision(12, 2);

               entity.HasOne(s=>s.Process)
                        .WithMany(p=>p.Steps)
                        .HasForeignKey(s=>s.ProcessId)
                        .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}