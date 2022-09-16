namespace PSSN.Api.Model;

public class Strategy
{
    public String Name { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Strategy strategy &&
               Name == strategy.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}
