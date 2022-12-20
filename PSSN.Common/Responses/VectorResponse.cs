namespace PSSN.Common.Responses;

public sealed record VectorResponse
{
    public int Ki { get; set; }
    public Dictionary<string, double> Values { get; set; } = new();
}