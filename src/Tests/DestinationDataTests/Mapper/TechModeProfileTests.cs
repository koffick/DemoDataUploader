using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DestinationData.Mapper;
using DataUploader.Domain.Models.Data;

namespace DestinationDataTests.Mapper
{
    public class TechModeProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public TechModeProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TechModeProfile());
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
        public void TechModeToTechMode_Must_Map_Correctly()
        {
            var row = new Fixture().Create<DestinationData.Models.TechMode>();
            var techMode = _mapper.Map<TechMode>(row);

            Assert.NotNull(techMode);
            Assert.Equal(techMode.Id, row.Id);
            Assert.Equal(techMode.ReportingMonth, row.ReportingMonth);
            Assert.Equal(techMode.WellId, row.WellId);
            Assert.Equal(techMode.WellType, row.WellType);
            Assert.Equal(techMode.Layers, row.Layers);
            Assert.Equal(techMode.ExploitationMode, row.ExploitationMode);
            Assert.Equal(techMode.PumpType, row.PumpType);
            Assert.Equal(techMode.F, row.F);
            Assert.Equal(techMode.Hsp, row.Hsp);
            Assert.Equal(techMode.HspMax, row.HspMax);
            Assert.Equal(techMode.BottomholeDepth, row.BottomholeDepth);
            Assert.Equal(techMode.Pzatr, row.Pzatr);
            Assert.Equal(techMode.Pin, row.Pin);
            Assert.Equal(techMode.DynLevel, row.DynLevel);
            Assert.Equal(techMode.OilRate, row.OilRate);
            Assert.Equal(techMode.LiqRate, row.LiqRate);
            Assert.Equal(techMode.WaterCut, row.WaterCut);
            Assert.Equal(techMode.OilDensity, row.OilDensity);
            Assert.Equal(techMode.Status, row.Status);
        }

        [Fact]
        public void TechModeToTechMode_Revert_Must_Map_Correctly()
        {
            var row = new Fixture().Create<TechMode>();
            var techMode = _mapper.Map<DestinationData.Models.TechMode>(row);

            Assert.NotNull(techMode);
            Assert.Equal(techMode.Id, row.Id);
            Assert.Equal(techMode.ReportingMonth, row.ReportingMonth);
            Assert.Equal(techMode.WellId, row.WellId);
            Assert.Equal(techMode.WellType, row.WellType);
            Assert.Equal(techMode.Layers, row.Layers);
            Assert.Equal(techMode.ExploitationMode, row.ExploitationMode);
            Assert.Equal(techMode.PumpType, row.PumpType);
            Assert.Equal(techMode.F, row.F);
            Assert.Equal(techMode.Hsp, row.Hsp);
            Assert.Equal(techMode.HspMax, row.HspMax);
            Assert.Equal(techMode.BottomholeDepth, row.BottomholeDepth);
            Assert.Equal(techMode.Pzatr, row.Pzatr);
            Assert.Equal(techMode.Pin, row.Pin);
            Assert.Equal(techMode.DynLevel, row.DynLevel);
            Assert.Equal(techMode.OilRate, row.OilRate);
            Assert.Equal(techMode.LiqRate, row.LiqRate);
            Assert.Equal(techMode.WaterCut, row.WaterCut);
            Assert.Equal(techMode.OilDensity, row.OilDensity);
            Assert.Equal(techMode.Status, row.Status);
        }
    }
}