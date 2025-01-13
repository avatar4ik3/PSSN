using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSSN.Api.DAL.Entities;

[Table(nameof(Research), Schema = "scheduled_research")]

public class Research
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public Guid Guid { get; set; }
    public int StrategiesCount { get; set; }
    public decimal DistributionStep { get; set; }
    public List<double> Ro { get; set; }
    public int CountOfExperiments { get; set; }
    public int GenCount { get; set; }
    public double SwapChance { get; set; }
    public int CrossingCount { get; set; }
    public int SelectionGroupSize { get; set; }

    public List<GameResults> GameResults { get; set; } = [];
    public int CountOfDistributions { get; set; }
    public int TotalGamesCount { get; set; }
    public ResearchCompletionStatus Status { get; set; }
}

public enum ResearchCompletionStatus
{
    InProgress,
    Dropped,
    Completed
}