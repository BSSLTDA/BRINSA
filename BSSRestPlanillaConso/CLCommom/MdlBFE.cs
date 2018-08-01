
namespace CLCommon
{
    /// <summary>
    /// INFORMACIÓN BANCO DEL BENEFICIARIO DEL EMISOR
    /// </summary>
    public class MdlBFE
    {
        /// <summary>
        /// Número de la cuenta
        /// </summary>
        public string BFE_1 { get; set; }
        /// <summary>
        /// Nombre del titular de la cuenta
        /// </summary>
        public string BFE_2 { get; set; }
        /// <summary>
        /// Tipo de cuenta
        /// CCTE-Cuenta Corriente
        /// CAHO-Cuenta de Ahorros
        /// </summary>
        public string BFE_3 { get; set; }
        /// <summary>
        /// Identificación de la Institución Financiera - Código de Compensación
        /// </summary>
        public string BFE_4 { get; set; }
        public string BFE_5 { get; set; }
    }
}
