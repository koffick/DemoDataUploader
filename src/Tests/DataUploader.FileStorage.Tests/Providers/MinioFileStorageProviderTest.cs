using AutoFixture;
using Castle.Core.Logging;
using DataUploader.FileStorage.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Moq;
using Storage.Minio;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace DataUploader.FileStorage.Tests.Providers
{
    public class MinioFileStorageProviderTest
    {
        private MinioFileStorageProvider _provider;
        private Mock<IMinioClient> _client;
        private Mock<IOptions<MinioOptions>> _options;
        private Mock<ILogger<MinioFileStorageProvider>> _logger;

        public MinioFileStorageProviderTest()
        {
            _client = new Mock<IMinioClient> ();
            _options = new Mock<IOptions<MinioOptions>> ();
            _logger = new Mock<ILogger<MinioFileStorageProvider>>();
            _provider = new MinioFileStorageProvider(_client.Object, _options.Object, _logger.Object);
        }

        [Fact]
        public void IsInstance()
        {
            Assert.IsAssignableFrom<IFileStorageProvider>(_provider);
        }

        [Fact]
        public async Task EnsureBucketExists_BucketExist()
        {
            _client.Setup(s => s.BucketExistsAsync(It.IsAny<BucketExistsArgs>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var bucket = Guid.NewGuid().ToString();
            await _provider.EnsureBucketExistsAsync(bucket);
            _client.Verify(v => v.BucketExistsAsync(It.IsAny<BucketExistsArgs>(), It.IsAny<CancellationToken>()), Times.Once);
            _client.Verify(v => v.MakeBucketAsync(It.IsAny<MakeBucketArgs>(), It.IsAny<CancellationToken>()), Times.Never);
            _logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Never
            );
        }

        [Fact]
        public async Task EnsureBucketExists_BucketNotExist()
        {
            _client.Setup(s => s.BucketExistsAsync(It.IsAny<BucketExistsArgs>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var bucket = Guid.NewGuid().ToString();
            await _provider.EnsureBucketExistsAsync(bucket);
            _client.Verify(v => v.BucketExistsAsync(It.IsAny<BucketExistsArgs>(), It.IsAny<CancellationToken>()), Times.Once);
            _client.Verify(v => v.MakeBucketAsync(It.IsAny<MakeBucketArgs>(), It.IsAny<CancellationToken>()), Times.Once);
            _logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == $"Bucket '{bucket}' создана."),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task UploadAsync_ValidData()
        {
            var objectName = Guid.NewGuid().ToString();
            var putObjectResponse = new Fixture().Create<PutObjectResponse>();
            var bucket = Guid.NewGuid().ToString();
            var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            using var stream = new MemoryStream(content);

            _client.Setup(s => s.PutObjectAsync(It.IsAny<PutObjectArgs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(putObjectResponse);

            await _provider.UploadAsync(objectName, stream, bucket: bucket);

            _client.Verify(v => v.PutObjectAsync(It.IsAny<PutObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);

            _logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Debug),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == $"Загружен объект '{objectName}' to bucket '{bucket}'"),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UploadAsync_InvalidArgs()
        {
            var objectName = string.Empty;
            var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            using var stream = new MemoryStream(content);

            await Assert.ThrowsAsync<ArgumentException>(
                    () => _provider.UploadAsync(objectName, stream));

            objectName = Guid.NewGuid().ToString();
            await Assert.ThrowsAsync<NullReferenceException>(
                    () => _provider.UploadAsync(objectName, stream));

            _client.Verify(v => v.PutObjectAsync(It.IsAny<PutObjectArgs>(), It.IsAny<CancellationToken>()), Times.Never);

            _logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Debug),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }


        [Fact]
        public async Task DownloadAsync_InvalidArgs()
        {
            var objectName = string.Empty;
            var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            using var stream = new MemoryStream(content);

            await Assert.ThrowsAsync<ArgumentException>(
                    () => _provider.DownloadAsync(objectName));

            _client.Verify(v => v.GetObjectAsync(It.IsAny<GetObjectArgs>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DownloadAsync_ValidData()
        {
            var objectName = Guid.NewGuid().ToString();
            var bucket = Guid.NewGuid().ToString();
            var getObjectResponse = new Fixture().Create<ObjectStat>();
            var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            using var stream = new MemoryStream(content);

            _client.Setup(s => s.GetObjectAsync(It.IsAny<GetObjectArgs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getObjectResponse);

            await _provider.DownloadAsync(objectName, bucket);

            _client.Verify(v => v.GetObjectAsync(It.IsAny<GetObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetBytesAsync_ValidData()
        {
            var objectName = Guid.NewGuid().ToString();
            var bucket = Guid.NewGuid().ToString();
            var getObjectResponse = new Fixture().Create<ObjectStat>();
            var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            using var stream = new MemoryStream(content);
            var expectedBytes = new byte[] { 1, 2, 3, 4, 5 };

            _client.Setup(s => s.GetObjectAsync(It.IsAny<GetObjectArgs>(), It.IsAny<CancellationToken>()))
                .Callback<GetObjectArgs, CancellationToken>((args, _) =>
                {
                    var callback = CallbackHelpers.ExtractCallback(args);
                    using var source = new MemoryStream(expectedBytes);
                    callback(source);
                })
                .ReturnsAsync(getObjectResponse);

            var response = await _provider.GetBytesAsync(objectName, bucket);

            Assert.NotNull(response);
            _client.Verify(v => v.GetObjectAsync(It.IsAny<GetObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExistsAsync_InvalidArgs()
        {
            var objectName = string.Empty;

            await Assert.ThrowsAsync<ArgumentException>(
                    () => _provider.ExistsAsync(objectName));

            _client.Verify(v => v.StatObjectAsync(It.IsAny<StatObjectArgs>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ExistsAsync_ValidData()
        {
            var objectName = Guid.NewGuid().ToString();
            var bucket = Guid.NewGuid().ToString();
            var getObjectResponse = new Fixture().Create<ObjectStat>();

            _client.Setup(s => s.StatObjectAsync(It.IsAny<StatObjectArgs>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getObjectResponse);

            var response = await _provider.ExistsAsync(objectName, bucket);

            Assert.True(response);
            _client.Verify(v => v.StatObjectAsync(It.IsAny<StatObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InvalidArgs()
        {
            var objectName = string.Empty;

            await Assert.ThrowsAsync<ArgumentException>(
                    () => _provider.DeleteAsync(objectName));

            _client.Verify(v => v.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ValidData()
        {
            var objectName = Guid.NewGuid().ToString();
            var bucket = Guid.NewGuid().ToString();

            _client.Setup(s => s.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _provider.DeleteAsync(objectName, bucket);

            _client.Verify(v => v.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
            _logger.Verify(
                 x => x.Log(
                     It.Is<LogLevel>(l => l == LogLevel.Debug),
                     It.IsAny<EventId>(),
                     It.Is<It.IsAnyType>((v, t) => v.ToString() == $"Удален объект '{objectName}' из bucket '{bucket}'"),
                     It.IsAny<Exception>(),
                     It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                 Times.Once);
        }

        [Fact]
        public async Task GetPresignedUrlAsync_InvalidArgs()
        {
            var objectName = string.Empty;

            await Assert.ThrowsAsync<ArgumentException>(
                    () => _provider.DeleteAsync(objectName));

            _client.Verify(v => v.PresignedGetObjectAsync(It.IsAny<PresignedGetObjectArgs>()), Times.Never);
        }

        [Fact]
        public async Task GetPresignedUrlAsync_ValidData()
        {
            var objectName = Guid.NewGuid().ToString();
            var bucket = Guid.NewGuid().ToString();
            var responseUrl = Guid.NewGuid().ToString(); 

            _client.Setup(s => s.PresignedGetObjectAsync(It.IsAny<PresignedGetObjectArgs>()))
                .Returns(Task.FromResult(responseUrl));

            var result = await _provider.GetPresignedUrlAsync(objectName, 40 , bucket);

            Assert.Equal(responseUrl, result);
            _client.Verify(v => v.PresignedGetObjectAsync(It.IsAny<PresignedGetObjectArgs>()), Times.Once);
        }
    }
}