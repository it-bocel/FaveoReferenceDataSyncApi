using FaveoReferenceDataSyncApi.API.Models.Sync;

namespace FaveoReferenceDataSyncApi.API.Services.Sync;

public interface ISyncCacheStore
{
    Task<SyncCacheState> LoadAsync(CancellationToken cancellationToken);

    Task SaveAsync(SyncCacheState cache, CancellationToken cancellationToken);
}
