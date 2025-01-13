using Microsoft.EntityFrameworkCore;
using PSSN.Api.DAL.Entities;

namespace PSSN.Api.DAL;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Research> Researches { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}