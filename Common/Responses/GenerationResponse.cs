using PSSN.Common.Model;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Common;

public class GenerationResponse
{
    public List<Item> Items { get; set; } = new();
}

public class Item
{
    public List<FilledStrategyModel> Strats { get; set; } = new();

    public ResultTree Result { get; set; } = new();

    public Item(List<FilledStrategyModel> strats, ResultTree result)
    {
        this.Strats = strats;
        this.Result = result;
    }

    public Item()
    {

    }
}