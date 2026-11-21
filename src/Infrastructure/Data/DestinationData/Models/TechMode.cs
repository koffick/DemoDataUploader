using System.ComponentModel.DataAnnotations;

namespace DestinationData.Models
{
    /// <summary>
    /// Модель технического режима
    /// </summary>
    public class TechMode
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Отчетный месяц
        /// </summary>
        [Required]
        public DateTime ReportingMonth { get; set; }

        /// <summary>
        /// Идентификатор скважины
        /// </summary>
        [Required]
        public Guid WellId { get; set; }

        public Well Well { get; set; }

        /// <summary>
        /// Тип скважины
        /// </summary>
        public string WellType { get; set; }

        /// <summary>
        /// Список пластов
        /// </summary>
        public string Layers { get; set; }

        /// <summary>
        /// СЭ (Способ эксплуатации)
        /// </summary>
        public string ExploitationMode { get; set; }

        /// <summary>
        /// Тип насоса
        /// </summary>
        public string PumpType { get; set; }

        /// <summary>
        /// УВЧ
        /// </summary>
        public double? F { get; set; }

        /// <summary>
        /// Глубина спуска насоса
        /// </summary>
        public double? Hsp { get; set; }

        /// <summary>
        /// Максимальная глубина спуска насоса
        /// </summary>
        public double? HspMax { get; set; }

        /// <summary>
        /// Глубина забоя, м
        /// </summary>
        public double? BottomholeDepth { get; set; }

        /// <summary>
        /// Давление затрубное
        /// </summary>
        public double? Pzatr { get; set; }

        /// <summary>
        /// Давление на приеме
        /// </summary>
        public double? Pin { get; set; }

        /// <summary>
        /// Н д (Динамический уровень)
        /// </summary>
        public double? DynLevel { get; set; }

        /// <summary>
        /// Фактический режим \ Q нефти(Дебит нефти)
        /// </summary>
        public double? OilRate { get; set; }

        /// <summary>
        /// Фактический режим \ Q жидкости(Дебит жидкости)
        /// </summary>
        public double? LiqRate { get; set; }

        /// <summary>
        /// Фактический режим \ Обводненность(Объемная обводненность)
        /// </summary>
        public double? WaterCut { get; set; }

        /// <summary>
        /// Плотность нефти
        /// </summary>
        public double? OilDensity { get; set; }

        /// <summary>
        /// Состояние на конец месяца
        /// </summary>
        public string Status { get; set; }
    }
}
