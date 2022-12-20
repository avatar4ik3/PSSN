using PSSN.Common.Model;

namespace PSSN.Common.Responses;

public class GenerationResponse
{
    public List<Item> Items { get; set; } = new();
}

public class Item
{
    public Item(List<FilledStrategyModel> strats, ResultTree result)
    {
        Strats = strats;
        Result = result;
    }

    public Item()
    {
    }

    public List<FilledStrategyModel> Strats { get; set; } = new();

    public ResultTree Result { get; set; } = new();
}