namespace PSSN.Core.Operators;

public interface IOperator<T, O>
{
    public IEnumerable<O> Operate(IEnumerable<T> strategies);
    public O Operate(T strategies);
}