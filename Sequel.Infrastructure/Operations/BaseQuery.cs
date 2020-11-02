namespace Sequel.Infrastructure.Operations
{
    public abstract class BaseQuery<TEntity> : IQuery<OperationResult<TEntity>>
    {
        public abstract OperationResult<TEntity> Query();
    }

    public abstract class BaseQuery<TArgs, TEntity> : IQuery<TArgs, OperationResult<TEntity>>
    {
        public abstract OperationResult<TEntity> Query(TArgs args);
    }
}
