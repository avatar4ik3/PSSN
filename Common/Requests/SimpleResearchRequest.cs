namespace PSSN.Common.Requests;

/*
    К - количество игр
    R - количество раундов в игре
    Strats - выбранные стратегии
    A - матрица выйгришей
*/
public record SimpleResearchRequest(int K,int R, IEnumerable<string> Strats,int[][] A)
{
    
}