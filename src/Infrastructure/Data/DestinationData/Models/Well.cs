using System.ComponentModel.DataAnnotations;

namespace DestinationData.Models
{
    /// <summary>
    /// Модель скважины.
    /// </summary>
    public class Well
    {
        /// <summary>
        /// Идентификатор скважины в справочнике
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Название скважины
        /// </summary>
        public string WellName { get; set; }

        /// <summary>
        /// Номер скважины
        /// </summary>
        public int? WellNumber { get; set; }

        /// <summary>
        /// Идентификатор месторождения
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Идентификатор ответственной организации.
        /// </summary>
        public Guid OrgId { get; set; }

        /// <summary>
        /// Куст
        /// </summary>
        public string Cluster { get; set; }
    }
}
