using MediatR;
using FluentValidation;

using Domain.Entities;
using Application.Common.Repositories;

namespace Application.Features.CustomerContactManager.Commands;
public class DeleteCustomerContactResult
{
    public CustomerContact? Data { get; set; }
}

public class DeleteCustomerContactRequest : IRequest<DeleteCustomerContactResult>
{
    public string? Id { get; init; }
    public string? DeletedById { get; init; }
}

public class DeleteCustomerContactValidator : AbstractValidator<DeleteCustomerContactRequest>
{
    public DeleteCustomerContactValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteCustomerContactHandler : IRequestHandler<DeleteCustomerContactRequest, DeleteCustomerContactResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandRepository<CustomerContact> _repository;

    public DeleteCustomerContactHandler(IUnitOfWork unitOfWork, ICommandRepository<CustomerContact> repository) => (
        _unitOfWork, _repository
    ) = (unitOfWork, repository);

    public async Task<DeleteCustomerContactResult> Handle(DeleteCustomerContactRequest request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id ?? string.Empty, cancellationToken); if (entity == null)
            throw new Exception($"Entity not found: {request.Id}");

        entity.UpdatedById = request.DeletedById;
        _repository.Delete(entity);

        await _unitOfWork.SaveAsync(cancellationToken);
        return new DeleteCustomerContactResult
        {
            Data = entity
        };
    }
}