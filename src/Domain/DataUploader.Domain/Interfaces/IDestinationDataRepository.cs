using DataUploader.Domain.Models.Data;

namespace DataUploader.Domain.Interfaces;

/// <summary>
/// Класс-хранилище целевых данных.
/// </summary>
public interface IDestinationDataRepository
{
    /// <summary>
    /// Находит месторождение по названию.
    /// </summary>
    /// <param name="fieldName">Название месторождения.</param>
    /// <returns>Месторождение.</returns>
    Task<Field> FindFieldAsync(string fieldName);

    /// <summary>
    /// Сохраняет месторождение.
    /// </summary>
    /// <param name="field">Месторождение.</param>
    /// <returns>Идентификатор сохраненной записи.</returns>
    Task<Guid> SaveFieldAsync(Field field);    
    
    /// <summary>
    /// Находит организацию по названию.
    /// </summary>
    /// <param name="orgName">Название организации.</param>
    /// <returns>Организация.</returns>
    Task<Organization> FindOrganizationAsync(string orgName);

    /// <summary>
    /// Сохраняет организацию.
    /// </summary>
    /// <param name="organization">Организация.</param>
    /// <returns>Идентификатор сохраненной записи.</returns>
    Task<Guid> SaveOrganizationAsync(Organization organization);

    /// <summary>
    /// Находит скважину по названию.
    /// </summary>
    /// <param name="wellName">Название скважины.</param>
    /// <returns>Скважина.</returns>
    Task<Well> FindWellAsync(string wellName);

    /// <summary>
    /// Сохраняет скважину.
    /// </summary>
    /// <param name="well">Скважина.</param>
    /// <returns>Идентификатор сохраненной записи.</returns>
    Task<Guid> SaveWellAsync(Well well);

    /// <summary>
    /// Находит запись технического режима по идентификатору скважины и отчётному месяцу.
    /// </summary>
    /// <param name="wellId">Идентификатор скважины.</param>
    /// <param name="reportingMonth">Отчётный месяц.</param>
    /// <returns>Запись технического режима.</returns>
    Task<TechMode> FindTechModeAsync(Guid wellId, DateTime reportingMonth);

    /// <summary>
    /// Сохраняет запись технического режима.
    /// </summary>
    /// <param name="techMode">Запись технического режима.</param>
    /// <returns>Идентификатор сохраненной записи.</returns>
    Task<Guid> SaveTechModeAsync(TechMode techMode);
}
