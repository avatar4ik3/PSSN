namespace PSSN.Common;

public sealed record VectorResponse{
    public int Ki {get;set;}
    public Dictionary<string,double> Values { get; set; } = new Dictionary<string, double>();
}