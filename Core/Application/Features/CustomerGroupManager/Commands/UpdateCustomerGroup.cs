using MediatR;
using FluentValidation;

using Domain.Entities;
using Application.Common.Repositories;

namespace Application.Features.CustomerGroupManager.Commands;
public class UpdateCustomerGroupResult
{
    public CustomerGroup? Data { get; set; }
}

public class UpdateCustomerGroupRequest : IRequest<UpdateCustomerGroupResult>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? UpdatedById { get; init; }
}

public class UpdateCustomerGroupValidator : AbstractValidator<UpdateCustomerGroupRequest>
{
    public UpdateCustomerGroupValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class UpdateCustomerGroupHandler : IRequestHandler<UpdateCustomerGroupRequest, UpdateCustomerGroupResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandRepository<CustomerGroup> _repository;

    public UpdateCustomerGroupHandler(IUnitOfWork unitOfWork, ICommandRepository<CustomerGroup> repository) => (
        _unitOfWork, _repository
    ) = (unitOfWork, repository);

    public async Task<UpdateCustomerGroupResult> Handle(UpdateCustomerGroupRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken); if (entity == null)
            throw new Exception($"Entity not found: {request.Id}");

        entity.Name = request.Name;
        entity.UpdatedById = request.UpdatedById;
        entity.Description = request.Description;

        _repository.Update(entity);

        await _unitOfWork.SaveAsync(cancellationToken);
        return new UpdateCustomerGroupResult
        {
            Data = entity
        };
    }
}