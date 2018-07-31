
namespace CLCommon
{
    /// <summary>
    /// IMPORTES TOTALES
    /// </summary>
    public class TOT
    {
        /// <summary>
        /// Total Importe bruto antes de impuestos. 
        /// Total importe bruto, suma de los importes brutos de las líneas de la factura.
        /// </summary>
        public decimal TOT_1 { get; set; }
        /// <summary>
        /// Moneda del importe bruto antes de impuestos
        /// </summary>
        public string TOT_2 { get; set; }
        /// <summary>
        /// Total Base Imponible para el cálculo de los impuestos.
        /// (Importe Bruto+Cargos-Descuentos)
        /// </summary>
        public decimal TOT_3 { get; set; }
        /// <summary>
        /// Moneda del total Base Imponible 
        /// </summary>
        public string TOT_4 { get; set; }
        /// <summary>
        /// Total de la Factura.
        /// (Total importe bruto + Total Impuestos - Total Impuesto Retenidos)
        /// </summary>
        public decimal TOT_5 { get; set; }
        /// <summary>
        /// Moneda del total de la factura
        /// </summary>
        public string TOT_6 { get; set; }
        public string TOT_7 { get; set; }
        public string TOT_8 { get; set; }
        public string TOT_9 { get; set; }
        public string TOT_10 { get; set; }
        public string TOT_11 { get; set; }
        public string TOT_12 { get; set; }
        public string TOT_13 { get; set; }
        public string TOT_14 { get; set; }
    }
}
