using PSSN.Core.Strategies;

namespace PSSN.Common;

public class GenerationRequest
{
    public double[][] Ro { get; set; }

    public int k { get; set; }
    public int K { get; set; }
    public int R { get; set; }
    public int GenCount { get; set; }
    public int Population { get; set; }
    public double SwapChance { get; set; }
    public int CrossingCount { get; set; }
}