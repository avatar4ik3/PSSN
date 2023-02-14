using PSSN.Common.Model;

namespace PSSN.Common.Responses;

public class SingleGenerationResponse
{
    public GenerationResponseItem? GameResult { get; set; }

    public IEnumerable<FilledStrategyModel>? NewStrats { get; set; }
}

