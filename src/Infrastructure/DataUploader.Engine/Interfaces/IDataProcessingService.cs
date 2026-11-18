using DataUploader.Domain.Enums;

namespace DataUploader.Engine.Interfaces
{
    public interface IDataProcessingService
    {
        public Task<ActionTypeEnum> Run(Stream data);
    }
}
