
namespace CLCommon
{
    /// <summary>
    /// DOCUMENTOS DE REFERENCIA
    /// </summary>
    public class REF
    {
        /// <summary>
        /// Referencia para uso interno del emisor.
        /// Información del emisor para filtros y visualización en el portal web de factura electrónica de Carvajal, solo se permite una sola repetición.
        /// Se recomienda en este campo como máximo enviar 20 caracteres para su correcta visualización en el sitio web.
        /// Referencia para uso interno del emisor.
        /// </summary>
        public string REF_1 { get; set; }
        /// <summary>
        /// Numero del documento que se referencia, asignado por el emisor.
        /// </summary>
        public string REF_2 { get; set; }
        public string REF_3 { get; set; }
    }
}
