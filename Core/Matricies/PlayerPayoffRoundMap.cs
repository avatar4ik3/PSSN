namespace PSSN.Core.Matrices;

public class PlayerPayoffRoundMap
{
    private Dictionary<Player,int> payoffs {get;init; }= new Dictionary<Player, int>();

    public int this[Player player]{
        get => payoffs[player];
        set => payoffs[player] = value;
    }
}