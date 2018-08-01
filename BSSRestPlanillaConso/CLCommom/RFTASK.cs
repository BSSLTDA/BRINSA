using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLCommom
{
    public class RFTASK
    {
        [Key]
        [Display(Name = "TNUMREG")]
        public string TNUMREG { get; set; }
        [Display(Name = "TCATEG")]
        public string TCATEG { get; set; }
        [Display(Name = "TDESC")]
        public string TDESC { get; set; }
        [Display(Name = "TLXMSG")]
        public string TLXMSG { get; set; }
        [Display(Name = "TTIPO")]
        public string TTIPO { get; set; }
        [Display(Name = "TKEY")]
        public string TKEY { get; set; }
        [Display(Name = "TPGM")]
        public string TPGM { get; set; }
        [Display(Name = "TPRM")]
        public string TPRM { get; set; }
        [Display(Name = "TMSG")]
        public string TMSG { get; set; }
        [Display(Name = "FWRKID")]
        public string FWRKID { get; set; }
        [Display(Name = "STS1")]
        public string STS1 { get; set; }
        [Display(Name = "STS2")]
        public string STS2 { get; set; }
        [Display(Name = "STS3")]
        public string STS3 { get; set; }
        [Display(Name = "STS4")]
        public string STS4 { get; set; }
        [Display(Name = "STS5")]
        public string STS5 { get; set; }
        [Display(Name = "TCRTDAT")]
        public string TCRTDAT { get; set; }
        [Display(Name = "TCRTUSR")]
        public string TCRTUSR { get; set; }
        [Display(Name = "TUPDDAT")]
        public string TUPDDAT { get; set; }
        [Display(Name = "TURL")]
        public string TURL { get; set; }
        [Display(Name = "TNUMTAREA")]
        public string TNUMTAREA { get; set; }
        [Display(Name = "TSUBCAT")]
        public string TSUBCAT { get; set; }
        [Display(Name = "TFRECUENCIA")]
        public string TFRECUENCIA { get; set; }
        [Display(Name = "TFPROXIMA")]
        public string TFPROXIMA { get; set; }
        [Display(Name = "THLD")]
        public string THLD { get; set; }
        [Display(Name = "TKEYWORD")]
        public string TKEYWORD { get; set; }
        [Display(Name = "TJOB400")]
        public string TJOB400 { get; set; }
        [Display(Name = "TURLTIT")]
        public string TURLTIT { get; set; }
        [Display(Name = "TCMD")]
        public string TCMD { get; set; }
        [Display(Name = "TAMBIENTE")]
        public string TAMBIENTE { get; set; }
        [Display(Name = "TJOBD")]
        public string TJOBD { get; set; }
        [Display(Name = "TJOBQ")]
        public string TJOBQ { get; set; }
    }
}
