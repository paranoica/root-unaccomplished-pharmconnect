using MediatR;
using FluentValidation;

using Domain.Entities;
using Application.Common.Repositories;

namespace Application.Features.CustomerGroupManager.Commands;
public class CreateCustomerGroupResult
{
    public CustomerGroup? Data { get; set; }
}

public class CreateCustomerGroupRequest : IRequest<CreateCustomerGroupResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateCustomerGroupValidator : AbstractValidator<CreateCustomerGroupRequest>
{
    public CreateCustomerGroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateCustomerGroupHandler : IRequestHandler<CreateCustomerGroupRequest, CreateCustomerGroupResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandRepository<CustomerGroup> _repository;

    public CreateCustomerGroupHandler(IUnitOfWork unitOfWork, ICommandRepository<CustomerGroup> repository) => (
        _unitOfWork, _repository
    ) = (unitOfWork, repository);

    public async Task<CreateCustomerGroupResult> Handle(CreateCustomerGroupRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new CustomerGroup();

        entity.Name = request.Name;
        entity.CreatedById = request.CreatedById;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCustomerGroupResult
        {
            Data = entity
        };
    }
}