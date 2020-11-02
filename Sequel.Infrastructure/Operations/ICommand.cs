namespace Sequel.Infrastructure.Operations
{
    public interface IBaseCommand
    {
    }

    public interface ICommand : IBaseCommand
    {
        ICanDoOperation CanExecute();
        void Execute();
    }

    public interface ICommand<in TArgs, out TValidation> : IBaseCommand
        where TValidation : ICanDoOperation
    {
        TValidation CanExecute(TArgs args);
        void Execute(TArgs args);
    }

    public interface ICommandWithResult<out TResult, out TValidation> : IBaseCommand
        where TValidation : ICanDoOperation
    {
        TValidation CanExecute();
        TResult Execute();
    }

    public interface ICommandWithResult<in TArgs, out TResult, out TValidation> : IBaseCommand
    {
        TValidation CanExecute(TArgs args);
        TResult Execute(TArgs args);
    }
}
