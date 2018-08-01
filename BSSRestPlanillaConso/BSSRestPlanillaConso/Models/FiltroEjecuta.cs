using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BSSRestPlanillaConso.Models
{
    public class FiltroEjecuta
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string que { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string xplanilla { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string phora { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string preporte { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string psender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rega { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ok { get; set; }
    }
}