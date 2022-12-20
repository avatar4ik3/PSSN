using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Core.States;

public interface IPlayerState
{
    public Player p { get; init; }
    public IStrategy strategy { get; set; }
    public List<Behavior> previousBehaviours { get; set; }
    public Behavior currentBehaviour { get; set; }
    public Dictionary<int, double> Scores { get; set; }

    public double TotalScore { get; set; }
    public bool Next(Game g);
}