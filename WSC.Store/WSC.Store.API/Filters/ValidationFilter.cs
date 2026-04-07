using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WSC.Store.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _service;
        public ValidationFilter(IServiceProvider service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var errors = new List<ValidationFailure>();

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = _service.GetService(validatorType) as IValidator;

                if (validator == null) continue;

                var validationContext = new ValidationContext<object>(argument);
                var result = await validator.ValidateAsync(validationContext);

                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors);
                }
            }

            if (errors.Count > 0)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Message = "Validation Failed",
                    Errors = errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    })
                });

                return;
            }

            await next();
        }
    } 
}
