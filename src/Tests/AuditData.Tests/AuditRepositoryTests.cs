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
            var UserAction = new Fixture().Create<UserActionHistory>();
 
            await _auditRepository.SaveUserActionHistory(UserAction);
            Assert.Equal(_dataContext.UserActionHistory.Count(), 1);
            var UserActionData = _dataContext.UserActionHistory.FirstOrDefault();
            Assert.Equal(UserActionData.FileId, UserAction.FileId);
        }
    }
}
