using PSSN.Core.States;

namespace PSSN.Core.Matrices;

public class PlayerPayoffRoundMap
{
    private readonly double[][] matrix;

    private Dictionary<Player, double> payoffs { get; init; } = new Dictionary<Player, double>();

    public double this[Player player]
    {
        get => payoffs[player];
        set => payoffs[player] = value;
    }

    public PlayerPayoffRoundMap(double[][] matrix)
    {
        this.matrix = matrix;
    }

    public PlayerPayoffRoundMap Calculate(IPlayerState ps1, IPlayerState ps2)
    {
        var b1 = ps1.currentBehaviour;
        var b2 = ps2.currentBehaviour;
        var win1 = matrix[(int)b1][(int)b2];
        var win2 = matrix[(int)b2][(int)b1];
        this[ps1.p] = win1;
        this[ps2.p] = win2;
        return this;
    }
}