using PSSN.Core.Round;

namespace PSSN.Core.Strategies;

public interface IStrategy //: IEquatable<IStrategy>
{
    public int Id {get;set;}
    public string Name { get; set; }
    public Behavior GetNextBehaviour(Game g, Player p);
}