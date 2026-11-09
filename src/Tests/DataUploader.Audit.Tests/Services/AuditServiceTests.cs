using AutoFixture;
using DataUploader.Audit.Interfaces;
using DataUploader.Audit.Services;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;


namespace DataUploader.Audit.Tests.Services
{
    public class AuditServiceTests
    {
        private AuditService _auditService;
        private Mock<IAuditRepository> _auditRepository;
        private Mock<ILogger<AuditService>> _logger;

        public AuditServiceTests() 
        {
            _auditRepository = new Mock<IAuditRepository>();
            _logger = new Mock<ILogger<AuditService>>();
            _auditService = new AuditService(_auditRepository.Object, _logger.Object);
        }

        [Fact]
        public void IsInstanceOf()
        {
            Assert.IsAssignableFrom<IAuditService>(_auditService);
        }

        [Fact]
        public async Task UserActionSaveToDBAsync_IsComplete()
        {
            UserActionHistory userActionHistory = new Fixture().Build<UserActionHistory>()
                .Without(w => w.Id)
                .Create();
            _auditRepository.Setup(s => s.SaveUserActionHistory(It.IsAny<UserActionHistory>()))
                .Callback<UserActionHistory>(cb => cb.Id = cb.Id ?? Guid.NewGuid());
            var res = await _auditService.UserActionSaveToDBAsync(userActionHistory);
            Assert.NotEqual(res, default);
            Assert.Equal(res, userActionHistory.Id);
            _auditRepository.Verify(v => v.SaveUserActionHistory(userActionHistory), Times.Once);
        }

        [Fact]
        public async Task FileActionSaveToDBAsync_IsComplete()
        {
            FileActionHistory fileActionHistory = new Fixture().Build<FileActionHistory>()
                .Without(w => w.Id)
                .Create();
            _auditRepository.Setup(s => s.SaveFileActionHistory(It.IsAny<FileActionHistory>()))
                .Callback<FileActionHistory>(cb => cb.Id = cb.Id ?? Guid.NewGuid());
            var res = await _auditService.FileActionSaveToDBAsync(fileActionHistory);
            Assert.NotEqual(res, default);
            Assert.Equal(res, fileActionHistory.Id);
            _auditRepository.Verify(v => v.SaveFileActionHistory(fileActionHistory), Times.Once);
        }
    }
}
