using FaveoReferenceDataSyncApi.API.Models;
using FaveoReferenceDataSyncApi.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaveoReferenceDataSyncApi.API.Controllers;

[ApiController]
[Route("api/catalogs")]
public sealed class EmployeeCatalogController(IEmployeeCatalogService employeeCatalogService) : ControllerBase
{
    [HttpGet("employees")]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeCatalogItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmployeeCatalogItem>>> GetEmployees(CancellationToken cancellationToken)
    {
        var employees = await employeeCatalogService.GetEmployeesAsync(cancellationToken);
        return Ok(employees);
    }

    [HttpGet("empresas")]
    [ProducesResponseType(typeof(IReadOnlyList<EmpresaCatalogItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmpresaCatalogItem>>> GetEmpresas(CancellationToken cancellationToken)
    {
        var empresas = await employeeCatalogService.GetEmpresasAsync(cancellationToken);
        return Ok(empresas);
    }

    [HttpGet("localidades")]
    [ProducesResponseType(typeof(IReadOnlyList<LocalidadCatalogItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<LocalidadCatalogItem>>> GetLocalidades(CancellationToken cancellationToken)
    {
        var localidades = await employeeCatalogService.GetLocalidadesAsync(cancellationToken);
        return Ok(localidades);
    }

    [HttpGet("lineas")]
    [ProducesResponseType(typeof(IReadOnlyList<LineaCatalogItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<LineaCatalogItem>>> GetLineas(CancellationToken cancellationToken)
    {
        var lineas = await employeeCatalogService.GetLineasAsync(cancellationToken);
        return Ok(lineas);
    }

    [HttpGet("areas")]
    [ProducesResponseType(typeof(IReadOnlyList<AreaCatalogItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AreaCatalogItem>>> GetAreas(CancellationToken cancellationToken)
    {
        var areas = await employeeCatalogService.GetAreasAsync(cancellationToken);
        return Ok(areas);
    }

    [HttpGet("secciones-departamentos")]
    [ProducesResponseType(typeof(IReadOnlyList<SeccionDepartamentoCatalogItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SeccionDepartamentoCatalogItem>>> GetSeccionesDepartamentos(CancellationToken cancellationToken)
    {
        var secciones = await employeeCatalogService.GetSeccionesDepartamentosAsync(cancellationToken);
        return Ok(secciones);
    }
}
