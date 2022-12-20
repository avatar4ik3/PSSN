namespace PSSN.Common.Requests;

public class GenerationRequest
{
    public double[][] Ro { get; set; }
    public int GenCount { get; set; }
    public int Population { get; set; }
    public double SwapChance { get; set; }
    public int CrossingCount { get; set; }
    public int SelectionGoupSize { get; set; }
}