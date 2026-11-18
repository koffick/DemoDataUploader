namespace DataUploader.Domain.Enums
{
    public enum ActionTypeEnum
    {
        /// <summary>
        /// Файл принят в работу.
        /// </summary>
        FileToProccess = 0,

        /// <summary>
        /// Данные обрабатываются.  
        /// </summary>
        FileDataParse = 1,

        /// <summary>
        /// Файл успешно обработан.
        /// </summary>
        FileDataParseSuccess = 2,

        /// <summary>
        /// В файл загружен частично.
        /// </summary>
        FileDataParseSuccessWithErrors = 3,

        /// <summary>
        /// В файле некорректные данные.
        /// </summary>
        FileDataParseFailure = 4
    }
}
