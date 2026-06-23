namespace FaveoReferenceDataSyncApi.API.Services.Sync;

public interface IJobFileLogger
{
    Task WriteAsync(string message, Exception? exception = null, CancellationToken cancellationToken = default);
}
