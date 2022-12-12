namespace PSSN.Common.Model;

public class FilledStrategyModel
{
    public String Name { get; set; }

    public Dictionary<int, String> Behaviors { get; set; } = new();
}