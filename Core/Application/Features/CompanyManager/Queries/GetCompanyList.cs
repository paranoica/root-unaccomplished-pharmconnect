using MediatR;
using AutoMapper;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Application.Common.Extensions;
using Application.Common.CQS.Queries;

namespace Application.Features.CompanyManager.Queries;
public record GetCompanyListDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Currency { get; init; }
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? ZipCode { get; init; }
    public string? Country { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FaxNumber { get; init; }
    public string? EmailAddress { get; init; }
    public string? Website { get; init; }
    public DateTime? CreatedAtUtc { get; init; }
}

public class GetCompanyListProfile : Profile
{
    public GetCompanyListProfile()
    {
        CreateMap<Company, GetCompanyListDto>();
    }
}

public class GetCompanyListResult
{
    public List<GetCompanyListDto>? Data { get; init; }
}

public class GetCompanyListRequest : IRequest<GetCompanyListResult>
{
    public bool IsDeleted { get; init; } = false;
}

public class GetCompanyListHandler : IRequestHandler<GetCompanyListRequest, GetCompanyListResult>
{
    private readonly IMapper _mapper;
    private readonly IQueryContext _context;

    public GetCompanyListHandler(IQueryContext context, IMapper mapper) => (_context, _mapper) = (context, mapper);
    public async Task<GetCompanyListResult> Handle(GetCompanyListRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Company.AsNoTracking().ApplyIsDeletedFilter(request.IsDeleted).AsQueryable();

        var entities = await query.ToListAsync(cancellationToken);
        var map_dtos = _mapper.Map<List<GetCompanyListDto>>(entities);

        return new GetCompanyListResult
        {
            Data = map_dtos
        };
    }
}