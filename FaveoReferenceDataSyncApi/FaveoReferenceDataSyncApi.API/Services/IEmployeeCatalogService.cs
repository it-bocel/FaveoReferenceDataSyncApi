using FaveoReferenceDataSyncApi.API.Models;

namespace FaveoReferenceDataSyncApi.API.Services;

public interface IEmployeeCatalogService
{
    Task<IReadOnlyList<EmployeeCatalogItem>> GetEmployeesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<EmpresaCatalogItem>> GetEmpresasAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<LocalidadCatalogItem>> GetLocalidadesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<LineaCatalogItem>> GetLineasAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<AreaCatalogItem>> GetAreasAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<SeccionDepartamentoCatalogItem>> GetSeccionesDepartamentosAsync(CancellationToken cancellationToken);
}
