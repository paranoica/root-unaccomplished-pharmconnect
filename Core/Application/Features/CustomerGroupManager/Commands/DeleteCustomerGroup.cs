using MediatR;
using FluentValidation;

using Domain.Entities;
using Application.Common.Repositories;

namespace Application.Features.CustomerGroupManager.Commands;
public class DeleteCustomerGroupResult
{
    public CustomerGroup? Data { get; set; }
}

public class DeleteCustomerGroupRequest : IRequest<DeleteCustomerGroupResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteCustomerGroupValidator : AbstractValidator<DeleteCustomerGroupRequest>
{
    public DeleteCustomerGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteCustomerGroupHandler : IRequestHandler<DeleteCustomerGroupRequest, DeleteCustomerGroupResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandRepository<CustomerGroup> _repository;

    public DeleteCustomerGroupHandler(IUnitOfWork unitOfWork, ICommandRepository<CustomerGroup> repository) => (
        _unitOfWork, _repository
    ) = (unitOfWork, repository);

    public async Task<DeleteCustomerGroupResult> Handle(DeleteCustomerGroupRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken); if (entity == null)
            throw new Exception($"Entity not found: {request.Id}");

        entity.UpdatedById = request.DeletedById;
        _repository.Delete(entity);

        await _unitOfWork.SaveAsync(cancellationToken);
        return new DeleteCustomerGroupResult
        {
            Data = entity
        };
    }
}