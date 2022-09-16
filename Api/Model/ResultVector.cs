namespace PSSN.Api.Model;

public sealed class ResultVector
{
    public int Stage { get; set; }
    public Dictionary<Strategy,double> Vector { get; set; }
}