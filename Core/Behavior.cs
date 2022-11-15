namespace PSSN.Core;

public enum Behavior
{
    C, D
}

public static class BehaviorExtensions
{
    public static Behavior Other(this Behavior b)
    {
        return b == Behavior.C ? Behavior.D : Behavior.C;
    }
}