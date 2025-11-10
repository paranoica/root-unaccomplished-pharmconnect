using MediatR;
using FluentValidation;

using Domain.Entities;
using Application.Common.Repositories;

namespace Application.Features.CustomerCategoryManager.Commands;
public class CreateCustomerCategoryResult
{
    public CustomerCategory? Data { get; set; }
}

public class CreateCustomerCategoryRequest : IRequest<CreateCustomerCategoryResult>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateCustomerCategoryValidator : AbstractValidator<CreateCustomerCategoryRequest>
{
    public CreateCustomerCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateCustomerCategoryHandler : IRequestHandler<CreateCustomerCategoryRequest, CreateCustomerCategoryResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandRepository<CustomerCategory> _repository;

    public CreateCustomerCategoryHandler(IUnitOfWork unitOfWork, ICommandRepository<CustomerCategory> repository) => (_unitOfWork, _repository) = (unitOfWork, repository);
    public async Task<CreateCustomerCategoryResult> Handle(CreateCustomerCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new CustomerCategory();

        entity.Name = request.Name;
        entity.CreatedById = request.CreatedById;
        entity.Description = request.Description;

        await _repository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new CreateCustomerCategoryResult
        {
            Data = entity
        };
    }
}