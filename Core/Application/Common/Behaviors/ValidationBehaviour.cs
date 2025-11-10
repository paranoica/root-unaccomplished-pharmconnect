using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var ctx = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(ctx, cancellationToken)));

            var failures = results.Where(r => r.Errors.Any()).SelectMany(r => r.Errors).ToList(); if (failures.Any())
                throw new ValidationException(failures);
        }

        return await next();
    }
}

