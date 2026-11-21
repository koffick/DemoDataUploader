namespace DataUploader.Domain.Models.Data
{
    /// <summary>
    /// Модель организации.
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// Идентификатор органмзации.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// Код.
        /// </summary>
        public string OrgCode { get; set; }
    }
}
