using Domain.Enums;
using Domain.Entities;

using MediatR;
using FluentValidation;

using Application.Common.Extensions;
using Application.Common.Repositories;

using Application.Features.NumberSequenceManager;
using Application.Features.InventoryTransactionManager;

using Microsoft.EntityFrameworkCore;
namespace Application.Features.DeliveryOrderManager.Commands;

public class CreateDeliveryOrderResult
{
    public DeliveryOrder? Data { get; set; }
}

public class CreateDeliveryOrderRequest : IRequest<CreateDeliveryOrderResult>
{
    public DateTime? DeliveryDate { get; init; }
    public string? Status { get; init; }
    public string? Description { get; init; }
    public string? SalesOrderId { get; init; }
    public string? CreatedById { get; init; }
}

public class CreateDeliveryOrderValidator : AbstractValidator<CreateDeliveryOrderRequest>
{
    public CreateDeliveryOrderValidator()
    {
        RuleFor(x => x.DeliveryDate).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.SalesOrderId).NotEmpty();
    }
}

public class CreateDeliveryOrderHandler : IRequestHandler<CreateDeliveryOrderRequest, CreateDeliveryOrderResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly NumberSequenceService _numberSequenceService;
    private readonly InventoryTransactionService _inventoryTransactionService;

    private readonly ICommandRepository<Warehouse> _warehouseRepository;
    private readonly ICommandRepository<DeliveryOrder> _deliveryOrderRepository;
    private readonly ICommandRepository<SalesOrderItem> _salesOrderItemRepository;

    public CreateDeliveryOrderHandler(
        IUnitOfWork unitOfWork,
        NumberSequenceService numberSequenceService,
        InventoryTransactionService inventoryTransactionService,

        ICommandRepository<Warehouse> warehouseRepository,
        ICommandRepository<DeliveryOrder> deliveryOrderRepository,
        ICommandRepository<SalesOrderItem> salesOrderItemRepository
    ) {
        _unitOfWork = unitOfWork;
        _numberSequenceService = numberSequenceService;
        _inventoryTransactionService = inventoryTransactionService;

        _warehouseRepository = warehouseRepository;
        _deliveryOrderRepository = deliveryOrderRepository;
        _salesOrderItemRepository = salesOrderItemRepository;
    }

    public async Task<CreateDeliveryOrderResult> Handle(CreateDeliveryOrderRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new DeliveryOrder();

        entity.Number = _numberSequenceService.GenerateNumber(nameof(DeliveryOrder), "", "DO");
        entity.DeliveryDate = request.DeliveryDate;
        entity.Status = (DeliveryOrderStatus)int.Parse(request.Status!);
        entity.Description = request.Description;
        entity.CreatedById = request.CreatedById;
        entity.SalesOrderId = request.SalesOrderId;

        await _deliveryOrderRepository.CreateAsync(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var defaultWarehouse = await _warehouseRepository.GetQuery().ApplyIsDeletedFilter(false)
                              .Where(x => x.SystemWarehouse == false).FirstOrDefaultAsync(cancellationToken);

        if (defaultWarehouse != null)
        {
            var items = await _salesOrderItemRepository.GetQuery().ApplyIsDeletedFilter(false)
                       .Where(x => x.SalesOrderId == entity.SalesOrderId)
                       .Include(x => x.Product).ToListAsync(cancellationToken);

            foreach (var item in items)
                if (item?.Product?.Physical ?? false)
                    await _inventoryTransactionService.DeliveryOrderCreateInvenTrans(
                        entity.Id, defaultWarehouse.Id, item.ProductId,
                        item.Quantity, entity.CreatedById, cancellationToken
                    );
        }

        return new CreateDeliveryOrderResult
        {
            Data = entity
        };
    }
}