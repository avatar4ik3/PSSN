namespace PSSN.Common.Model;

public class ResultTree
{
    public Dictionary<string, Dictionary<string, Dictionary<int, double>>> Map { get; set; } = new();
}