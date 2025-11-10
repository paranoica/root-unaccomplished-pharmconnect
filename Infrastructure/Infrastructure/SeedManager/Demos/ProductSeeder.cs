using Application.Common.Repositories;
using Application.Features.NumberSequenceManager;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedManager.Demos
{
    public class ProductSeeder
    {
        private readonly ICommandRepository<Product> _productRepository;
        private readonly ICommandRepository<ProductGroup> _productGroupRepository;
        private readonly ICommandRepository<UnitMeasure> _unitMeasureRepository;
        private readonly NumberSequenceService _numberSequenceService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductSeeder(
            ICommandRepository<Product> productRepository,
            ICommandRepository<ProductGroup> productGroupRepository,
            ICommandRepository<UnitMeasure> unitMeasureRepository,
            NumberSequenceService numberSequenceService,
            IUnitOfWork unitOfWork
        )
        {
            _productRepository = productRepository;
            _productGroupRepository = productGroupRepository;
            _unitMeasureRepository = unitMeasureRepository;
            _numberSequenceService = numberSequenceService;
            _unitOfWork = unitOfWork;
        }

        public async Task GenerateDataAsync()
        {
            var productGroups = await _productGroupRepository.GetQuery().ToListAsync();
            var measures = (await _unitMeasureRepository.GetQuery().Where(x => x.Name == "unit").ToListAsync()).Select(x => x.Id).ToArray();

            var groupMapping = new Dictionary<string, string>(); foreach (var pg in productGroups)
                if (!string.IsNullOrEmpty(pg.Name) && pg.Id != null)
                    groupMapping.Add(pg.Name, pg.Id);

            var products = new List<Product>
            {
                new Product { Name = "Painkiller 100mg", UnitPrice = 19.99, ProductGroupId = groupMapping["Medicines"] },
                new Product { Name = "Imodium Express 2mg", UnitPrice = 9.99, ProductGroupId = groupMapping["Medicines"] },
                new Product { Name = "Ointment Dioxidine 30g", UnitPrice = 12.59, ProductGroupId = groupMapping["Medicines"] },

                new Product { Name = "Diapers 50", UnitPrice = 59.99, ProductGroupId = groupMapping["Medical products"] },
                new Product { Name = "Medical bandages 10m", UnitPrice = 39.80, ProductGroupId = groupMapping["Medical products"] },
                new Product { Name = "Thermometer Universal v2.0", UnitPrice = 14.99, ProductGroupId = groupMapping["Medical products"] },

                new Product { Name = "Deodorant Unisex 50g", UnitPrice = 3.99, ProductGroupId = groupMapping["Personal hygiene products"] },
                new Product { Name = "Teeth carebrusher v1", UnitPrice = 19.99, ProductGroupId = groupMapping["Personal hygiene products"] },

                new Product { Name = "Mineral complexes", UnitPrice = 29.99, ProductGroupId = groupMapping["Biologically active additives"] },
                new Product { Name = "Vitamin B Complex", UnitPrice = 9.85, ProductGroupId = groupMapping["Biologically active additives"] },
                new Product { Name = "FoodAdditive Universal", UnitPrice = 21.95, ProductGroupId = groupMapping["Biologically active additives"] },
                new Product { Name = "BabyFood box", UnitPrice = 49.99, ProductGroupId = groupMapping["Biologically active additives"] }
            };

            foreach (var product in products)
            {
                product.Number = _numberSequenceService.GenerateNumber(nameof(Product), "", "ART");
                product.UnitMeasureId = measures[0];
                product.Physical ??= true;

                await _productRepository.CreateAsync(product);
            }

            await _unitOfWork.SaveAsync();
        }

        private static T GetRandomValue<T>(T[] array, Random random)
        {
            return array[random.Next(array.Length)];
        }
    }
}