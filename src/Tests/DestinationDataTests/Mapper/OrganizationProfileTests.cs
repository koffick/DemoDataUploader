using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DestinationData.Mapper;
using DataUploader.Domain.Models.Data;

namespace DestinationDataTests.Mapper
{
    public class OrganizationProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public OrganizationProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrganizationProfile());
            },
            new LoggerFactory());
            _mapper = new AutoMapper.Mapper(_configuration);
        }

        [Fact]
        public void Configuration_Must_Be_Valid()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void OrganizationToOrganization_Must_Map_Correctly()
        {
            var row = new Fixture().Create<DestinationData.Models.Organization>();
            var organization = _mapper.Map<Organization>(row);

            Assert.NotNull(organization);
            Assert.Equal(organization.Id, row.Id);
            Assert.Equal(organization.OrgName, row.OrgName);
            Assert.Equal(organization.OrgCode, row.OrgCode);
        }

        [Fact]
        public void OrganizationToOrganization_Revert_Must_Map_Correctly()
        {
            var row = new Fixture().Create<Organization>();
            var organization = _mapper.Map<DestinationData.Models.Organization>(row);

            Assert.NotNull(organization);
            Assert.Equal(organization.Id, row.Id);
            Assert.Equal(organization.OrgName, row.OrgName);
            Assert.Equal(organization.OrgCode, row.OrgCode);
        }
    }
}