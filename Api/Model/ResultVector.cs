using PSSN.Core.Strategies;

namespace PSSN.Api.Model;

public sealed class ResultVector
{
    public int Stage { get; set; }
    public Dictionary<IStrategy, double> Vector { get; set; }
}