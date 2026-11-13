namespace Storage.Minio;

public interface IFileStorageProvider
{
    /// <summary>
    /// Проверяет наличие bucket.
    /// </summary>
    /// <param name="bucket">Имя bucket.</param>
    /// <param name="ct"><see cref="CancellationToken"/> </param>
    /// <returns></returns>
    Task EnsureBucketExistsAsync(string? bucket = null, CancellationToken ct = default);

    /// <summary>
    /// Загружает файл в хранилище.
    /// </summary>
    /// <param name="objectName">Идентификатор файла.</param>
    /// <param name="stream">Файл в виде потока.</param>
    /// <param name="contentType">Контент.</param>
    /// <param name="bucket">Имя bucket.</param>
    /// <param name="metadata">Сопутсвующие данные.</param>
    /// <param name="ct"><see cref="CancellationToken"/> </param>
    /// <returns></returns>
    Task UploadAsync(
        string objectName,
        Stream stream,
        string contentType = "application/octet-stream",
        string? bucket = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken ct = default);

    /// <summary>
    /// Получает файл из хранилища.
    /// </summary>
    /// <param name="objectName">Идентификатор файла.</param>
    /// <param name="bucket">Имя bucket.</param>
    /// <param name="ct"><see cref="CancellationToken"/> </param>
    /// <returns>Файл в виде потока.</returns>
    Task<Stream> DownloadAsync(
        string objectName,
        string? bucket = null,
        CancellationToken ct = default);

    /// <summary>
    /// Получает файл из хранилища.
    /// </summary>
    /// <param name="objectName">Идентификатор файла.</param>
    /// <param name="bucket">Имя bucket.</param>
    /// <param name="ct"><see cref="CancellationToken"/> </param>
    /// <returns>Файл в виде массива байт.</returns>
    Task<byte[]> GetBytesAsync(
        string objectName,
        string? bucket = null,
        CancellationToken ct = default);

    /// <summary>
    /// Проверяет наличие файла.
    /// </summary>
    /// <param name="objectName">Идентификатор файла.</param>
    /// <param name="bucket">Имя bucket.</param>
    /// <param name="ct"><see cref="CancellationToken"/> </param>
    /// <returns></returns>
    Task<bool> ExistsAsync(
        string objectName,
        string? bucket = null,
        CancellationToken ct = default);

    /// <summary>
    /// Удаляет файл из хранилища.
    /// </summary>
    /// <param name="objectName">Идентификатор файла.</param>
    /// <param name="bucket">Имя bucket.</param>
    /// <param name="ct"><see cref="CancellationToken"/> </param>
    /// <returns></returns>
    Task DeleteAsync(
        string objectName,
        string? bucket = null,
        CancellationToken ct = default);

    /// <summary>
    /// Генерирует прямую ссылку на файл.
    /// </summary>
    /// <param name="objectName">Идентификатор файла.</param>
    /// <param name="expiryMinutes">Вркмя жизни ссылки.</param>
    /// <param name="bucket">Имя bucket.</param>
    /// <param name="ct"><see cref="CancellationToken"/> </param>
    /// <returns></returns>
    Task<string> GetPresignedUrlAsync(
        string objectName,
        int? expiryMinutes = null,
        string? bucket = null,
        CancellationToken ct = default);
}