using System;

namespace Sequel.Infrastructure.Operations
{
    public enum OperationStatus
    {
        None,
        Success,
        EntityNotFound,
        EntityCreated,
        InvalidArguments,
        Error
    }

    public interface IOperationResult
    {
        OperationStatus Status { get; }
        string[] Messages { get; }
    }

    public interface IOperationResult<out TEntity> : IOperationResult
    {
        TEntity Entity { get; }
    }

    public interface ICanDoOperation : IOperationResult { }

    public class OperationResult : IOperationResult
    {
        public OperationResult(OperationStatus status)
        {
            Status = status;
            Messages = new string[0];
        }

        public OperationResult(OperationStatus succeeded, params string[] messages)
            : this(succeeded)
        {
            Messages = messages;
        }

        public OperationStatus Status { get; }

        public string[] Messages { get; }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Messages);
        }
    }

    public class OperationResult<TEntity> : IOperationResult<TEntity>
    {
        public OperationResult(OperationStatus success, TEntity entity)
        {
            Entity = entity;
            Status = success;
            Messages = new string[0];
        }

        public OperationResult(OperationStatus status, TEntity entity, params string[] messages)
            : this(status, entity)
        {
            Messages = messages;
        }

        public TEntity Entity { get; }

        public OperationStatus Status { get; }

        public string[] Messages { get; }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Messages);
        }
    }

    public class ValidationResult : OperationResult, ICanDoOperation
    {
        public ValidationResult(OperationStatus status)
            : base(status)
        {
        }

        public ValidationResult(OperationStatus status, params string[] messages)
            : base(status, messages)
        {
        }
    }
}
