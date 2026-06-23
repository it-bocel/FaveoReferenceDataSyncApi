namespace FaveoReferenceDataSyncApi.API.Models.Sync;

public sealed class SyncCacheOptions
{
    public const string SectionName = "SyncCache";

    public string Path { get; set; } = "App_Data/sync-cache.json";
}
