using System.ComponentModel.DataAnnotations;

namespace BSSRestPlanillaConso.Models
{
    /// <summary>
    /// Filtro para generación de notas crédito
    /// </summary>
    public class FiltroNCE
    {
        /// <summary>
        /// Cargar, Estado, Descargar
        /// </summary>
        [Required]
        public string ACCION { get; set; }
        /// <summary>
        /// Prefijo
        /// </summary>
        public string PREFIJO { get; set; }
        /// <summary>
        /// Nota
        /// </summary>
        public string NOTA { get; set; }
        /// <summary>
        /// Id Transaccion
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Usuario (Opcional)
        /// </summary>
        public string USUARIO { get; set; }
        /// <summary>
        /// Contraseña (Opcional)
        /// </summary>
        public string PASSWORD { get; set; }
    }
}