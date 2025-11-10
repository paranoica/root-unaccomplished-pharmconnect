using MediatR;
using AutoMapper;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Application.Common.Extensions;
using Application.Common.CQS.Queries;

namespace Application.Features.CustomerGroupManager.Queries;
public record GetCustomerGroupListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetCustomerGroupListProfile : Profile
{
    public GetCustomerGroupListProfile()
    {
        CreateMap<CustomerGroup, GetCustomerGroupListDto>();
    }
}

public class GetCustomerGroupListResult
{
    public List<GetCustomerGroupListDto>? Data { get; init; }
}

public class GetCustomerGroupListRequest : IRequest<GetCustomerGroupListResult>
{
    public bool IsDeleted { get; init; } = false;
}

public class GetCustomerGroupListHandler : IRequestHandler<GetCustomerGroupListRequest, GetCustomerGroupListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetCustomerGroupListHandler(IMapper mapper, IQueryContext context) => (_mapper, _context) = (mapper, context);
    public async Task<GetCustomerGroupListResult> Handle(GetCustomerGroupListRequest request, CancellationToken cancellationToken)
    {
        var query = _context.CustomerGroup.AsNoTracking().ApplyIsDeletedFilter(request.IsDeleted).AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);
        var map_dtos = _mapper.Map<List<GetCustomerGroupListDto>>(entities);

        return new GetCustomerGroupListResult
        {
            Data = map_dtos
        };
    }
}