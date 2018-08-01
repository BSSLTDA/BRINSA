
namespace CLCommon
{
    /// <summary>
    /// DATOS RESOLUCIÓN DE NUMERACIÓN DE FACTURAS DIAN
    /// </summary>
    public class DRF
    {
        /// <summary>
        /// Número autorización.
        /// Número o código de la resolución otorgada para la numeración.
        /// </summary>
        public string DRF_1 { get; set; }
        /// <summary>
        /// Fecha de inicio del período de autorización de la numeración.
        /// Formato AAAA-MM-DD
        /// </summary>
        public string DRF_2 { get; set; }
        public string DRF_3 { get; set; }
        /// <summary>
        /// Prefijo del rango de numeración.
        /// </summary>
        public string DRF_4 { get; set; }
        /// <summary>
        /// Rango de Numeración (mínimo).
        /// Rango de numeración otorgado, valor mínimo y máximo
        /// </summary>
        public string DRF_5 { get; set; }
        /// <summary>
        /// Rango de Numeración (máximo)
        /// Rango de numeración otorgado, valor mínimo y máximo
        /// </summary>
        public string DRF_6 { get; set; }
    }
}
