using Microsoft.EntityFrameworkCore;
using PSSN.Api.DAL.Entities;

namespace PSSN.Api.DAL;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Research> Researches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Research>()
            .HasMany<GameResults>(x => x.GameResults)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GameResults>()
            .HasOne<GenerationResults>(x => x.GenerationResults)
            .WithOne()
            .HasForeignKey<GenerationResults>(x => x.GameResultsGuid)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GenerationResults>()
            .HasMany<GenerationTreeNode>(x => x.Tree)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GenerationResults>()
            .HasMany<ConditionalStrategy>(x => x.Strategies)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GenerationTreeNode>()
            .HasOne<ConditionalStrategy>(x => x.Strategy1)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<GenerationTreeNode>()
            .HasOne<ConditionalStrategy>(x => x.Strategy2)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


        base.OnModelCreating(modelBuilder);
    }
}