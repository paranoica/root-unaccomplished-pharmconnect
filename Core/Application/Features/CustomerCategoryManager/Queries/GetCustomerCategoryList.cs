using MediatR;
using AutoMapper;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Application.Common.Extensions;
using Application.Common.CQS.Queries;

namespace Application.Features.CustomerCategoryManager.Queries;
public record GetCustomerCategoryListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetCustomerCategoryListProfile : Profile
{
    public GetCustomerCategoryListProfile()
    {
        CreateMap<CustomerCategory, GetCustomerCategoryListDto>();
    }
}

public class GetCustomerCategoryListResult
{
    public List<GetCustomerCategoryListDto>? Data { get; init; }
}

public class GetCustomerCategoryListRequest : IRequest<GetCustomerCategoryListResult>
{
    public bool IsDeleted { get; init; } = false;
}

public class GetCustomerCategoryListHandler : IRequestHandler<GetCustomerCategoryListRequest, GetCustomerCategoryListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetCustomerCategoryListHandler(IQueryContext context, IMapper mapper) => (_context, _mapper) = (context, mapper);
    public async Task<GetCustomerCategoryListResult> Handle(GetCustomerCategoryListRequest request, CancellationToken cancellationToken)
    {
        var query = _context.CustomerCategory.AsNoTracking().ApplyIsDeletedFilter(request.IsDeleted).AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);
        var map_dtos = _mapper.Map<List<GetCustomerCategoryListDto>>(entities);

        return new GetCustomerCategoryListResult
        {
            Data = map_dtos
        };
    }
}