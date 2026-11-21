using System.ComponentModel.DataAnnotations;

namespace DestinationData.Models
{
    /// <summary>
    /// Модель месторождения.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Bидентификатор месторождения.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Код месторождения
        /// </summary>
        public int? FieldCode { get; set; }

        /// <summary>
        /// Наименование месторождения
        /// </summary>
        public string FieldName { get; set; }
    }
}
