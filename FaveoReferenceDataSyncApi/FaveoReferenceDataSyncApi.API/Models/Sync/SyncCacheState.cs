namespace FaveoReferenceDataSyncApi.API.Models.Sync;

public sealed class SyncCacheState
{
    public Dictionary<string, Dictionary<string, CachedCatalogItem>> Catalogs { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public Dictionary<string, CachedEmployeeItem> Employees { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public sealed class CachedCatalogItem
{
    public int? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string PayloadHash { get; set; } = string.Empty;

    public DateTimeOffset SentAt { get; set; }
}

public sealed class CachedEmployeeItem
{
    public string PayloadHash { get; set; } = string.Empty;

    public DateTimeOffset SentAt { get; set; }
}
