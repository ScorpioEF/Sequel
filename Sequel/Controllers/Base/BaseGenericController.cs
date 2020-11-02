using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sequel.Infrastructure.Operations;

namespace Sequel.Controllers.Base
{
    public abstract class BaseGenericController : ControllerBase
    {
        protected ActionResult HandleOperationResult(IOperationResult operationResult)
        {
            switch (operationResult.Status)
            {
                case OperationStatus.Success:
                    return Ok(operationResult);
                case OperationStatus.EntityCreated:
                    return Created(string.Empty, operationResult);
                case OperationStatus.EntityNotFound:
                    return NotFound(operationResult);
                case OperationStatus.InvalidArguments:
                    return BadRequest(operationResult);
                case OperationStatus.Error:
                    return StatusCode(StatusCodes.Status500InternalServerError, operationResult);
                default:
                    return NoContent();
            }
        }
    }
}
