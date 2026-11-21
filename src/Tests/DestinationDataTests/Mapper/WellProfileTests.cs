using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DestinationData.Mapper;
using DataUploader.Domain.Models.Data;

namespace DestinationDataTests.Mapper
{
    public class WellProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public WellProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WellProfile());
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
        public void WellToWell_Must_Map_Correctly()
        {
            var row = new Fixture().Create<DestinationData.Models.Well>();
            var well = _mapper.Map<Well>(row);

            Assert.NotNull(well);
            Assert.Equal(well.Id, row.Id);
            Assert.Equal(well.Cluster, row.Cluster);
            Assert.Equal(well.WellName, row.WellName);
            Assert.Equal(well.WellNumber, row.WellNumber);
            Assert.Equal(well.OrgId, row.OrgId);
            Assert.Equal(well.FieldId, row.FieldId);
        }

        [Fact]
        public void WellToWell_Revert_Must_Map_Correctly()
        {
            var row = new Fixture().Create<Well>();
            var well = _mapper.Map<DestinationData.Models.Well>(row);

            Assert.NotNull(well);
            Assert.Equal(well.Id, row.Id);
            Assert.Equal(well.Cluster, row.Cluster);
            Assert.Equal(well.WellName, row.WellName);
            Assert.Equal(well.WellNumber, row.WellNumber);
            Assert.Equal(well.OrgId, row.OrgId);
            Assert.Equal(well.FieldId, row.FieldId);
        }
    }
}