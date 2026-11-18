using DataUploader.Engine.Interfaces;
using DataUploader.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUploader.Engine.Tests.Services
{
    public class DataProcessingServiceTests
    {
        private DataProcessingService dataProcessingService;

        public DataProcessingServiceTests()
        {
            dataProcessingService = new DataProcessingService();
        }

        [Fact]
        public void IsInstance()
        {
            Assert.IsAssignableFrom<IDataProcessingService>(dataProcessingService);
        }
    }
}
