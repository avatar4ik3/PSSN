namespace PSSN.Core;

public class Player
{
    public String Name {get;set;}

    public Player(string name)
    {
        Name = name;
    }

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