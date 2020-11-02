namespace Sequel.Infrastructure.Operations
{
    public interface IQuery
    {
    }

    public interface IQuery<out TResult> : IQuery
    {
        TResult Query();
    }

    public interface IQuery<in TArgs, out TResult> : IQuery
    {
        TResult Query(TArgs args);
    }
}
