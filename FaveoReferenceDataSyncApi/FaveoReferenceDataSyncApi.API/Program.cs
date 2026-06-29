using FaveoReferenceDataSyncApi.API.Jobs;
using FaveoReferenceDataSyncApi.API.Models;
using FaveoReferenceDataSyncApi.API.Models.Sync;
using FaveoReferenceDataSyncApi.API.Services;
using FaveoReferenceDataSyncApi.API.Services.Sync;
using Hangfire;
using Microsoft.Extensions.Options;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWindowsService(options =>
{
    options.ServiceName = "Faveo Reference Data Sync API";
});

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.Configure<TicketFormApiOptions>(
    builder.Configuration.GetSection(TicketFormApiOptions.SectionName));
builder.Services.Configure<SyncCacheOptions>(
    builder.Configuration.GetSection(SyncCacheOptions.SectionName));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IEmployeeCatalogService, EmployeeCatalogService>();
builder.Services.AddScoped<ITicketFormSyncJob, TicketFormSyncJob>();
builder.Services.AddSingleton<ISyncCacheStore, FileSyncCacheStore>();
builder.Services.AddScoped<ITicketFormApiClient, TicketFormApiClient>();
builder.Services.AddSingleton<IJobFileLogger, DailyFileJobLogger>();

builder.Services.AddHangfire(configuration => configuration.UseInMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddHttpClient("TicketFormApi", (serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<TicketFormApiOptions>>().Value;

    if (!string.IsNullOrWhiteSpace(options.Url))
    {
        client.BaseAddress = new Uri(options.Url.EndsWith("/", StringComparison.Ordinal) ? options.Url : options.Url + "/");
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

app.UseHangfireDashboard("/hangfire");

app.MapControllers();

RecurringJob.AddOrUpdate<ITicketFormSyncJob>(
    "ticket-form-reference-data-sync",
    job => job.RunAsync(),
    Cron.Daily(20));

app.Run();