using PSSN.Common.Model;

namespace PSSN.Common.Requests;

/*
    К - количество игр
    R - количество раундов в игре
    Strats - выбранные стратегии
    A - матрица выйгришей
*/
//(int K, int R, List<ConditionalStrategyModel> Strats, double[][] Po)
public class SimpleResearchRequest
{
    public int K { get; set; }
    public int R { get; set; }

    public List<ConditionalStrategyModel> Strats { get; set; } = new();

    public double[][]? Po { get; set; }
}