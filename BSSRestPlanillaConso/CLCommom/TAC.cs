
namespace CLCommon
{
    /// <summary>
    /// INFORMACIÓN TRIBUTARIA, ADUANERA Y CAMBIARIA - EMISOR
    /// </summary>
    public class TAC
    {
        /// <summary>
        /// Valores de la casilla 53 del RUT || Valores de la casilla 54 del RUT 
        /// Este campo se repetirá tantas veces(""n"") como la cantidad de valores registrados en el RUT.Ver Nota 24
        /// </summary>
        public string TAC_1 { get; set; }
        public string TAC_2 { get; set; }
        public string TAC_3 { get; set; }
        public string TAC_4 { get; set; }
        public string TAC_5 { get; set; }
        public string TAC_6 { get; set; }
        public string TAC_7 { get; set; }
        public string TAC_8 { get; set; }
        public string TAC_9 { get; set; }
        public string TAC_10 { get; set; }
        public string TAC_11 { get; set; }
    }
}
