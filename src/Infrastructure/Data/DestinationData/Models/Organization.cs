using System.ComponentModel.DataAnnotations;

namespace DestinationData.Models
{
    /// <summary>
    /// Модель организации.
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// Идентификатор органмзации.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [Required]
        public string OrgName { get; set; }

        /// <summary>
        /// Код.
        /// </summary>
        public string OrgCode { get; set; }
    }
}
