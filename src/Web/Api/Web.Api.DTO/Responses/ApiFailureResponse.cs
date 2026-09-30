namespace Web.Api.DTO.Responses
{
    /// <summary>
    /// Cтандартизированный ответ API об ошибке.
    /// </summary>
    public class ApiFailureResponse
    {
        /// <summary>
        /// Конструктор <see cref="ApiFailureResponse"/>.
        /// </summary>
        /// <param name="errorMessage">Информация об ошибке.</param>
        public ApiFailureResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Сообщение, содержащее информацию об ошибке.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
