
using System;
using System.ComponentModel.DataAnnotations;

namespace BSSRestPlanillaConso.Models
{
    public class FiltroTrafico
    {
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
        /// Consolidado
        /// </summary>
        public string CONSO { get; set; }
        /// <summary>
        /// Reporte
        /// </summary>
        public string REPORTE { get; set; }
        /// <summary>
        /// Planilla
        /// </summary>
        public string PLANILLA { get; set; }
        /// <summary>
        /// Placa
        /// </summary>
        public string PLACA { get; set; }
        /// <summary>
        /// Fecha en formato (yyyy-MM-dd HH:mm:ss)
        /// </summary>        
        public DateTime FECREP { get; set; }
        /// <summary>
        /// Código DANE
        /// </summary>
        public string CODDANE { get; set; }
        /// <summary>
        /// Código Novedad
        /// </summary>
        public string CODNOVEDAD { get; set; }
        /// <summary>
        /// Número
        /// </summary>
        public string NUMERO { get; set; }
        
    }
}