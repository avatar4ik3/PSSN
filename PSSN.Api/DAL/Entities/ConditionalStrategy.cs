using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSSN.Api.DAL.Entities;

[Table(nameof(ConditionalStrategy), Schema = "scheduled_research")]
public class ConditionalStrategy
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public Guid Guid { get; set; }
    public string PatternName { get; set; }
    public int[] PatternCoefs { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
}