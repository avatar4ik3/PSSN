using PSSN.Common.Model;

namespace PSSN.Common.Requests;

public class SingleGenerationRequest
{
    public List<FilledStrategyModel> Strats { get; set; } = new();
    public double[][]? Ro { get; set; }
    public int GenCount { get; set; }
    public double SwapChance { get; set; }
    public int CrossingCount { get; set; }
    public int SelectionGoupSize { get; set; }

}