namespace PSSN.Core.Matrices;

public class PlayerPayoffRoundMap
{
    private Dictionary<Player,double> payoffs {get;init; }= new Dictionary<Player, double>();

    public double this[Player player]{
        get => payoffs[player];
        set => payoffs[player] = value;
    }
}