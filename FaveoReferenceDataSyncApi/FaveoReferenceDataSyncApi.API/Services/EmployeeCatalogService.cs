using FaveoReferenceDataSyncApi.API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FaveoReferenceDataSyncApi.API.Services;

public sealed class EmployeeCatalogService(IConfiguration configuration) : IEmployeeCatalogService
{
    private const string Query = """
        SELECT
            A.codigo_empleado AS Codigo,
            RTRIM(A.primer_nombre) + ' ' + RTRIM(A.segundo_nombre) + ' ' + RTRIM(A.primer_apellido) + ' ' + RTRIM(A.segundo_apellido) AS [Nombre Empleado],
            A.fecha_ultimo_ingreso AS [Fecha Ingreso],
            A.serie_nueva + '- ' + A.cedula_nueva + '- ' + A.cedula_guion AS Cedula,
            A.sexo AS Genero,
            B.descripcion AS [Posición],
            A.codigo_empresa AS Empresa,
            RIGHT(F.descripcion, 8) AS Localidad,
            C.descripcion AS Linea,
            G.descripcion AS Area,
            D.descripcion AS [Sección(Dept)],
            A.estatus
        FROM Bvsoft.dbo.EMPLEADO AS A
        INNER JOIN Bvsoft.dbo.OCUPACION AS B ON A.codigo_ocupacion = B.codigo_ocupacion
        INNER JOIN Bvsoft.dbo.LINEA AS C ON A.codigo_linea = C.codigo_Linea AND A.codigo_empresa = C.codigo_empresa AND A.codigo_planta = C.codigo_planta
        INNER JOIN Bvsoft.dbo.DEPARTAMENTO AS D ON A.codigo_departamento = D.codigo_departamento AND A.codigo_empresa = D.codigo_empresa
            AND A.codigo_planta = D.codigo_planta AND A.codigo_linea = D.codigo_Linea
        INNER JOIN Bvsoft.dbo.NIVELSALARIAL AS E ON B.codigo_nivel = E.codigo_nivel
        INNER JOIN Bvsoft.dbo.CLASENOMINA AS F ON A.clase_nomina = F.codigo_clase_nomina
        INNER JOIN Bvsoft.dbo.CLASIFICACION AS G ON A.clasificacion = G.codigo_clasificacion
        WHERE (A.estatus <> 'C') AND (A.codigo_empresa IN ('MVC', 'LDI'))
        """;

    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string DefaultConnection is not configured.");

    public async Task<IReadOnlyList<EmployeeCatalogItem>> GetEmployeesAsync(CancellationToken cancellationToken)
    {
        var employees = new List<EmployeeCatalogItem>();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(Query, connection)
        {
            CommandType = CommandType.Text
        };

        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult, cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            employees.Add(new EmployeeCatalogItem
            {
                Codigo = GetNullableString(reader, "Codigo"),
                NombreEmpleado = GetNullableString(reader, "Nombre Empleado"),
                FechaIngreso = GetNullableDateTime(reader, "Fecha Ingreso"),
                Cedula = GetNullableString(reader, "Cedula"),
                Genero = GetNullableString(reader, "Genero"),
                Posicion = GetNullableString(reader, "Posición"),
                Empresa = GetNullableString(reader, "Empresa"),
                Localidad = GetNullableString(reader, "Localidad"),
                Linea = GetNullableString(reader, "Linea"),
                Area = GetNullableString(reader, "Area"),
                SeccionDept = GetNullableString(reader, "Sección(Dept)"),
                Estatus = GetNullableString(reader, "estatus")
            });
        }

        return employees;
    }

    private static string? GetNullableString(SqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal).ToString();
    }

    private static DateTime? GetNullableDateTime(SqlDataReader reader, string name)
    {
        var ordinal = reader.GetOrdinal(name);
        return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
    }
}