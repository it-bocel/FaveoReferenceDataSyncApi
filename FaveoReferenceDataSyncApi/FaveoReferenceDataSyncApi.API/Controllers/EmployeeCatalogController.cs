using FaveoReferenceDataSyncApi.API.Models;
using FaveoReferenceDataSyncApi.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaveoReferenceDataSyncApi.API.Controllers;

[ApiController]
[Route("api/catalogs/employees")]
public sealed class EmployeeCatalogController(IEmployeeCatalogService employeeCatalogService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeCatalogItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmployeeCatalogItem>>> GetEmployees(CancellationToken cancellationToken)
    {
        var employees = await employeeCatalogService.GetEmployeesAsync(cancellationToken);
        return Ok(employees);
    }
}
