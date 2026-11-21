using AutoMapper;
using Microsoft.Extensions.Logging;
using AutoFixture;
using DestinationData.Mapper;
using DataUploader.Domain.Models.Data;

namespace DestinationDataTests.Mapper
{
    public class FieldProfileTests
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public FieldProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new FieldProfile());
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
        public void FieldToField_Must_Map_Correctly()
        {
            var row = new Fixture().Create<DestinationData.Models.Field>();
            var field = _mapper.Map<Field>(row);

            Assert.NotNull(field);
            Assert.Equal(field.Id, row.Id);
            Assert.Equal(field.FieldCode, row.FieldCode);
            Assert.Equal(field.FieldName, row.FieldName);
        }

        [Fact]
        public void FieldToField_Revert_Must_Map_Correctly()
        {
            var row = new Fixture().Create<Field>();
            var field = _mapper.Map<DestinationData.Models.Field>(row);

            Assert.NotNull(field);
            Assert.Equal(field.Id, row.Id);
            Assert.Equal(field.FieldCode, row.FieldCode);
            Assert.Equal(field.FieldName, row.FieldName);
        }
    }
}