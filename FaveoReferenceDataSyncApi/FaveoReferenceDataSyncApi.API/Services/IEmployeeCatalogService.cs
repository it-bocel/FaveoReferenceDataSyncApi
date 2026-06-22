using FaveoReferenceDataSyncApi.API.Models;

namespace FaveoReferenceDataSyncApi.API.Services;

public interface IEmployeeCatalogService
{
    Task<IReadOnlyList<EmployeeCatalogItem>> GetEmployeesAsync(CancellationToken cancellationToken);
}
