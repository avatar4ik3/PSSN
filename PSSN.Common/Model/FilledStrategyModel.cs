namespace PSSN.Common.Model;

public class FilledStrategyModel
{
    public string Name { get; set; }

    public Dictionary<int, string> Behaviors { get; set; } = new();
}