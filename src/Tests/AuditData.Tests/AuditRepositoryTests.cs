using AuditData.Mapper;
using AutoFixture;
using AutoMapper;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AuditData.Tests
{
    public class AuditRepositoryTests
    {
        private AuditRepository _auditRepository;
        private DataContext _dataContext;

        public AuditRepositoryTests()
        {
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("AuditRepositoryTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

            _dataContext = new DataContext(contextOptions);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FileActionAuditProfile());
                cfg.AddProfile(new UserActionAuditProfile());
            },
            new LoggerFactory());
            var mapper = new AutoMapper.Mapper(configuration);

            _auditRepository = new AuditRepository(_dataContext, mapper);
        }

        [Fact]
        public void IsInstance_True()
        {
            Assert.IsAssignableFrom<IAuditRepository>(_auditRepository);
        }

        [Fact]
        public async Task SaveUserAction_Must_Be_Return_User()
        {
            var userAction = new Fixture().Create<UserActionHistory>();
 
            await _auditRepository.SaveUserActionHistory(userAction);
            Assert.Equal(_dataContext.UserActionHistory.Count(), 1);
            var userActionData = _dataContext.UserActionHistory.FirstOrDefault();
            Assert.Equal(userActionData.FileId, userAction.FileId);
        }

        [Fact]
        public async Task SaveFileAction_Must_Be_Return_User()
        {
            var fileAction = new Fixture().Create<FileActionHistory>();

            await _auditRepository.SaveFileActionHistory(fileAction);
            Assert.Equal(_dataContext.FileActionHistory.Count(), 1);
            var fileActionData = _dataContext.FileActionHistory.FirstOrDefault();
            Assert.Equal(fileActionData.FileId, fileAction.FileId);
        }
    }
}
