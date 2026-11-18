using DataUploader.Domain.Enums;
using DataUploader.Engine.Interfaces;

namespace DataUploader.Engine.Services
{
    public class DataProcessingService : IDataProcessingService
    {
        public Task<ActionTypeEnum> Run(Stream data)
        {
            throw new NotImplementedException();
        }
    }
}
