using FaveoReferenceDataSyncApi.API.Models.Sync;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FaveoReferenceDataSyncApi.API.Services.Sync;

public sealed class FileSyncCacheStore(IOptions<SyncCacheOptions> options, IWebHostEnvironment environment) : ISyncCacheStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public async Task<SyncCacheState> LoadAsync(CancellationToken cancellationToken)
    {
        var path = GetCachePath();
        if (!File.Exists(path))
        {
            return new SyncCacheState();
        }

        await using var stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<SyncCacheState>(stream, JsonOptions, cancellationToken)
            ?? new SyncCacheState();
    }

    public async Task SaveAsync(SyncCacheState cache, CancellationToken cancellationToken)
    {
        var path = GetCachePath();
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        var tempPath = path + ".tmp";
        await using (var stream = File.Create(tempPath))
        {
            await JsonSerializer.SerializeAsync(stream, cache, JsonOptions, cancellationToken);
        }

        File.Move(tempPath, path, overwrite: true);
    }

    private string GetCachePath()
    {
        var configuredPath = options.Value.Path;
        return Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(environment.ContentRootPath, configuredPath);
    }
}
