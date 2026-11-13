namespace Storage.Minio;

/// <summary>
/// Опции конфигурации клиента minio.
/// </summary>
public sealed class MinioOptions
{
    public const string SectionName = "Minio";

    public string Endpoint { get; set; } = "localhost:9000";
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string DefaultBucket { get; set; } = "files";
    public bool UseSsl { get; set; } = false;
    public int? PresignedUrlExpiryMinutes { get; set; } = 60;
}