namespace Sequel.Infrastructure.Operations
{
    public abstract class BaseCommand : ICommandWithResult<OperationResult, ValidationResult>
    {
        public abstract ValidationResult CanExecute();

        public abstract OperationResult Execute();
    }

    public abstract class BaseCommand<TArgs> : ICommandWithResult<TArgs, OperationResult, ValidationResult>
    {
        protected ValidationManager<TArgs> Validator;

        protected BaseCommand()
        {
            Validator = new ValidationManager<TArgs>();
        }

        public abstract ValidationResult CanExecute(TArgs args);

        public abstract OperationResult Execute(TArgs args);
    }

    public abstract class BaseCommand<TArgs, TResult> : ICommandWithResult<TArgs, OperationResult<TResult>, ValidationResult>
    {
        protected ValidationManager<TArgs> Validator;

        protected BaseCommand()
        {
            Validator = new ValidationManager<TArgs>();
        }

        public abstract ValidationResult CanExecute(TArgs args);

        public abstract OperationResult<TResult> Execute(TArgs args);
    }
}
