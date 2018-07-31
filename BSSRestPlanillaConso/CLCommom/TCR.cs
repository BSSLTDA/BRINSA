
namespace CLCommon
{
    /// <summary>
    /// INFORMACIÓN TRIBUTARIA, ADUANERA Y CAMBIARIA - ADQUIRIENTE
    /// </summary>
    public class TCR
    {
        /// <summary>
        /// Valores de la casilla 53 del RUT || Valores de la casilla 54 del RUT 
        /// Este campo se repetirá tantas veces(""n"") como la cantidad de valores registrados en el RUT.
        /// Use ""O-99"" si el Facturador Electrónico desconoce las obligaciones y responsabilidades del Adquiriente.
        /// Observación: se incluye el código suplementario “R-00-PN” para el adquiriente que no está obligado a registrarse en el RUT, para así cumplir con lo previsto en el numeral 3 del Artículo 7 del Decreto 2242-2015
        /// </summary>
        public string TCR_1 { get; set; }
        public string TCR_2 { get; set; }
        public string TCR_3 { get; set; }
        public string TCR_4 { get; set; }
        public string TCR_5 { get; set; }
        public string TCR_6 { get; set; }
        public string TCR_7 { get; set; }
        public string TCR_8 { get; set; }
        public string TCR_9 { get; set; }
        public string TCR_10 { get; set; }
        public string TCR_11 { get; set; }
    }
}
