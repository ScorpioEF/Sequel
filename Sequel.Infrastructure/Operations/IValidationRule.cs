namespace Sequel.Infrastructure.Operations
{
    public interface IValidationRule<in TEntity>
    {
        string Message { get; }
        bool Validate(TEntity entity);
    }
}
