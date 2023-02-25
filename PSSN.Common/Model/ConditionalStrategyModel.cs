namespace PSSN.Common.Model;

public class ConditionalStrategyModel
{
    public List<PatternModel> Patterns { get; set; } = new();

    public Dictionary<int, string>? Behaviors { get; set; } = new();
    public string? Name { get; set; }
}

public class PatternModel
{
    public string? Name { get; set; }

    public int[]? Coeffs { get; set; }
}