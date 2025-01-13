using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSSN.Api.DAL.Entities;

[Table(nameof(GameResults), Schema = "scheduled_research")]
public class GameResults
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public Guid Guid { get; set; }
    public int CurrentExperiment { get; set; }
    public decimal CurrentDistribution { get; set; }
    public int CurrentGeneration { get; set; }

    public GenerationResults GenerationResults { get; set; }
}