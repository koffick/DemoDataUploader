using AutoFixture;
using AutoMapper;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models.Data;
using DestinationData;
using DestinationData.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DestinationDataTests
{
    public class DestinationDataRepositoryTests
    {
        private DestinationDataRepository _repository;
        private DataContext _dataContext;

        public DestinationDataRepositoryTests()
        {
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("DataRepositoryTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

            _dataContext = new DataContext(contextOptions);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<FieldProfile>();
                cfg.AddProfile<OrganizationProfile>();
                cfg.AddProfile<WellProfile>();
                cfg.AddProfile<TechModeProfile>();
            },
            new LoggerFactory());
            var mapper = new AutoMapper.Mapper(configuration);

            _repository = new DestinationDataRepository(_dataContext, mapper);
        }

        [Fact]
        public void IsInstance()
        {
            Assert.IsAssignableFrom<IDestinationDataRepository>(_repository);
        }

        [Fact]
        public async Task FindField_Must_Be_Return_Field()
        {
            var field = new Fixture().Create<DestinationData.Models.Field>();
            _dataContext.Add(field);
            _dataContext.SaveChanges();

            var response = await _repository.FindFieldAsync(field.FieldName);
            Assert.NotNull(response);
            Assert.Equal(response.Id, field.Id);
        }


        [Fact]
        public async Task FindField_Must_Be_Return_Null_If_Not_Found()
        {
            var response = await _repository.FindFieldAsync("nonexistent-field-name");
            Assert.Null(response);
        }

        [Fact]
        public async Task SaveField_Must_Be_Compete()
        {
            var field = new Fixture().Create<Field>();
            _dataContext.Fields.RemoveRange(_dataContext.Fields);

            var response = await _repository.SaveFieldAsync(field);
            Assert.NotNull(response);
            Assert.Equal(response, field.Id);
            Assert.Equal(_dataContext.Fields.Count(), 1);
            var fieldData = _dataContext.Fields.FirstOrDefault();
            Assert.Equal(fieldData.Id, field.Id);
        }

        [Fact]
        public async Task SaveField_Must_Be_Return_NewId()
        {
            var field = new Fixture()
                .Build<Field>()
                .Without(w => w.Id)
                .Create();

            var response = await _repository.SaveFieldAsync(field);
            Assert.NotNull(response);
            Assert.NotEqual(response, field.Id);
        }

        [Fact]
        public async Task FindOrganization_Must_Be_Return_Organization()
        {
            var organization = new Fixture().Create<DestinationData.Models.Organization>();
            _dataContext.Add(organization);
            _dataContext.SaveChanges();

            var response = await _repository.FindOrganizationAsync(organization.OrgName);
            Assert.NotNull(response);
            Assert.Equal(response.Id, organization.Id);
        }

        [Fact]
        public async Task FindOrganization_Must_Be_Return_Null_If_Not_Found()
        {
            var response = await _repository.FindOrganizationAsync("nonexistent-org-name");
            Assert.Null(response);
        }

        [Fact]
        public async Task SaveOrganization_Must_Be_Compete()
        {
            var organization = new Fixture().Create<Organization>();
            _dataContext.Organizations.RemoveRange(_dataContext.Organizations);

            var response = await _repository.SaveOrganizationAsync(organization);
            Assert.NotNull(response);
            Assert.Equal(response, organization.Id);
            Assert.Equal(_dataContext.Organizations.Count(), 1);
            var organizationData = _dataContext.Organizations.FirstOrDefault();
            Assert.Equal(organizationData.Id, organization.Id);
        }

        [Fact]
        public async Task SaveOrganization_Must_Be_Return_NewId()
        {
            var organization = new Fixture()
                .Build<Organization>()
                .Without(w => w.Id)
                .Create();

            var response = await _repository.SaveOrganizationAsync(organization);
            Assert.NotNull(response);
            Assert.NotEqual(response, organization.Id);
        }

        [Fact]
        public async Task FindWell_Must_Be_Return_Well()
        {
            var well = new Fixture()
                .Build<DestinationData.Models.Well>()
                .Without(w => w.Field)
                .Without(w => w.Organization)
                .Create();
            _dataContext.Add(well);
            _dataContext.SaveChanges();

            var response = await _repository.FindWellAsync(well.WellName);
            Assert.NotNull(response);
            Assert.Equal(response.Id, well.Id);
        }

        [Fact]
        public async Task FindWell_Must_Be_Return_Null_If_Not_Found()
        {
            var response = await _repository.FindWellAsync("nonexistent-well-name");
            Assert.Null(response);
        }

        [Fact]
        public async Task SaveWell_Must_Be_Compete()
        {
            var well = new Fixture()
                .Build<Well>()
                .Create();
            _dataContext.Wells.RemoveRange(_dataContext.Wells);

            var response = await _repository.SaveWellAsync(well);
            Assert.NotNull(response);
            Assert.Equal(response, well.Id);
            Assert.Equal(_dataContext.Wells.Count(), 1);
            var wellData = _dataContext.Wells.FirstOrDefault();
            Assert.Equal(wellData.Id, well.Id);
        }

        [Fact]
        public async Task SaveWell_Must_Be_Return_NewId()
        {
            var well = new Fixture()
                .Build<Well>()
                .Without(w => w.Id)
                .Create();

            var response = await _repository.SaveWellAsync(well);
            Assert.NotNull(response);
            Assert.NotEqual(response, well.Id);
        }

        [Fact]
        public async Task FindTechMode_Must_Be_Return_TechMode()
        {
            var techMode = new Fixture()
                .Build<DestinationData.Models.TechMode>()
                .Without(t => t.Well)
                .Create();
            _dataContext.Add(techMode);
            _dataContext.SaveChanges();

            var response = await _repository.FindTechModeAsync(techMode.WellId, techMode.ReportingMonth);
            Assert.NotNull(response);
            Assert.Equal(response.Id, techMode.Id);
            Assert.Equal(response.WellId, techMode.WellId);
            Assert.Equal(response.ReportingMonth, techMode.ReportingMonth);
        }

        [Fact]
        public async Task FindTechMode_Must_Be_Return_Null_If_Not_Found()
        {
            var response = await _repository.FindTechModeAsync(Guid.NewGuid(), DateTime.UtcNow);
            Assert.Null(response);
        }

        [Fact]
        public async Task SaveTechMode_Must_Be_Compete()
        {
            var techMode = new Fixture()
                .Build<TechMode>()
                .Create();
            _dataContext.TechModes.RemoveRange(_dataContext.TechModes);

            var response = await _repository.SaveTechModeAsync(techMode);
            Assert.NotNull(response);
            Assert.Equal(response, techMode.Id);
            Assert.Equal(_dataContext.TechModes.Count(), 1);

            var techModeData = _dataContext.TechModes.FirstOrDefault();
            Assert.Equal(techModeData.Id, techMode.Id);
            Assert.Equal(techModeData.WellId, techMode.WellId);
            Assert.Equal(techModeData.ReportingMonth, techMode.ReportingMonth);
            Assert.Equal(techModeData.WellType, techMode.WellType);
            Assert.Equal(techModeData.Layers, techMode.Layers);
            Assert.Equal(techModeData.ExploitationMode, techMode.ExploitationMode);
            Assert.Equal(techModeData.PumpType, techMode.PumpType);
            Assert.Equal(techModeData.F, techMode.F);
            Assert.Equal(techModeData.Hsp, techMode.Hsp);
            Assert.Equal(techModeData.HspMax, techMode.HspMax);
            Assert.Equal(techModeData.BottomholeDepth, techMode.BottomholeDepth);
            Assert.Equal(techModeData.Pzatr, techMode.Pzatr);
            Assert.Equal(techModeData.Pin, techMode.Pin);
            Assert.Equal(techModeData.DynLevel, techMode.DynLevel);
            Assert.Equal(techModeData.OilRate, techMode.OilRate);
            Assert.Equal(techModeData.LiqRate, techMode.LiqRate);
            Assert.Equal(techModeData.WaterCut, techMode.WaterCut);
            Assert.Equal(techModeData.OilDensity, techMode.OilDensity);
            Assert.Equal(techModeData.Status, techMode.Status);
        }

        [Fact]
        public async Task SaveTechMode_Must_Be_Return_NewId()
        {
            var techMode = new Fixture()
                .Build<TechMode>()
                .Without(t => t.Id)
                .Create();

            var response = await _repository.SaveTechModeAsync(techMode);
            Assert.NotNull(response);
            Assert.NotEqual(response, techMode.Id); // techMode.Id == Guid.Empty
        }
    }
}
