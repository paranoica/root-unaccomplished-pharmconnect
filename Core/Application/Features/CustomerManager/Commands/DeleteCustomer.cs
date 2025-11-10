using MediatR;
using FluentValidation;

using Domain.Entities;
using Application.Common.Repositories;

namespace Application.Features.CustomerManager.Commands;
public class DeleteCustomerResult
{
    public Customer? Data { get; set; }
}

public class DeleteCustomerRequest : IRequest<DeleteCustomerResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerRequest>
{
    public DeleteCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerRequest, DeleteCustomerResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandRepository<Customer> _repository;

    public DeleteCustomerHandler(IUnitOfWork unitOfWork, ICommandRepository<Customer> repository)
        => (_unitOfWork, _repository) = (unitOfWork, repository);

    public async Task<DeleteCustomerResult> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken); if (entity == null)
            throw new Exception($"Entity not found: {request.Id}");

        entity.UpdatedById = request.DeletedById;
        _repository.Delete(entity);

        await _unitOfWork.SaveAsync(cancellationToken);
        return new DeleteCustomerResult
        {
            Data = entity
        };
    }
}