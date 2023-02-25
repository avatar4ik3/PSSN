using PSSN.Common.Model;

namespace PSSN.Common.Responses;

public class GenerationResponse
{
    public List<GenerationResponseItem> Items { get; set; } = new();
}

public class GenerationResponseItem
{
    public GenerationResponseItem(List<ConditionalStrategyModel> strats, ResultTree result)
    {
        Strats = strats;
        Result = result;
    }

    public GenerationResponseItem()
    {
    }

    public List<ConditionalStrategyModel> Strats { get; set; } = new();

    public ResultTree Result { get; set; } = new();
}