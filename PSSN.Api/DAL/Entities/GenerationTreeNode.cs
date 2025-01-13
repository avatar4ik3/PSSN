using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSSN.Api.DAL.Entities;

[Table(nameof(GenerationTreeNode), Schema = "scheduled_research")]
public class GenerationTreeNode
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public Guid Guid { get; set; }
    public ConditionalStrategy Strategy1 { get; set; }
    public ConditionalStrategy Strategy2 { get; set; }
    public List<double> Results { get; set; }
}

