using System.ComponentModel.DataAnnotations;

namespace BSSRestPlanillaConso.Models
{
    public class FiltroServicios
    {
        /// <summary>
        /// Id Tarea
        /// </summary>
        public string IDTAREA { get; set; }
        /// <summary>
        /// Esperar Respuesta
        /// </summary>
        public string ESPERARRESPUESTA { get; set; }
        /// <summary>
        /// NuevoTrafico, EnTransito, Reportar
        /// </summary>
        [Required]
        public string ACCION { get; set; }
        /// <summary>
        /// Usuario
        /// </summary>
        [Required]
        public string USUARIO { get; set; }
        /// <summary>
        /// Contraseña
        /// </summary>
        [Required]
        public string PASSWORD { get; set; }
        /// <summary>
        /// Resultado
        /// </summary>        
        public RespuestaSometer400 RESULTADO { get; set; }
    }
}