using AutoMapper;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DestinationData
{
    /// <inheritdoc cref="IDestinationDataRepository"/>>
    public class DestinationDataRepository : IDestinationDataRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DestinationDataRepository(DataContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc cref="IDestinationDataRepository.FindFieldAsync(string)"/>
        public async Task<Field> FindFieldAsync(string fieldName)
        {
            var field = await _context.Fields.FirstOrDefaultAsync(f => f.FieldName == fieldName);
            return _mapper.Map<Field>(field);
        }

        /// <inheritdoc cref="IDestinationDataRepository.SaveFieldAsync(Field)"/>
        public async Task<Guid> SaveFieldAsync(Field field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));                   
            }
            var dataToSave = _mapper.Map<Models.Field>(field);
            await _context.AddAsync(dataToSave);
            await _context.SaveChangesAsync();
            return dataToSave.Id;
        }

        /// <inheritdoc cref="IDestinationDataRepository.FindOrganizationAsync(string)"/>
        public async Task<Organization> FindOrganizationAsync(string orgName)
        {
            var organization = await _context.Organizations.FirstOrDefaultAsync(f => f.OrgName == orgName);
            return _mapper.Map<Organization>(organization);
        }

        public async Task<Guid> SaveOrganizationAsync(Organization organization)
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }
            var dataToSave = _mapper.Map<Models.Organization>(organization);
            await _context.AddAsync(dataToSave);
            await _context.SaveChangesAsync();
            return dataToSave.Id;
        }

        /// <inheritdoc cref="IDestinationDataRepository.FindWellAsync(string)"/>
        public async Task<Well> FindWellAsync(string wellName)
        {
            var well = await _context.Wells.FirstOrDefaultAsync(w => w.WellName == wellName);
            return _mapper.Map<Well>(well);
        }

        /// <inheritdoc cref="IDestinationDataRepository.SaveWellAsync(Well)"/>
        public async Task<Guid> SaveWellAsync(Well well)
        {
            if (well == null)
            {
                throw new ArgumentNullException(nameof(well));
            }
            var dataToSave = _mapper.Map<Models.Well>(well);
            await _context.AddAsync(dataToSave);
            await _context.SaveChangesAsync();
            return dataToSave.Id;
        }

        /// <inheritdoc cref="IDestinationDataRepository.FindTechModeAsync(Guid, DateTime)"/>
        public async Task<TechMode> FindTechModeAsync(Guid wellId, DateTime reportingMonth)
        {
            var techMode = await _context.TechModes
                .FirstOrDefaultAsync(t => t.WellId == wellId && t.ReportingMonth == reportingMonth);
            return _mapper.Map<TechMode>(techMode);
        }

        /// <inheritdoc cref="IDestinationDataRepository.SaveTechModeAsync(TechMode)"/>
        public async Task<Guid> SaveTechModeAsync(TechMode techMode)
        {
            if (techMode == null)
            {
                throw new ArgumentNullException(nameof(techMode));
            }
            var dataToSave = _mapper.Map<Models.TechMode>(techMode);
            await _context.AddAsync(dataToSave);
            await _context.SaveChangesAsync();
            return dataToSave.Id;
        }
    }
}
