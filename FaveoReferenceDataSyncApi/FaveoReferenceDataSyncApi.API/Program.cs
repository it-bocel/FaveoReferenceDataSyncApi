using FaveoReferenceDataSyncApi.API.Models;
using FaveoReferenceDataSyncApi.API.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TicketFormApiOptions>(
    builder.Configuration.GetSection(TicketFormApiOptions.SectionName));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IEmployeeCatalogService, EmployeeCatalogService>();

builder.Services.AddHttpClient("TicketFormApi", (serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<TicketFormApiOptions>>().Value;

    if (!string.IsNullOrWhiteSpace(options.Url))
    {
        client.BaseAddress = new Uri(options.Url);
    }

    if (!string.IsNullOrWhiteSpace(options.ApiKey))
    {
        client.DefaultRequestHeaders.Add(options.ApiKeyHeaderName, options.ApiKey);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
