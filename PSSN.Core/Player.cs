namespace PSSN.Core;

public class Player
{
    public Player(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public static Player P2 => new("p2");
    public static Player P1 => new("p1");

    public override bool Equals(object? obj)
    {
        return obj is Player player &&
               Name == player.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}