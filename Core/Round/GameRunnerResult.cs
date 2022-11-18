using PSSN.Core.Strategies;

namespace PSSN.Core.Round;

public record GameRunnerResult(
    IStrategy S1,
    IStrategy S2,
    double Score1,
    double Score2
);
