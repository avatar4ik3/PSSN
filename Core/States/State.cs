using PSSN.Core.Round;

namespace PSSN.Core.States;

public interface State
{
    public bool Next(Game g);
}