namespace PSSN.Core.States;

public interface State
{
    public bool Next(Game g);
}