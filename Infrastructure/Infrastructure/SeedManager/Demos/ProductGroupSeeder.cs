using Application.Common.Repositories;
using Domain.Entities;

namespace Infrastructure.SeedManager.Demos;

public class ProductGroupSeeder
{
    private readonly ICommandRepository<ProductGroup> _productGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductGroupSeeder(
        ICommandRepository<ProductGroup> productGroupRepository,
        IUnitOfWork unitOfWork
    )
    {
        _productGroupRepository = productGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task GenerateDataAsync()
    {
        var productGroups = new List<ProductGroup>
        {
            new ProductGroup { Name = "Medicines" },
            new ProductGroup { Name = "Medical products" },
            new ProductGroup { Name = "Personal hygiene products" },
            new ProductGroup { Name = "Biologically active additives" }
        };

        foreach (var productGroup in productGroups)
        {
            await _productGroupRepository.CreateAsync(productGroup);
        }

        await _unitOfWork.SaveAsync();
    }
}
