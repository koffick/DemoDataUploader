using System.ComponentModel.DataAnnotations;

namespace UserData.Models
{
    /// <summary>
    /// Модель хранения данных о пользователе.
    /// </summary>
    internal class User
    {
        [Key]
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string LastName { get; set; }
    }
}
