using Microsoft.AspNetCore.Diagnostics;
using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.Exceptions
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new ErrorResponse();
            if (exception is AdminNotFoundException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ExceptionMessage = exception.Message;
                response.Title = "Wrong Input";
            }
            else if(exception is AgentNotFoundException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ExceptionMessage = exception.Message;
                response.Title = "Wrong Input";
            }
            else if (exception is EmployeeNotFoundException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ExceptionMessage = exception.Message;
                response.Title = "Wrong Input";
            }
            else if (exception is CustomerNotFoundException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ExceptionMessage = exception.Message;
                response.Title = "Wrong Input";
            }
            else if (exception is ValidationException validationException)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.ExceptionMessage = validationException.Message;
                response.Title = "Validation Error";
            }
            else if(exception is PlanExistException planExistException)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.ExceptionMessage = planExistException.Message;
                response.Title = "Already Exist";
            }
            else
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ExceptionMessage = exception.Message;
                response.Title = "Something went Wroung";
            }

            httpContext.Response.StatusCode = response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
