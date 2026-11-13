using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Storage.Minio;

/// <summary>
/// Провайер для взаимодействия с хранилищем файлов.
/// </summary>
public sealed class MinioFileStorageProvider : IFileStorageProvider
{
    private readonly IMinioClient _client;
    private readonly MinioOptions _options;
    private readonly ILogger<MinioFileStorageProvider> _logger;

    public MinioFileStorageProvider(
        IMinioClient client,
        IOptions<MinioOptions> options,
        ILogger<MinioFileStorageProvider> logger)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    private string ResolveBucket(string? bucket)
        => string.IsNullOrWhiteSpace(bucket) ? _options.DefaultBucket : bucket;

    /// <inheritdoc cref="IFileStorageProvider.EnsureBucketExistsAsync(string?, CancellationToken)"/>
    public async Task EnsureBucketExistsAsync(string? bucket = null, CancellationToken ct = default)
    {
        var name = ResolveBucket(bucket);

        var existsArgs = new BucketExistsArgs().WithBucket(name);
        var exists = await _client.BucketExistsAsync(existsArgs, ct);

        if (exists) return;

        var makeArgs = new MakeBucketArgs().WithBucket(name);
        await _client.MakeBucketAsync(makeArgs, ct);

        _logger.LogInformation($"Bucket '{name}' создана.");
    }

    /// <inheritdoc cref="IFileStorageProvider.UploadAsync(string, Stream, string, string?, IDictionary{string, string}?, CancellationToken)"/>
    public async Task UploadAsync(
        string objectName,
        Stream stream,
        string contentType = "application/octet-stream",
        string? bucket = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(objectName);
        ArgumentNullException.ThrowIfNull(stream);
        stream.Position = 0;

        var bucketName = ResolveBucket(bucket);

        var args = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.CanSeek ? stream.Length : -1)
            .WithContentType(contentType);

        if (metadata is { Count: > 0 })
        {
            args = args.WithHeaders(metadata.ToDictionary(x => x.Key, x => x.Value));
        }

        await _client.PutObjectAsync(args, ct);
        _logger.LogDebug($"Загружен объект '{objectName}' to bucket '{bucketName}'");
    }

    /// <inheritdoc cref="IFileStorageProvider.DeleteAsync(string, string?, CancellationToken)"/>
    public async Task<Stream> DownloadAsync(
        string objectName,
        string? bucket = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(objectName);
        var bucketName = ResolveBucket(bucket);

        var memory = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream => stream.CopyTo(memory));

        await _client.GetObjectAsync(args, ct);
        memory.Position = 0;
        return memory;
    }

    /// <inheritdoc cref="IFileStorageProvider.GetBytesAsync(string, string?, CancellationToken)"/>
    public async Task<byte[]> GetBytesAsync(
        string objectName,
        string? bucket = null,
        CancellationToken ct = default)
    {
        await using var stream = await DownloadAsync(objectName, bucket, ct);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms, ct);
        return ms.ToArray();
    }

    /// <inheritdoc cref="IFileStorageProvider.ExistsAsync(string, string?, CancellationToken)"/>
    public async Task<bool> ExistsAsync(
        string objectName,
        string? bucket = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(objectName);
        var bucketName = ResolveBucket(bucket);

        try
        {
            var args = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);

            await _client.StatObjectAsync(args, ct);
            return true;
        }
        catch (ObjectNotFoundException ex)
        {
            return false;
        }
    }

    /// <inheritdoc cref="IFileStorageProvider.DeleteAsync(string, string?, CancellationToken)"/>
    public async Task DeleteAsync(
        string objectName,
        string? bucket = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(objectName);
        var bucketName = ResolveBucket(bucket);

        var args = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);

        await _client.RemoveObjectAsync(args, ct);
        _logger.LogDebug($"Удален объект '{objectName}' из bucket '{bucketName}'");
    }

    /// <inheritdoc cref="IFileStorageProvider.GetPresignedUrlAsync(string, int?, string?, CancellationToken)"/>
    public async Task<string> GetPresignedUrlAsync(
        string objectName,
        int? expiryMinutes = null,
        string? bucket = null,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(objectName);
        var bucketName = ResolveBucket(bucket);
        var expires = expiryMinutes ?? _options.PresignedUrlExpiryMinutes ?? 60;

        var args = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expires * 60);

        var url = await _client.PresignedGetObjectAsync(args);
        return url;
    }
}