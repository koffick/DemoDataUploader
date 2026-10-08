using AutoMapper;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;

namespace AuditData
{
    public class AuditRepository : IAuditRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public AuditRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task SaveFileActionHistory(FileActionHistory action)
        {
            if (action != null)
            {
                var actionData = _mapper.Map<Models.FileActionHistory>(action);
                await _dataContext.FileActionHistory.AddAsync(actionData);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
