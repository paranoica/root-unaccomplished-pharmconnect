using Application.Common.Repositories;
using Domain.Entities;

namespace Infrastructure.SeedManager.Systems;

public class CompanySeeder
{
    private readonly ICommandRepository<Company> _repository;
    private readonly IUnitOfWork _unitOfWork;
    public CompanySeeder(
        ICommandRepository<Company> repository,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task GenerateDataAsync()
    {
        var entity = new Company
        {
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false,
            Name = "Pharmacy",
            Currency = "EUR",
            Street = "Archenholdstra$e 66",
            City = "Berlin",
            State = "-",
            ZipCode = "120055",
            Country = "Germany",
            PhoneNumber = "+49-32-325-32",
            FaxNumber = "+49-32-325-32",
            EmailAddress = "info@pharmacy.com",
            Website = "https://www.demo.local"
        };

        await _repository.CreateAsync(entity);
        await _unitOfWork.SaveAsync();
    }
}