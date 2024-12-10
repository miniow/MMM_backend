using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<DataPipeline> DataPipelines { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<DataTransformation> DataTransformations { get; set; }
        public DbSet<DataDestination> DataDestinations { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("app");
            modelBuilder.Entity<Workspace>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.UserId)
                      .HasMaxLength(50);
            });
            modelBuilder.Entity<DataPipeline>()
    .HasMany(p => p.Sources)
    .WithOne() // brak nawigacji w DataSource
    .HasForeignKey("DataPipelineId") // "shadow property"
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DataPipeline>()
                .HasMany(p => p.Transforms)
                .WithOne() // brak nawigacji w DataTransformation
                .HasForeignKey("DataPipelineId") // "shadow property"
                .OnDelete(DeleteBehavior.Restrict);
            // Configure DataPipeline relationships
            modelBuilder.Entity<DataPipeline>()
    .HasOne(p => p.Destination)
    .WithOne() // brak nawigacji w DataDestination
    .HasForeignKey<DataDestination>("DataPipelineId") // "shadow property"
    .OnDelete(DeleteBehavior.Restrict);

            // Configure Connection entity
            modelBuilder.Entity<Connection>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(c => c.Source)
                      .WithMany()
                      .HasForeignKey(c => c.SourceId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Destination)
                      .WithMany()
                      .HasForeignKey(c => c.DestinationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
