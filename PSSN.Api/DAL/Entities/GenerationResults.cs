using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSSN.Api.DAL.Entities;

[Table(nameof(GenerationResults), Schema = "scheduled_research")]
public class GenerationResults
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public Guid Guid { get; set; }

    public List<GenerationTreeNode> Tree { get; set; } = [];
    public List<ConditionalStrategy> Strategies { get; set; } = [];
    public Guid GameResultsGuid { get; set; }
}