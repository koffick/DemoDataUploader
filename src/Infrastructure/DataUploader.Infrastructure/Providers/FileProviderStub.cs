using DataUploader.Domain.Interfaces;

namespace DataUploader.Infrastructure.Providers
{
    /// <summary>
    /// Класс-заглушка, реализующий интерфейс IFileProvider.
    /// </summary>
    public class FileProviderStub : IFileProvider
    {
        /// <inheritdoc cref="IFileProvider.SaveFileAsync(Stream, string)"/>
        public async Task<Guid> SaveFileAsync(Stream fileData, string fileName)
        {
            return await Task.FromResult(Guid.NewGuid());
        }
    }
}
