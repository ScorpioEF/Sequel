using System;
using System.Collections.Generic;
using System.Linq;

namespace Sequel.Infrastructure.Operations
{
    public class ValidationManager<TEntity>
    {
        private readonly HashSet<IValidationRule<TEntity>> _rulesList;

        public ValidationManager()
        {
            _rulesList = new HashSet<IValidationRule<TEntity>>();
        }

        public ValidationManager<TEntity> Must<TValidationRule>(params object[] args) where TValidationRule : IValidationRule<TEntity>
        {
            IValidationRule<TEntity> validationRule = (IValidationRule<TEntity>)Activator.CreateInstance(typeof(TValidationRule), args);
            _rulesList.Add(validationRule);
            return this;
        }

        public bool Validate(TEntity entity)
        {
            foreach (IValidationRule<TEntity> validationRule in _rulesList)
            {
                if (validationRule.Validate(entity) == false)
                    return false;
            }
            return true;
        }

        public ValidationResult GetValidationResult(TEntity entity)
        {
            HashSet<string> validationMessages = new HashSet<string>();
            foreach (IValidationRule<TEntity> validationRule in _rulesList)
            {
                bool result = validationRule.Validate(entity);
                if (result == false)
                    validationMessages.Add(validationRule.Message);
            }

            if (validationMessages.Any())
                return new ValidationResult(OperationStatus.InvalidArguments, validationMessages.ToArray());
            return new ValidationResult(OperationStatus.Success);
        }
    }
}
