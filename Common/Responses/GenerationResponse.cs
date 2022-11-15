using PSSN.Core.Strategies;

namespace PSSN.Common;

public class GenerationResponse
{
    public List<Item> items = new();
}

public class Item
{
    public List<FilledStrategy> strats = new();
    public IEnumerable<VectorResponse> response;
}